using System;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    class Patches
    {
        static String MP1_Dol_Path = ".\\tmp\\wii\\DATA\\sys\\main.dol";

        public static void SetStartingArea(Enums.SpawnRoom spawnRoom)
        {
            UInt16 MLVL_H = (UInt16)((spawnRoom.MLVL & 0xFFFF0000) >> 16);
            UInt16 MLVL_L = (UInt16)(spawnRoom.MLVL & 0xFFFF);
            if (MLVL_L > 0x7FFF)
                MLVL_H++;

            new Patcher.DOL_Code_Patch<UInt16>(MP1_Dol_Path, 0x801098EC + 2, MLVL_H).Apply();
            new Patcher.DOL_Code_Patch<UInt16>(MP1_Dol_Path, 0x801098EC + 14, MLVL_L).Apply();
            new Patcher.DOL_Code_Patch<UInt16>(MP1_Dol_Path, 0x800CD1F8 + 18, (UInt16)spawnRoom.MREA_ID).Apply();

            // Set default MLVL in case of crash or save corruption
            new Patcher.DOL_Code_Patch<UInt16>(MP1_Dol_Path, 0x8010D278 + 2, MLVL_H).Apply();
            new Patcher.DOL_Code_Patch<UInt16>(MP1_Dol_Path, 0x8010D278 + 14, MLVL_L).Apply();

            // Doesn't reset intro messages???
            new Patcher.DOL_Code_Patch<UInt16>(MP1_Dol_Path, 0x8010D27C + 2, (UInt16)0).Apply();
        }

        public static void ApplySkipCutscenePatch(bool enabled)
        {
            if (enabled)
            {
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FC70C, (UInt32)0x38600001).Apply(); // li r3, 1
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FC70C + 4, (UInt32)0x4E800020).Apply(); // blr
            }
            else
            {
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FC70C, (UInt32)0x9421FFE0).Apply(); // stwu r1, local_20(r1)
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FC70C + 4, (UInt32)0x7C0802A6).Apply(); // mfspr r0, LR
            }
        }

        public static void ApplyHeatProtectionPatch(String type)
        {
            if (type == "Any Suit")
            {
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48, (UInt32)0x7C0800D0).Apply(); // neg r0, r8
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 4, (UInt32)0x7C004078).Apply(); // andc r0, r0, r8
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 8, (UInt32)0x7CC63850).Apply(); // subf r6, r6, r7
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 12, (UInt32)0x7CC60034).Apply(); // cntlzw r6, r6
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 16, (UInt32)0x54070FFE).Apply(); // rlwinm r7, r0, 0x1, 0x1f, 0x1f
            }
            if (type == "Varia Only")
            {
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48, (UInt32)0x7CC63850).Apply(); // subf r6, r6, r7
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 4, (UInt32)0x80F000E0).Apply(); // cntlzw r6, r6
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 8, (UInt32)0x80E600E0).Apply(); // lwz r7, 0xe0(r6)
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 12, (UInt32)0x60000000).Apply(); // nop
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801FEBCC + 0x48 + 16, (UInt32)0x60000000).Apply(); // nop
            }
        }

        public static void ApplyScanDashPatch(bool enabled)
        {
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801932FC + 0x38, (UInt32)(enabled ? 0x42800018 : 0x41820010)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80194AF0 + 0x70, (UInt32)(enabled ? 0x4280001C : 0x4082001C)).Apply();

            if (enabled)
            {
                // Stop dash when done with dash
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80192CC0, (UInt32)0x801F037C).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80192CE4, (UInt32)0x60000000).Apply();
                // Restoring strafeVel self multiply from GC in CPlayer::ComputeDash
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A70, (UInt32)0xEF9C0032).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193B14, (UInt32)0xEF9C0032).Apply();
            }
            else
            {
                // Stop dash when no more locking on lock point
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80192CC0, (UInt32)0x801F0300).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80192CE4, (UInt32)0x40820128).Apply();
                // Restoring strafeVel limitation from Wii in CPlayer::ComputeDash
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A70, (UInt32)0xEF9F0032).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193B14, (UInt32)0xEF9F0032).Apply();
            }
            // 804D3FA8 804D3FB0
        }

        public static void ApplyUnderwaterSlopeJumpFixPatch(bool enabled)
        {            
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x8019569C, (UInt32)(enabled ? 0x3D40C0F0 : 0x4080002C)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801956A0, (UInt32)(enabled ? 0x3BBD24A1 : 0xEF60E024)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801956A4, (UInt32)(enabled ? 0x3BBD7FFF : 0x7FA3EB78)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801956A8, (UInt32)(enabled ? 0x915D0000 : 0x4BFFD8F5)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801956AC, (UInt32)(enabled ? 0x3BBD8000 : 0xC042A524)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801956B0, (UInt32)(enabled ? 0x3BBDDB60 : 0xEC1B0672)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801956B4, (UInt32)(enabled ? 0x48000014 : 0xEC42D828)).Apply();
        }
    }
}
