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
            //new Patcher.DOL_Patch<UInt16>(MP1_Dol_Path, 0x8010D27C + 2, (UInt16)spawnRoom.PakIndex).Apply();
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
                // Restoring dash calculation from GC in CPlayer::ComputeDash
                new Patcher.DOL_ScrollCode_Patch(MP1_Dol_Path, 0x80193A3C, 0x80193AA4, -4).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A78, (UInt32)0xC042A524).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A80, (UInt32)0xEC210024).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A84, (UInt32)0xFC200210).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A88, (UInt32)0xFC001040).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A8C, (UInt32)0x40810014).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A90, (UInt32)0xC002A52C).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A94, (UInt32)0xC042A4E4).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A98, (UInt32)0xEC020072).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A9C, (UInt32)0xEC010028).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193AA0, (UInt32)0x2C060002).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A6C, (UInt32)0xEF9C0032).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193B14, (UInt32)0xEF9C0032).Apply();
            }
            else
            {
                // Restoring dash calculation from Wii in CPlayer::ComputeDash
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A78, (UInt32)0x2C060002).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A80, (UInt32)0xC042A524).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A84, (UInt32)0xEC010024).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A88, (UInt32)0xFC601050).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A8C, (UInt32)0xEC230028).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A90, (UInt32)0xFC0100EE).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A94, (UInt32)0xEC201028).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A98, (UInt32)0xFC0100AE).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A9C, (UInt32)0xEC420028).Apply();
                new Patcher.DOL_ScrollCode_Patch(MP1_Dol_Path, 0x80193A3C, 0x80193AA0, 4).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A38, (UInt32)0x2C060002).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193A6C, (UInt32)0xEF9F0032).Apply();
                new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80193B14, (UInt32)0xEF9F0032).Apply();
            }
            // 804D3FA8 804D3FB0
        }

        public static void ApplyUnderwaterSlopeJumpFixPatch(bool enabled)
        {
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80192FCC, (UInt32)(enabled ? 0x3AE30000 : 0x7C600034)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80192FD4, (UInt32)(enabled ? 0x60000000 : 0x541FD97E)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80192FDC, (UInt32)(enabled ? 0x2C170000 : 0x2C030000)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x8019557C, (UInt32)(enabled ? 0x60000000 : 0xEC190032)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80195580, (UInt32)(enabled ? 0xEC19F03C : 0xEC1E0028)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x80195698, (UInt32)(enabled ? 0x48000054 : 0xEF3F0072)).Apply();
            new Patcher.DOL_Code_Patch<UInt32>(MP1_Dol_Path, 0x801A0B88, (UInt32)(enabled ? 0x2C040028 : 0x28040028)).Apply();
            //new Patcher.DOL_Value_Patch<Single>(MP1_Dol_Path, 0x804DE0C0, enabled ? -4.15f : -10.0f).Apply();
        }
    }
}
