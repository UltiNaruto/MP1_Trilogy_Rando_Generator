using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.AddressDB.FrontEnd
{
    class A_FE_PAL : TrilogyPrime
    {
        internal override UInt32 Get(String function_symbol)
        {
            switch (function_symbol)
            {
                case "LoadSettingsForProfile":
                    return 0x8019207c;
                case "g_Save_FileName":
                    return 0x805340a8;
            }
            return base.Get(function_symbol);
        }
    }
}
