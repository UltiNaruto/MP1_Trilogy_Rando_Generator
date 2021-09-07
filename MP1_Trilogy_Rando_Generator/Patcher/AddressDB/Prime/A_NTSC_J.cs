using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.AddressDB.Prime
{
    class A_NTSC_J : TrilogyPrime
    {
        internal A_NTSC_J()
        {
            _FrontEnd = new FrontEnd.A_FE_NTSC_J_MP1();
        }

        internal override UInt32 Get(String function_symbol)
        {
            switch (function_symbol)
            {
                case "LoadSettingsFromFrontEnd":
                    return 0x800c9550;
                case "__ct__11CWorldStateFUi":
                    return 0x800cd374;
                case "SetBombParams__17CHudBallInterfaceFiiibbb":
                    return 0x800e04ec;
                case "__sinit_CFrontEndUI_cpp":
                    return 0x80109ed4;
                case "__sinit_CMFGame_cpp":
                    return 0x8010d4f0;
                case "IsMapped__13CMapWorldInfoCF7TAreaId":
                    return 0x801160f8;
                case "IsAreaVisited__13CMapWorldInfoCF7TAreaId":
                    return 0x801167b4;
                case "ComputeSpringBallMovement__10CMorphBallFRC11CFinalInputR13CStateManagerf":
                    return 0x80147c08;
                case "AcceptScriptMsg__7CPlayerF20EScriptObjectMessage9TUniqueIdR13CStateManager":
                    return 0x80181eac;
                case "ProcessInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x801833ec;
                case "UpdateOrbitInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x8019c254;
                case "GetDampedClampedVelocityWR__7CPlayerCFv":
                    return 0x801937d0;
                case "SidewaysDashAllowed__7CPlayerCFffRC11CFinalInputR13CStateManager":
                    return 0x80193e7c;
                case "StrafeInput__7CPlayerCFRC11CFinalInput":
                    return 0x80195670;
                case "JumpInput__7CPlayerFRC11CFinalInputR13CStateManager":
                    return 0x80195e98;
                case "BombJump__7CPlayerFRC9CVector3fR13CStateManager":
                    return 0x8019727c;
                case "ShouldSkipCinematic__22CScriptSpecialFunctionFR13CStateManager":
                    return 0x801fd344;
                case "ThinkAreaDamage__22CScriptSpecialFunctionFfR13CStateManager":
                    return 0x801ff788;
                case "ApplyLocalDamage__13CStateManagerFRC9CVector3fRC9CVector3fR6CActorfRC11CWeaponMode":
                    return 0x8022a8d8;
                case "ResetControllerSettings":
                    return 0x8024b2b8;
                case "ResetDisplaySettings":
                    return 0x8024b2d4;
                case "ResetSoundSettings":
                    return 0x8024b304;
                case "ResetVisorSettings":
                    return 0x8024b318;
                case "sprintf":
                    return 0x802b6568;
                case "CPlayerState_PowerUpMaxValues":
                    return 0x80473e68;
                case "g_Save_FileName":
                    return 0x80475030;
                case "g_EtankCapacity":
                    return 0x806461c8;
            }
            return base.Get(function_symbol);
        }
    }
}
