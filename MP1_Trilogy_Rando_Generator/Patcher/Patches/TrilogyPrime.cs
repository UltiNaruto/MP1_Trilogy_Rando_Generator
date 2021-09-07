using ppcasm_cs;
using System;
using System.Collections.Generic;

namespace MP1_Trilogy_Rando_Generator.Patcher.Patches
{
    abstract class TrilogyPrime : Patcher
    {
        internal virtual String MP1_Dol_Path { get; }
        internal virtual String FE_Dol_Path { get; }
        internal virtual String Main_Dol_Path { get; }

        protected Addresses _addresses = null;
        internal Addresses addresses { get => _addresses; }

        void SetStartingArea(Enums.SpawnRoom spawnRoom)
        {
            UInt16 MLVL_H = (UInt16)((spawnRoom.MLVL & 0xFFFF0000) >> 16);
            UInt16 MLVL_L = (UInt16)(spawnRoom.MLVL & 0xFFFF);

            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            List<PPC_Instruction> instructions = new List<PPC_Instruction>();

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("__sinit_CFrontEndUI_cpp") + 0xc,
                "lis r4, " + String.Format("0x{0:X4}", MLVL_H),
                "subi r3, r5, 0xbe8",
                "stw r6, -0xbe8(r5)",
                "ori r0, r4, " + String.Format("0x{0:X4}", MLVL_L)
            ));

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("__ct__11CWorldStateFUi") + 0x10,
                "li r0, " + spawnRoom.MREA_ID
            ));

            // Load default world/area if save corrupted or pak corrupted
            instructions.AddRange(PPCASM.Parse(
                addresses.Get("__sinit_CMFGame_cpp") + 0x37c,
                "lis r4, " + String.Format("0x{0:X4}", MLVL_H),
                "li r0, 1",
                "addi r3, r5, 0x5d0",
                "ori r4, r4, " + String.Format("0x{0:X4}", MLVL_L)
            ));

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyEnableHypermodePatch()
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            List<PPC_Instruction> instructions = new List<PPC_Instruction>();

            // Unlock hypermode for MP1
            instructions.AddRange(PPCASM.Parse(
                addresses.FrontEnd.Get("LoadSettingsForProfile") + 0x170,
                "li r0, 1"
            ));

            // Unlock hypermode for MP2
            /*instructions.AddRange(PPCASM.Parse(
                addresses.FrontEnd.Get("LoadSettingsForProfile") + 0x264,
                "li r0, 1"
            ));

            // Unlock hypermode for MP3
            instructions.AddRange(PPCASM.Parse(
                addresses.FrontEnd.Get("LoadSettingsForProfile") + 0x35C,
                "li r0, 1"
            ));*/

            foreach (PPC_Instruction instruction in instructions)
            {
                new DolPatch(FE_Dol_Path, instruction).Apply();
                new DolPatch(Main_Dol_Path, instruction).Apply();
            }
        }

        void ApplyPowerBombHudFormattingPatch()
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = PPCASM.Parse(
                addresses.Get("SetBombParams__17CHudBallInterfaceFiiibbb") + 0x2c,
                "b " + addresses.GetAsStr("SetBombParams__17CHudBallInterfaceFiiibbb", 0x38),
                ".ascii \"%d:%d\"",
                "nop",
                "mr r6, r5",
                "mr r5, r4",
                "lis r4, " + addresses.GetHiBitsAsStr("SetBombParams__17CHudBallInterfaceFiiibbb", 0x30),
                "ori r4, r4, " + addresses.GetLoBitsAsStr("SetBombParams__17CHudBallInterfaceFiiibbb", 0x30),
                "addi r3, r1, 12",
                "nop",
                "bl " + addresses.GetAsStr("sprintf"),
                "nop"
            );

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyMapDefaultStatePatch(String map_default_state)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            List<PPC_Instruction> instructions = new List<PPC_Instruction>();

            if (map_default_state != "Default") {
                instructions.AddRange(PPCASM.Parse(
                    addresses.Get("IsMapped__13CMapWorldInfoCF7TAreaId"),
                    "li r3, 1",
                    "blr"
                ));
                if (map_default_state == "Visited") {
                    instructions.AddRange(PPCASM.Parse(
                        addresses.Get("IsAreaVisited__13CMapWorldInfoCF7TAreaId"),
                        "li r3, 1",
                        "blr"
                    ));
                }
            }

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplySkipCutscenePatch()
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = PPCASM.Parse(
                addresses.Get("ShouldSkipCinematic__22CScriptSpecialFunctionFR13CStateManager"),
                "li r3, 1",
                "blr"
            );

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyHeatProtectionPatch(String type)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = new PPC_Instruction[0];

            if (type == "Varia Only")
            {
                instructions = PPCASM.Parse(
                    addresses.Get("ThinkAreaDamage__22CScriptSpecialFunctionFfR13CStateManager") + 0x48,
                    "subf r6, r6, r7",
                    "cntlzw r6, r6",
                    "lwz r7, 0x8b4(r4)",
                    "lwz r7, 0xe0(r7)",
                    "nop"
                );
            }

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplySuitDamageReductionPatch(String type)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = new PPC_Instruction[0];
            if (type == "Progressive")
            {
                instructions = PPCASM.Parse(
                    addresses.Get("ApplyLocalDamage__13CStateManagerFRC9CVector3fRC9CVector3fR6CActorfRC11CWeaponMode") + 0x114,
                    "lwz r4,0xe0(r3)",
                    "lwz r5,0xd8(r3)",
                    "addc r4, r4, r5",
                    "lwz r5,0xe8(r3)",
                    "addc r4, r4, r5",
                    "rlwinm r4, r4, 0x2, 0x0, 0x1d",
                    "lis r6, " + addresses.GetHiBitsAsStr("ApplyLocalDamage__13CStateManagerFRC9CVector3fRC9CVector3fR6CActorfRC11CWeaponMode", 0x13c),
                    "ori r6, r6, " + addresses.GetLoBitsAsStr("ApplyLocalDamage__13CStateManagerFRC9CVector3fRC9CVector3fR6CActorfRC11CWeaponMode", 0x13c),
                    "lfsx f0, r4, r6",
                    "b " + addresses.GetAsStr("ApplyLocalDamage__13CStateManagerFRC9CVector3fRC9CVector3fR6CActorfRC11CWeaponMode", 0x178),
                    ".float 0.0",
                    ".float 0.1",
                    ".float 0.2",
                    ".float 0.5",
                    // useless instructions we skip them due to jump to further
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "fnmsubs f29, f0, f29, f29"
                );
            }

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyScanDashPatch()
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            List<PPC_Instruction> instructions = new List<PPC_Instruction>();

            // Allow scan dash
            instructions.AddRange(PPCASM.Parse(
                addresses.Get("SidewaysDashAllowed__7CPlayerCFffRC11CFinalInputR13CStateManager") + 0x38,
                "b " + addresses.GetAsStr("SidewaysDashAllowed__7CPlayerCFffRC11CFinalInputR13CStateManager", 0x50)
            ));

            // ??? might help for dashing
            instructions.AddRange(PPCASM.Parse(
                addresses.Get("StrafeInput__7CPlayerCFRC11CFinalInput") + 0x70,
                "b " + addresses.GetAsStr("StrafeInput__7CPlayerCFRC11CFinalInput", 0x8c)
            ));

            // Stop dash when done with dash
            instructions.AddRange(PPCASM.Parse(
                addresses.Get("GetDampedClampedVelocityWR__7CPlayerCFv") + 0x70,
                "lbz r0, 0x37c(r31)"
            ));

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyUnderwaterSlopeJumpFixPatch()
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = PPCASM.Parse(
                addresses.Get("JumpInput__7CPlayerFRC11CFinalInputR13CStateManager") + 0x384,
                "lis r10, 0xc0f0",
                "addi r29, r29, 0x24a1",
                "addi r29, r29, 0x7fff",
                "stw r10, 0x0(r29)",
                "subi r29, r29, 0x8000",
                "subi r29, r29, 0x24a0",
                "b " + addresses.GetAsStr("JumpInput__7CPlayerFRC11CFinalInputR13CStateManager", 0x3b0)
            );

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyRestoreInfiniteSpeedPatch()
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = PPCASM.Parse(
                addresses.Get("AcceptScriptMsg__7CPlayerF20EScriptObjectMessage9TUniqueIdR13CStateManager") + 0x3ec,
                "li r4, 0"
            );

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }
		
		void ApplyRestoreOOBBombJumpPatch()
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = PPCASM.Parse(
                addresses.Get("BombJump__7CPlayerFRC9CVector3fR13CStateManager") + 0x1b4,
                "b "+ addresses.GetAsStr("BombJump__7CPlayerFRC9CVector3fR13CStateManager", 0x1c4)
            );

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyDisableSpringBallPatch(bool disable)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            if (!disable)
                return;

            PPC_Instruction[] instructions = PPCASM.Parse(
                addresses.Get("ComputeSpringBallMovement__10CMorphBallFRC11CFinalInputR13CStateManagerf") + 0x80,
                "nop"
            );

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyEtankCapacityPatch(float etank_capacity)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            PPC_Instruction[] instructions = PPCASM.Parse(
                addresses.Get("g_EtankCapacity"),
                ".float " + String.Format("{0:0.0}", etank_capacity - 1).Replace(',', '.'),
                ".float " + String.Format("{0:0.0}", etank_capacity - 1).Replace(',', '.'),
                ".float " + String.Format("{0:0.0}", etank_capacity).Replace(',', '.')
            );

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplyMaxAmmoPatch(int max_missiles, int max_power_bombs)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            List<PPC_Instruction> instructions = new List<PPC_Instruction>();

            if (max_missiles > 999)
                throw new Exception("Max obtainable missiles cannot be set to more than 999 missiles!");

            if (max_power_bombs > 9)
                throw new Exception("Max obtainable power bombs cannot be set to more than 9 power bombs!");

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("CPlayerState_PowerUpMaxValues") + 0x10,
                ".long " + max_missiles
            ));

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("CPlayerState_PowerUpMaxValues") + 0x1c,
                ".long " + max_power_bombs
            ));

            foreach (PPC_Instruction instruction in instructions)
                new DolPatch(MP1_Dol_Path, instruction).Apply();
        }

        void ApplySettingsPatch(Config.PrimeSettings settings)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            List<PPC_Instruction> instructions = new List<PPC_Instruction>();

            /* Prime Settings */
            /*
             * Load Settings => 800c9444
             */

            /* Controller Enum -> Reset Func 8024a734
             * + 0xc li r0, controller_enum
             * + 0x14 li r0, sensitivity_enum
             * Lock On/Free Aim = 0x2
             * Swap Beam and Visor = 0x4
             * Swap Jump and Fire = 0x8
             * Rumble = 0x40
             * 
             * Sensitivity = 0 | 1 | 2
            */

            /* Display Reset Func 8024a754
             * + 0xc li r6, brightness
             * + 0x10 li r0, display_enum
             * + 0x18 li r0, display_enum_2
             * Display Enum
             * Hint System = 0x20
             * Display Enum 2
             * Bonus Credit Messages = 0x80
             * 
             * Brightness = 0-100
            */

            /* Sound Reset Func 8024a784
             * + 0x0 li r0, sound
             * + 0x4 stw r0, 0x0c(r3)
             * + 0x8 li r0, music
             * + 0xc stw r0, 0x10(r3)
             * 
             * Sound = 0-100
             * Music = 0-100
            */

            /* Visor Reset Func 8024a798
             * + 0x0 li r4, visor_opacity
             * + 0x4 stw r4, 0x18(r3)
             * + 0x8 li r4, helmet_opacity
             * + 0xc stw r4, 0x1c(r3)
             * + 0x10 li r0, visor_enum
             * + 0x14 stb r0, 0x20(r3)
             * + 0x18 blr
             * + 0x1c nop
             * Visor Enum
             * HUD Lag = 0x80
             * 
             * Visor Opacity = 0-100
             * Helmet Opacity = 0-100
            */

            /* Load Settings from FrontEnd */

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("LoadSettingsFromFrontEnd") + 0x20,
                "li r5, " + String.Format("0x{0:X2}", (int)settings.controller.Sensitivity),
                "stw r5, 0x24(r4)",
                "lis r5, " + String.Format("0x{0:X4}", settings.controller.GetControllerEnum() << 8 | settings.display.GetDisplayEnum2() | settings.visor.GetVisorEnum()),
                "stw r5, 0x20(r4)",
                "li r5, " + String.Format("0x{0:X2}", settings.display.Brightness),
                "stw r5, 0x0(r4)",
                "li r5, " + String.Format("0x{0:X2}", (int)(255.0f * ((float)settings.visor.VisorOpacity / 100.0f))),
                "stw r5, 0x18(r4)",
                "li r5, " + String.Format("0x{0:X2}", (int)(255.0f * ((float)settings.visor.HelmetOpacity / 100.0f))),
                "stw r5, 0x1c(r4)",
                "li r5, " + String.Format("0x{0:X2}", settings.sound.SoundFXVolume),
                "stw r5, 0xc(r4)",
                "li r5, " + String.Format("0x{0:X2}", settings.sound.MusicVolume),
                "stw r5, 0x10(r4)",
                "b "+ addresses.GetAsStr("LoadSettingsFromFrontEnd", 0x140)
            ));

            /* Controller Menu */

            UInt32 controller_func_offset = (UInt32)(addresses.GetType() == typeof(AddressDB.FrontEnd.A_FE_NTSC_J_MP1) ? 0x8 : 0xc);

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ResetControllerSettings") + controller_func_offset,
                "li r0, " + String.Format("0x{0:X2}", settings.controller.GetControllerEnum())
            ));

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ResetControllerSettings") + controller_func_offset + 0x8,
                "li r0, " + String.Format("0x{0:X2}", (int)settings.controller.Sensitivity)
            ));

            /* Display Menu */

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ResetDisplaySettings") + 0xc,
                "li r6, " + String.Format("0x{0:X2}", settings.display.Brightness)
            ));

            // disable hint system everytime
            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ResetDisplaySettings") + 0x10,
                "li r4, 0"
            ));

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ResetDisplaySettings") + 0x18,
                "li r0, " + String.Format("0x{0:X2}", settings.display.GetDisplayEnum2())
            ));

            /* Sound Menu */

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ResetSoundSettings"),
                "li r0, " + String.Format("0x{0:X2}", settings.sound.SoundFXVolume),
                "stw r0, 0xc(r3)",
                "li r0, " + String.Format("0x{0:X2}", settings.sound.MusicVolume),
                "stw r0, 0x10(r3)"
            ));

            /* Visor Menu */

            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ResetVisorSettings"),
                "li r4, " + String.Format("0x{0:X2}", (int)(255.0f * ((float)settings.visor.VisorOpacity / 100.0f))),
                "stw r4, 0x18(r3)",
                "li r4, " + String.Format("0x{0:X2}", (int)(255.0f * ((float)settings.visor.HelmetOpacity / 100.0f))),
                "stw r4, 0x1c(r3)",
                "li r0, " + String.Format("0x{0:X2}", settings.visor.GetVisorEnum()),
                "stb r0, 0x20(r3)",
                "blr",
                "nop"
            ));

            foreach (PPC_Instruction instruction in instructions)
            {
                new DolPatch(MP1_Dol_Path, instruction).Apply();
            }
        }

        void SetSaveFilename(String filename)
        {
            if (addresses == null)
                throw new Exception("Initalize patcher first!");

            if (filename.Length > 8 || !filename.EndsWith(".bin"))
                return;

            new DolPatch(MP1_Dol_Path, PPCASM.Parse(addresses.Get("g_Save_FileName"), ".ascii \"" + filename + "\"")[0]).Apply();
            new DolPatch(FE_Dol_Path, PPCASM.Parse(addresses.FrontEnd.Get("g_Save_FileName"), ".ascii \"" + filename + "\"")[0]).Apply();
            new DolPatch(Main_Dol_Path, PPCASM.Parse(addresses.FrontEnd.Get("g_Save_FileName"), ".ascii \"" + filename + "\"")[0]).Apply();
        }

        internal new void Apply(Dictionary<String, Object> config)
        {
            //new DolPatch(MP1_Dol_Path, DolPatch.SectionType.TEXT, 0x80001800, 0x800).Apply();

            SetSaveFilename((String)config["Save File Name"]);
            SetStartingArea((Enums.SpawnRoom)config["Starting Area"]);
            //ApplyInputPatch();
            ApplyEnableHypermodePatch();
            ApplyPowerBombHudFormattingPatch();
            ApplyMapDefaultStatePatch((String)config["Map Default State"]);
            ApplySkipCutscenePatch();
            ApplyScanDashPatch();
            ApplyUnderwaterSlopeJumpFixPatch();
            ApplyRestoreInfiniteSpeedPatch();
            ApplyRestoreOOBBombJumpPatch();
            ApplyHeatProtectionPatch((String)config["Heat Protection"]);
            ApplySuitDamageReductionPatch((String)config["Suit Damage Reduction"]);
            ApplyDisableSpringBallPatch((bool)config["Disable Spring Ball"]);
            ApplyEtankCapacityPatch((float)config["Etank Capacity"]);
            ApplyMaxAmmoPatch((int)config["Max Obtainable Missiles"], (int)config["Max Obtainable Power Bombs"]);
            ApplySettingsPatch((Config.PrimeSettings)config["Settings"]);
        }

        void ApplyInputPatch()
        {
            // NTSC_U
            // DPAD Up => 0x1074 | Hypermode
            // DPAD Down => 0x1077 | Shoot missiles
            // DPAD Right => 0x107A | R Button
            // DPAD Left => 0x107D | L Button
            // unused register r16, r18, r19
            // r1 => sp
            // Hypermode => (CPlayer + 0x480) + 0x84B as u8
            // Hypermode | 0x20  set in phazon pool

            List<PPC_Instruction> instructions = new List<PPC_Instruction>();

            // Hook(src, dst) where src is CPlayer::ProcessInput + 0x64c which points to CPlayer::UpdateOrbitInput and dst is our custom function
            instructions.AddRange(PPCASM.Parse(
                addresses.Get("ProcessInput__7CPlayerFRC11CFinalInputR13CStateManager") + 0x640,
                "lis r16, 0x8000",
                "ori r16, r16, 0x1810",
                "mtlr r16",
                "blr"
            ));

            // custom function
            instructions.AddRange(PPCASM.Parse(
                0x80001800,
                ".long 0", // g_HypermodePressedFrames
                ".long 0", // g_IsDoneActivatingHypermode
                ".long 0", // g_IsHypermodeEnabled
                ".float 100.0", // g_EnergyNeededForHypermodeActivation
                "mr r3, r29",
                "mr r4, r30",
                "mr r5, r31",
                "bl " + addresses.GetAsStr("UpdateOrbitInput__7CPlayerFRC11CFinalInputR13CStateManager"),
                // if DPad Up Pressed increase pressed frames for hypermode else decrease
                "lbz r16, 0x1074(r3)",
                "cmpwi r16, 1",
                "lis r16, 0x8000",
                "ori r16, r16, 0x1800",
                "bne 0x80001868",
                "lwz r18, 0x8b8(r4)",
                "lwz r18, 0xe8(r18)",
                "cmpwi r18, 1",
                "bne 0x80001868",
                "lwz r18, 0x4(r16)",
                "cmpwi r18, 1",
                "beq 0x80001868",
                "lwz r18, 0x0(r16)",
                "cmpwi r18, 20",
                "blt 0x80001868",
                "addi r18, r18, 1",
                "stw r18, 0x0(r16)",
                "b 0x8000187c",
                "lwz r18, 0x0(r16)", // 0x80001868
                "cmpwi r18, 0x0",
                "beq 0x8000187c",
                "subi r18, r18, 1",
                "stw r18, 0x0(r16)",
                // Handle phazon global variables


                /*"bne 0x80001814",
                "li r18, 6",
                "stw r18, 0x300(r29)",
                "lis r18, 0xFFFF",
                "addi r18, r18, 0x7FFF",
                "addi r18, r18, 0x7FFF",
                "addi r18, r18, 1",
                "lbz r16, 0x107D(r29)",
                "cmpwi r16, 1",
                "bne 0x80001868",
                "stw r18, 0x310(r29)",
                "stw r18, 0x314(r29)",
                "stw r18, 0x318(r29)",
                "li r18, 2",
                "stw r18, 0x300(r29)",
                "b 0x80001868",
                "lwz r19, 0x310(r29)",
                "cmpw r18, r19",
                "bne 0x80001868",
                "li r19, 0",
                "stw r19, 0x310(r29)",
                "stw r19, 0x314(r29)",
                "stw r19, 0x318(r29)",
                "stw r19, 0x300(r29)",
                "and r16, r17, r17",
                "and r18, r17, r17",
                "and r19, r17, r17",
                "mr r3, r29",*/
                "b " + addresses.GetAsStr("ProcessInput__7CPlayerFRC11CFinalInputR13CStateManager", 0x650)
            ));

            /*
            // Check if Z Button or DPad Up were pressed
            //new DolPatch<UInt32>(MP1_Dol_Path, 0x80194680, (UInt32)(enabled ? 0x801D1060 : 0x801D0300)).Apply();
            
            if(enabled)
                new DolPatch(MP1_Dol_Path, 0x8019568C, 0x801956C8, -4).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80195698, (UInt32)(enabled ? 0x48000030 : 0x4080002C)).Apply(); // jmp to 801956C8
            new DolPatch<UInt32>(MP1_Dol_Path, 0x8019569C, (UInt32)(enabled ? 0x48006038 : 0xEF60E024)).Apply(); // bl CPlayer::UpdateOrbitInput
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956A0, (UInt32)(enabled ? 0x89DD1074 : 0x7FA3EB78)).Apply(); // lbz r14, CPlayer[0x1074] aka DPad Up
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956A4, (UInt32)(enabled ? 0x89FD109B : 0x4BFFD8F5)).Apply(); // lbz r15, CPlayer[0x109B] aka Z Button
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956A8, (UInt32)(enabled ? 0x7DEE7378 : 0xC042A524)).Apply(); // set r14 to CPlayer[0x1074] or CPlayer[0x109B]
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956AC, (UInt32)(enabled ? 0x39E00001 : 0xEC1B0672)).Apply(); // li r15, 1
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956B0, (UInt32)(enabled ? 0x7DCE7830 : 0xEC42D828)).Apply(); // multiply r14 by 2
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956B4, (UInt32)(enabled ? 0x91DD0300 : 0xEC220072)).Apply(); // set CPlayer[0x300] to r14
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956B8, (UInt32)(enabled ? 0x60000000 : 0xEC2106B2)).Apply(); // 
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956BC, (UInt32)(enabled ? 0x60000000 : 0xEC20082A)).Apply(); //
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956C0, (UInt32)(enabled ? 0x60000000 : 0x48000028)).Apply(); //
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801956C4, (UInt32)(enabled ? 0x4BFED7F8 : 0x60000000)).Apply(); // jump back to CPlayer::ProcessInput
            if(!enabled)
            {
                new DolPatch(MP1_Dol_Path, 0x80195688, 0x801956C4, 4).Apply();
                new DolPatch<UInt32>(MP1_Dol_Path, 0x80195688, 0xFC00E040).Apply();
            }*/

            foreach (PPC_Instruction instruction in instructions)
            {
                new DolPatch(MP1_Dol_Path, instruction).Apply();
            }
        }
    }
}
