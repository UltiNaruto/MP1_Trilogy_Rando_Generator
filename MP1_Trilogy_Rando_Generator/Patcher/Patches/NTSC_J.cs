using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.Patches
{
    class NTSC_J : _Trilogy
    {
        internal static new String MP1_Dol_Path = ".\\tmp\\wii\\DATA\\files\\rs5mp1jpn_p.dol";

        public override void SetStartingArea(UInt16 MLVL_H, UInt16 MLVL_L, UInt16 MREA_ID)
        {
            new DolPatch<UInt16>(MP1_Dol_Path, 0x80109EE0 + 2, MLVL_H).Apply();
            new DolPatch<UInt16>(MP1_Dol_Path, 0x80109EE0 + 14, MLVL_L).Apply();
            new DolPatch<UInt16>(MP1_Dol_Path, 0x800CD374 + 18, MREA_ID).Apply();

            // Set default MLVL in case of crash or save corruption
            new DolPatch<UInt16>(MP1_Dol_Path, 0x8010D86C + 2, MLVL_H).Apply();
            new DolPatch<UInt16>(MP1_Dol_Path, 0x8010D86C + 14, MLVL_L).Apply();
        }
        
        public override void ApplySkipCutscenePatch()
        {
            UInt32 addr = 0x801FD344;
            new DolPatch<UInt32>(MP1_Dol_Path, addr, (UInt32)0x38600001).Apply(); // li r3, 1
            new DolPatch<UInt32>(MP1_Dol_Path, addr + 4, (UInt32)0x4E800020).Apply(); // blr
        }

        public override void ApplyHeatProtectionPatch(String type)
        {
            UInt32 addr = 0x801FF788;
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
            UInt32 addr = 0x8022A8D8;
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
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80193E7C + 0x38, (UInt32)0x48000018).Apply();
            // ??? might help for dashing
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80195670 + 0x70, (UInt32)0x4800001C).Apply();
            // Stop dash when done with dash
            new DolPatch<UInt32>(MP1_Dol_Path, 0x801937D0 + 0x70, (UInt32)0x881F037C).Apply();
        }

        public override void ApplyUnderwaterSlopeJumpFixPatch(bool enabled)
        {
            UInt32 addr = 0x80195E98;
            UInt32 off = 0x384;
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off, (UInt32)(enabled ? 0x3D40C0F0 : 0x4080002C)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 4, (UInt32)(enabled ? 0x3BBD24A1 : 0xEF60E024)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 8, (UInt32)(enabled ? 0x3BBD7FFF : 0x7FA3EB78)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 12, (UInt32)(enabled ? 0x915D0000 : 0x4BFFD8F5)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 16, (UInt32)(enabled ? 0x3BBD8000 : 0xC042A5C4)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 20, (UInt32)(enabled ? 0x3BBDDB60 : 0xEC1B0672)).Apply();
            new DolPatch<UInt32>(MP1_Dol_Path, addr + off + 24, (UInt32)(enabled ? 0x48000014 : 0xEC42D828)).Apply();
        }
        
        public override void ApplyDisableSpringBallPatch()
        {
            new DolPatch<UInt32>(MP1_Dol_Path, 0x80147C08 + 0x80, (UInt32)0x60000000).Apply();
        }

        public override void SetSaveFilename(String filename)
        {
            new DolPatch<String>(FE_Dol_Path, 0x8052F788, filename).Apply();
            new DolPatch<String>(Main_Dol_Path, 0x8052F788, filename).Apply();
            new DolPatch<String>(MP1_Dol_Path, 0x80475030, filename).Apply();
        }
    }
}
