using System;

namespace MP1_Trilogy_Rando_Generator.Patcher.Patches.Prime
{
    class PAL : TrilogyPrime
    {
        internal override String MP1_Dol_Path { get => ".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol"; }
        internal override String FE_Dol_Path { get => ".\\tmp\\wii\\DATA\\files\\rs5fe_p.dol"; }
        internal override String Main_Dol_Path { get => ".\\tmp\\wii\\DATA\\sys\\main.dol"; }

        internal PAL()
        {
            _addresses = new AddressDB.Prime.A_PAL();
        }
    }
}
