using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.Patches
{
    abstract class _Trilogy
    {
        internal static String MP1_Dol_Path = ".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol";
        internal static String FE_Dol_Path = ".\\tmp\\wii\\DATA\\files\\rs5fe_p.dol";
        internal static String Main_Dol_Path = ".\\tmp\\wii\\DATA\\sys\\main.dol";

        public abstract void SetStartingArea(UInt16 MLVL_H, UInt16 MLVL_L, UInt16 MREA_ID);
        public abstract void ApplySkipCutscenePatch();
        public abstract void ApplyHeatProtectionPatch(String type);
        public abstract void ApplySuitDamageReductionPatch(String type);
        public abstract void ApplyScanDashPatch();
        public abstract void ApplyUnderwaterSlopeJumpFixPatch(bool enabled);
        public abstract void ApplyDisableSpringBallPatch();
        public abstract void SetSaveFilename(String filename);
    }
}
