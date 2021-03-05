using System;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    // Add suit damage reduction patch
    class Patcher
    {
        static Patches._Trilogy patches = null;

        static UInt32 MakeJMP(UInt32 src, UInt32 dst)
        {
            return (UInt32)(0x48000000 + (dst - src));
        }

        /*static void WriteFunction(UInt32 address, UInt32[] opcodes)
        {
            UInt32 max_opcodes = (0x800 - (address - 0x80001800)) / 4;
            if (opcodes.Length > max_opcodes)
                throw new Exception("Cannot create function there's not enough memory allocated in that section");

            for (int i = 0; i < opcodes.Length; i++)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, (UInt32)(address + i * 4), opcodes[i]).Apply();
        }*/

        public static void Init(char Game_Region)
        {
            if (Game_Region == 'P')
                patches = new Patches.PAL();
            if (Game_Region == 'E')
                patches = new Patches.NTSC_U();
            if (Game_Region == 'J')
                patches = new Patches.NTSC_J();
        }

        public static void SetStartingArea(Enums.SpawnRoom spawnRoom)
        {
            UInt16 MLVL_H = (UInt16)((spawnRoom.MLVL & 0xFFFF0000) >> 16);
            UInt16 MLVL_L = (UInt16)(spawnRoom.MLVL & 0xFFFF);
            if (MLVL_L > 0x7FFF)
                MLVL_H++;

            if (patches == null)
                return;

            patches.SetStartingArea(MLVL_H, MLVL_L, (UInt16)spawnRoom.MREA_ID);
        }

        public static void ApplySkipCutscenePatch()
        {
            if (patches == null)
                return;

            patches.ApplySkipCutscenePatch();
        }

        public static void ApplyHeatProtectionPatch(String type)
        {
            if (patches == null)
                return;

            patches.ApplyHeatProtectionPatch(type);
        }

        public static void ApplySuitDamageReductionPatch(String type)
        {
            if (patches == null)
                return;

            patches.ApplySuitDamageReductionPatch(type);
        }

        public static void ApplyScanDashPatch()
        {
            if (patches == null)
                return;

            patches.ApplyScanDashPatch();
        }

        public static void ApplyUnderwaterSlopeJumpFixPatch(bool enabled)
        {
            if (patches == null)
                return;

            patches.ApplyUnderwaterSlopeJumpFixPatch(enabled);
        }
        
        public static void ApplyDisableSpringBallPatch()
        {
            if (patches == null)
                return;

            patches.ApplyDisableSpringBallPatch();
        }

        public static void SetSaveFilename(String filename)
        {
            if (patches == null)
                return;

            if (!filename.EndsWith(".bin"))
                return;
            if (filename.Length > 8)
                return;
            while (filename.Length < 8)
                filename += "\0";

            patches.SetSaveFilename(filename);
        }
    }
}
