using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.AddressDB.Prime
{
    class A_NTSC_U : TrilogyPrime
    {
        internal A_NTSC_U()
        {
            _FrontEnd = new FrontEnd.A_FE_NTSC_U();
        }

        internal override UInt32 Get(String function_symbol)
        {
            switch(function_symbol)
            {
                case "LoadSettingsFromFrontEnd":
                    return 0x800c9444;
                case "__ct__11CWorldStateFUi":
                    return 0x800cd1f8;
                case "SetBombParams__17CHudBallInterfaceFiiibbb":
                    return 0x800e0180;
                case "__sinit_CFrontEndUI_cpp":
                    return 0x801098e0;
                case "__sinit_CMFGame_cpp":
                    return 0x8010cefc;
                case "IsMapped__13CMapWorldInfoCF7TAreaId":
                    return 0x80115b04;
                case "IsAreaVisited__13CMapWorldInfoCF7TAreaId":
                    return 0x801161c0;
                case "ComputeSpringBallMovement__10CMorphBallFRC11CFinalInputR13CStateManagerf":
                    return 0x80147608;
                case "AcceptScriptMsg__7CPlayerF20EScriptObjectMessage9TUniqueIdR13CStateManager":
                    return 0x8018132c;
                case "ProcessInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x8018286c;
                case "UpdateOrbitInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x8019b6d4;
                case "GetDampedClampedVelocityWR__7CPlayerCFv":
                    return 0x80192c50;
                case "SidewaysDashAllowed__7CPlayerCFffRC11CFinalInputR13CStateManager":
                    return 0x801932fc;
                case "StrafeInput__7CPlayerCFRC11CFinalInput":
                    return 0x80194af0;
                case "JumpInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x80195318;
                case "BombJump__7CPlayerFRC9CVector3fR13CStateManager":
                    return 0x801966fc;
                case "ShouldSkipCinematic__22CScriptSpecialFunctionFR13CStateManager":
                    return 0x801fc70c;
                case "ThinkAreaDamage__22CScriptSpecialFunctionFfR13CStateManager":
                    return 0x801febcc;
                case "ApplyLocalDamage__13CStateManagerFRC9CVector3fRC9CVector3fR6CActorfRC11CWeaponMode":
                    return 0x80229d1c;
                case "ResetControllerSettings":
                    return 0x8024a734;
                case "ResetDisplaySettings":
                    return 0x8024a754;
                case "ResetSoundSettings":
                    return 0x8024a784;
                case "ResetVisorSettings":
                    return 0x8024a798;
                case "sprintf":
                    return 0x802b68e8;
                case "CPlayerState_PowerUpMaxValues":
                    return 0x80474b80;
                case "g_Save_FileName":
                    return 0x80475d48;
                case "g_EtankCapacity":
                    return 0x805c5e48;
            }
            return base.Get(function_symbol);
        }
    }
}
