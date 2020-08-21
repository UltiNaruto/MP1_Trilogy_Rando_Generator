using System;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    // Add suit damage reduction patch
    class Patches
    {
        static String MP1_Dol_Path = ".\\tmp\\wii\\DATA\\sys\\main.dol";

        public static void SetStartingArea(Enums.SpawnRoom spawnRoom)
        {
            UInt16 MLVL_H = (UInt16)((spawnRoom.MLVL & 0xFFFF0000) >> 16);
            UInt16 MLVL_L = (UInt16)(spawnRoom.MLVL & 0xFFFF);
            if (MLVL_L > 0x7FFF)
                MLVL_H++;

            new Patcher.DOL_Patch<UInt16>(MP1_Dol_Path, 0x801098EC + 2, MLVL_H).Apply();
            new Patcher.DOL_Patch<UInt16>(MP1_Dol_Path, 0x801098EC + 14, MLVL_L).Apply();
            new Patcher.DOL_Patch<UInt16>(MP1_Dol_Path, 0x800CD1F8 + 18, (UInt16)spawnRoom.MREA_ID).Apply();

            // Set default MLVL in case of crash or save corruption
            new Patcher.DOL_Patch<UInt16>(MP1_Dol_Path, 0x8010D278 + 2, MLVL_H).Apply();
            new Patcher.DOL_Patch<UInt16>(MP1_Dol_Path, 0x8010D278 + 14, MLVL_L).Apply();

            // Doesn't reset intro messages???
            //new Patcher.DOL_Code_Patch<UInt16>(MP1_Dol_Path, 0x8010D27C + 2, (UInt16)0).Apply();
        }

        public static void ApplySkipCutscenePatch(bool enabled)
        {
            UInt32 addr = 0x801FC70C;
            if (enabled)
            {
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr, (UInt32)0x38600001).Apply(); // li r3, 1
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + 4, (UInt32)0x4E800020).Apply(); // blr
            }
            else
            {
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr, (UInt32)0x9421FFE0).Apply(); // stwu r1, local_20(r1)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + 4, (UInt32)0x7C0802A6).Apply(); // mfspr r0, LR
            }
        }

        public static void ApplyHeatProtectionPatch(String type)
        {
            UInt32 addr = 0x801FEBCC;
            UInt32 off = 0x48;
            if (type == "Any Suit")
            {
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)0x7C0800D0).Apply(); // neg r0, r8
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)0x7C004078).Apply(); // andc r0, r0, r8
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)0x7CC63850).Apply(); // subf r6, r6, r7
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)0x7CC60034).Apply(); // cntlzw r6, r6
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)0x54070FFE).Apply(); // rlwinm r7, r0, 0x1, 0x1f, 0x1f
            }
            if (type == "Varia Only")
            {
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)0x7CC63850).Apply(); // subf r6, r6, r7
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)0x7CC60034).Apply(); // cntlzw r6, r6
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)0x80E408b4).Apply(); // lwz r7, 0x4b4(r4)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)0x80E700E0).Apply(); // lwz r7, 0xe0(r7)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)0x60000000).Apply(); // nop
            }
        }

        public static void ApplySuitDamageReductionPatch(String type)
        {
            UInt32 addr = 0x80229D1C;
            UInt32 off = 0x114;
            if (type == "Progressive")
            {
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)0x808300E0).Apply(); // lwz r4,0xe0(r3)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)0x80A300D8).Apply(); // lwz r5,0xd8(r3)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)0x7C842814).Apply(); // addc r4, r4, r5
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)0x80A300E8).Apply(); // lwz r5,0xe8(r3)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)0x7C842814).Apply(); // addc r4, r4, r5
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 20, (UInt32)0x5484103A).Apply(); // rlwinm r4, r4, 0x2, 0x0, 0x1d
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 24, (UInt32)0x3CC08005).Apply(); // lis r6, -0x7ffb
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 28, (UInt32)0x38C69F2C).Apply(); // subi r6, r6, 0x60d4
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 32, (UInt32)0x7C04342E).Apply(); // lfsx f0, r4, r6
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 36, (UInt32)0x48000040).Apply(); // b 80229E94
                new Patcher.DOL_Patch<Single>(MP1_Dol_Path, addr + off + 40, 0.0f).Apply();
                new Patcher.DOL_Patch<Single>(MP1_Dol_Path, addr + off + 44, 0.1f).Apply();
                new Patcher.DOL_Patch<Single>(MP1_Dol_Path, addr + off + 48, 0.2f).Apply();
                new Patcher.DOL_Patch<Single>(MP1_Dol_Path, addr + off + 52, 0.5f).Apply();
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 100, (UInt32)0xEFC0F7BC).Apply(); // fnmsubs f30, f0, f30, f30
            }
            if (type == "Default")
            {
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)0x38800016).Apply(); // li r4, 0x16
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)0x4BF76D55).Apply(); // bl FUN_801a0b88
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)0x2C030000).Apply(); // cmpwi r3, 0x0 
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)0x4182000C).Apply(); // beq 80229e48
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)0x806D9BC0).Apply(); // lwz r3, -0x6440(r13)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 20, (UInt32)0xC3C30300).Apply(); // lfs f30, 0x300(r3)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 24, (UInt32)0x807808B4).Apply(); // lwz r3, 0x8b4(r24)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 28, (UInt32)0x38800015).Apply(); // li r4, 0x15
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 32, (UInt32)0x4BF76D39).Apply(); // bl 801a0b88
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 36, (UInt32)0x2C030000).Apply(); // cmpwi r3, 0x0
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 40, (UInt32)0x41820014).Apply(); // beq 80229e6c
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 44, (UInt32)0x806D9BC0).Apply(); // lwz r3, -0x6440(r13)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 48, (UInt32)0xC0230304).Apply(); // lfs f1, 0x304(r3)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 52, (UInt32)0xEC1E0828).Apply(); // fsubs f0, f30, f1
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, addr + off + 100, (UInt32)0xEFBD0028).Apply(); // fsubs f29, f29, f0
            }
        }

        public static void ApplyScanDashPatch(bool enabled)
        {
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x801932FC + 0x38, (UInt32)(enabled ? 0x48000018 : 0x41820010)).Apply();
            //new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x80194AF0 + 0x70, (UInt32)(enabled ? 0x4800001C : 0x4082001C)).Apply();

            if (enabled)
            {
                // Stop dash when done with dash
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x80192CC0, (UInt32)0x801F025C).Apply();
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x80192CE4, (UInt32)0x60000000).Apply();
                // Restoring strafeVel self multiply from GC in CPlayer::ComputeDash
                //new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x80193A70, (UInt32)0xEF9C0032).Apply();
                //new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193B14, (UInt32)0xEF9C0032).Apply();
            }
            else
            {
                // Stop dash when no more locking on lock point
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x80192CC0, (UInt32)0x801F0300).Apply();
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x80192CE4, (UInt32)0x40820128).Apply();
                // Restoring strafeVel limitation from Wii in CPlayer::ComputeDash
                //new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x80193A70, (UInt32)0xEF9F0032).Apply();
                //new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193B14, (UInt32)0xEF9F0032).Apply();
            }
            // 804D3FA8 804D3FB0
        }

        public static void ApplyUnderwaterSlopeJumpFixPatch(bool enabled)
        {            
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x8019569C, (UInt32)(enabled ? 0x3D40C0F0 : 0x4080002C)).Apply();
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x801956A0, (UInt32)(enabled ? 0x3BBD24A1 : 0xEF60E024)).Apply();
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x801956A4, (UInt32)(enabled ? 0x3BBD7FFF : 0x7FA3EB78)).Apply();
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x801956A8, (UInt32)(enabled ? 0x915D0000 : 0x4BFFD8F5)).Apply();
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x801956AC, (UInt32)(enabled ? 0x3BBD8000 : 0xC042A524)).Apply();
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x801956B0, (UInt32)(enabled ? 0x3BBDDB60 : 0xEC1B0672)).Apply();
            new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, 0x801956B4, (UInt32)(enabled ? 0x48000014 : 0xEC42D828)).Apply();
        }
    }
}
