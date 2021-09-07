using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.AddressDB.Prime
{
    class A_PAL : TrilogyPrime
    {
        internal A_PAL()
        {
            _FrontEnd = new FrontEnd.A_FE_PAL();
        }

        internal override UInt32 Get(String function_symbol)
        {
            switch (function_symbol)
            {
                case "LoadSettingsFromFrontEnd":
                    return 0x800c95d0;
                case "__ct__11CWorldStateFUi":
                    return 0x800cd348;
                case "SetBombParams__17CHudBallInterfaceFiiibbb":
                    return 0x800e0270;
                case "__sinit_CFrontEndUI_cpp":
                    return 0x801099fc;
                case "__sinit_CMFGame_cpp":
                    return 0x8010d018;
                case "IsMapped__13CMapWorldInfoCF7TAreaId":
                    return 0x80115c20;
                case "IsAreaVisited__13CMapWorldInfoCF7TAreaId":
                    return 0x801162dc;
                case "ComputeSpringBallMovement__10CMorphBallFRC11CFinalInputR13CStateManagerf":
                    return 0x80147758;
                case "AcceptScriptMsg__7CPlayerF20EScriptObjectMessage9TUniqueIdR13CStateManager":
                    return 0x801815c4;
                case "ProcessInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x80182b04;
                case "UpdateOrbitInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x8019b96c;
                case "GetDampedClampedVelocityWR__7CPlayerCFv":
                    return 0x80192ee8;
                case "SidewaysDashAllowed__7CPlayerCFffRC11CFinalInputR13CStateManager":
                    return 0x80193594;
                case "StrafeInput__7CPlayerCFRC11CFinalInput":
                    return 0x80194d88;
                case "JumpInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x801955b0;
                case "BombJump__7CPlayerFRC9CVector3fR13CStateManager":
                    return 0x80196994;
                case "ShouldSkipCinematic__22CScriptSpecialFunctionFR13CStateManager":
                    return 0x801fca60;
                case "ThinkAreaDamage__22CScriptSpecialFunctionFfR13CStateManager":
                    return 0x801fef20;
                case "ApplyLocalDamage__13CStateManagerFRC9CVector3fRC9CVector3fR6CActorfRC11CWeaponMode":
                    return 0x8022a070;
                case "ResetControllerSettings":
                    return 0x8024aa64;
                case "ResetDisplaySettings":
                    return 0x8024aa84;
                case "ResetSoundSettings":
                    return 0x8024aab4;
                case "ResetVisorSettings":
                    return 0x8024aac8;
                case "sprintf":
                    return 0x802b6b2c;
                case "CPlayerState_PowerUpMaxValues":
                    return 0x804780a0;
                case "g_Save_FileName":
                    return 0x80479268;
                case "g_EtankCapacity":
                    return 0x805ca220;
            }
            return base.Get(function_symbol);
        }
    }
}
