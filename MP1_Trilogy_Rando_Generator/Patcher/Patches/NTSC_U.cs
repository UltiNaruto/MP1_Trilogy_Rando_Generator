using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.Patches
{
    class NTSC_U : _Trilogy
    {
        public override void SetStartingArea(UInt16 MLVL_H, UInt16 MLVL_L, UInt16 MREA_ID)
        {
            new DolPatch<UInt16>(MP1_Dol_Path, 0x801098EC + 2, MLVL_H).Apply();
            new DolPatch<UInt16>(MP1_Dol_Path, 0x801098EC + 14, MLVL_L).Apply();
            new DolPatch<UInt16>(MP1_Dol_Path, 0x800CD1F8 + 18, MREA_ID).Apply();

            // Set default MLVL in case of crash or save corruption
            new DolPatch<UInt16>(MP1_Dol_Path, 0x8010D278 + 2, MLVL_H).Apply();
            new DolPatch<UInt16>(MP1_Dol_Path, 0x8010D278 + 14, MLVL_L).Apply();
        }

        /*public static void ApplyDisableHintSystemPatch() // Could be used for hud memo in multiworld ?
        {
            if(enabled)
                new DolPatch<UInt32>(MP1_Dol_Path, 0x80197268, (UInt32)0x4E800020).Apply(); // blr
              else
                new DolPatch<UInt32>(MP1_Dol_Path, 0x80197268, (UInt32)0x9421FFB0).Apply(); // stwu r1, -0x50(r1)
        }*/

        public override void ApplySkipCutscenePatch()
        {
            UInt32 addr = 0x801FC70C;
            new DolPatch<UInt32>(MP1_Dol_Path, addr, (UInt32)0x38600001).Apply(); // li r3, 1
            new DolPatch<UInt32>(MP1_Dol_Path, addr + 4, (UInt32)0x4E800020).Apply(); // blr
        }

        public override void ApplyHeatProtectionPatch(String type)
        {
            UInt32 addr = 0x801FEBCC;
            UInt32 off = 0x48;
            if (type == "Varia Only")
            {
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)0x7CC63850).Apply(); // subf r6, r6, r7
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)0x7CC60034).Apply(); // cntlzw r6, r6
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)0x80E408b4).Apply(); // lwz r7, 0x4b4(r4)
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)0x80E700E0).Apply(); // lwz r7, 0xe0(r7)
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)0x60000000).Apply(); // nop
            }
        }

        public override void ApplySuitDamageReductionPatch(String type)
        {
            UInt32 addr = 0x80229D1C;
            UInt32 off = 0x114;
            if (type == "Progressive")
            {
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)0x808300E0).Apply(); // lwz r4,0xe0(r3)
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)0x80A300D8).Apply(); // lwz r5,0xd8(r3)
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)0x7C842814).Apply(); // addc r4, r4, r5
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)0x80A300E8).Apply(); // lwz r5,0xe8(r3)
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)0x7C842814).Apply(); // addc r4, r4, r5
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 20, (UInt32)0x5484103A).Apply(); // rlwinm r4, r4, 0x2, 0x0, 0x1d
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 24, (UInt32)0x3CC08005).Apply(); // lis r6, -0x7ffb
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 28, (UInt32)0x38C69F2C).Apply(); // subi r6, r6, 0x60d4
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 32, (UInt32)0x7C04342E).Apply(); // lfsx f0, r4, r6
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 36, (UInt32)0x48000040).Apply(); // b 80229E94
                new DolPatch<Single>(MP1_Dol_Path, addr + off + 40, 0.0f).Apply();
                new DolPatch<Single>(MP1_Dol_Path, addr + off + 44, 0.1f).Apply();
                new DolPatch<Single>(MP1_Dol_Path, addr + off + 48, 0.2f).Apply();
                new DolPatch<Single>(MP1_Dol_Path, addr + off + 52, 0.5f).Apply();
                new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 100, (UInt32)0xEFC0F7BC).Apply(); // fnmsubs f30, f0, f30, f30
            }
        }

        public override void ApplyScanDashPatch()
        {
            // Allow scan dash
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801932FC + 0x38, (UInt32)0x48000018).Apply();
            // ??? might help for dashing
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80194AF0 + 0x70, (UInt32)0x4800001C).Apply();
            // Stop dash when done with dash
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80192C50 + 0x70, (UInt32)0x881F037C).Apply();
        }

        public override void ApplyUnderwaterSlopeJumpFixPatch(bool enabled)
        {
            UInt32 addr = 0x80195318;
            UInt32 off = 0x384;
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)(enabled ? 0x3D40C0F0 : 0x4080002C)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)(enabled ? 0x3BBD24A1 : 0xEF60E024)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)(enabled ? 0x3BBD7FFF : 0x7FA3EB78)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)(enabled ? 0x915D0000 : 0x4BFFD8F5)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)(enabled ? 0x3BBD8000 : 0xC042A524)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 20, (UInt32)(enabled ? 0x3BBDDB60 : 0xEC1B0672)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 24, (UInt32)(enabled ? 0x48000014 : 0xEC42D828)).Apply();
        }

        public override void ApplyDisableSpringBallPatch()
        {
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80147608 + 0x80, (UInt32)0x60000000).Apply();
        }

        public override void SetSaveFilename(String filename)
        {
            new DolPatch<String>(FE_Dol_Path, 0x8052FBA8, filename).Apply();
            new DolPatch<String>(Main_Dol_Path, 0x8052FBA8, filename).Apply();
            new DolPatch<String>(MP1_Dol_Path, 0x80475D48, filename).Apply();
        }

        /*public static void ApplyInputPatch()
        {
            // DPAD Up => 0x1074 | Hypermode
            // DPAD Down => 0x1077 | Shoot missiles
            // DPAD Right => 0x107A | R Button
            // DPAD Left => 0x107D | L Button
            // unused register r16, r18, r19
            // r1 => sp
            // Hook(src, dst) where src is setting "this" to CPlayer::??? and dst is our custom function
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80182EBC, (UInt32)0x4BE7E944).Apply();

            UInt32 custom_func_address = 0x80001800;
            UInt32[] custom_func = new UInt32[] {
                0x8A1D107A, // lbz r16, 0x107A(r29)
                0x2C100001, // cmpwi r16, 1
                0x4082000C, // bne 0x80001814
                0x3A400006, // li r18, 6
                0x925D0300, // stw r18, 0x300(r29)
                0x3E40FFFF, // lis r18, 0xFFFF
                0x3A527FFF, // addi r18, r18, 0x7FFF
                0x3A527FFF, // addi r18, r18, 0x7FFF
                0x3A520001, // addi r18, r18, 0x1
                0x8A1D107D, // lbz r16, 0x107D(r29)
                0x2C100001, // cmpwi r16, 1
                0x4082003C, // bne 0x80001868
                0x925D0310, // stw r18, 0x310(r29)
                0x925D0314, // stw r18, 0x314(r29)
                0x925D0318, // stw r18, 0x318(r29)
                0x3A400002, // li r18, 2
                0x925D0300, // stw r18, 0x300(r29)
                0x48000024, // b 0x80001868
                0x827D0310, // lwz r19, 0x310(r29)
                0x7C120000, // cmpw r18, r19
                0x40820018, // bne 0x80001868
                0x3A600000, // li r19, 0
                0x927D0310, // stw r19, 0x310(r29)
                0x927D0314, // stw r19, 0x314(r29)
                0x927D0318, // stw r19, 0x318(r29)
                0x927D0310, // stw r19, 0x300(r29)
                0x7E308838, // and r16, r17, r17
                0x7E328838, // and r18, r17, r17
                0x7E338838, // and r19, r17, r17
                0x7FA3EB78, // mr r3, r29
                0 // b 80001804 -> 80182EC0
            };

            // set jump back to CPlayer::ProcessInput
            custom_func[custom_func.Length - 1] = MakeJMP((UInt32)(custom_func_address + (custom_func.Length - 1) * 4), 0x80182EC0);

            WriteFunction(custom_func_address, custom_func);

            // Jump to our subroutine then call CPlayer::UpdateOrbitInput do our stuff and return to CPlayer::ProcessInput
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80182EB8, (UInt32)(enabled ? 0x480127E4 : 0x4801881D)).Apply();

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
            }

            // Patch return of CPlayer::UpdateOrbitInput because it doesn't call the function but rather jump to it
            new DolPatch<UInt32>(MP1_Dol_Path, 0x8019BDB4, (UInt32)(enabled ? 0x428098EC : 0x4E800020)).Apply();
        }*/
    }
}
