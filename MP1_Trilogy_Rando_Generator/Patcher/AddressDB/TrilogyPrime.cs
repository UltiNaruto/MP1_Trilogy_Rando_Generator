namespace MP1_Trilogy_Rando_Generator.Patcher.AddressDB
{
    class TrilogyPrime : Addresses
    {
        protected TrilogyPrime() { }

        // Restore bomb jump OOB NTSC_U
        // 801930a8
        // 80193118 jump to +0xc

        // FrontEnd NTSC_U
        // 800858d4 set hud lag setting
    }
}
