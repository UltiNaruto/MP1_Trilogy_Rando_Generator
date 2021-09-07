using System;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    abstract class Addresses
    {
        protected Addresses _FrontEnd;
        internal Addresses FrontEnd { get => _FrontEnd; }

        internal virtual UInt32 Get(String function_symbol) {
            throw new NotImplementedException();
        }
        internal String GetAsStr(String function_symbol, UInt32 offset = 0) {
            return String.Format("0x{0:X8}", Get(function_symbol) + offset).ToLower();
        }
        internal UInt16 GetHiBitsFrom(String function_symbol, UInt32 offset = 0) {
            UInt32 address = Get(function_symbol) + offset;
            address &= 0xFFFF0000;
            return (UInt16)(address >> 16);
        }
        internal String GetHiBitsAsStr(String function_symbol, UInt32 offset = 0) {
            return String.Format("0x{0:X4}", GetHiBitsFrom(function_symbol, offset)).ToLower();
        }
        internal UInt16 GetLoBitsFrom(String function_symbol, UInt32 offset = 0) {
            UInt32 address = Get(function_symbol) + offset;
            address &= 0xFFFF;
            return (UInt16)address;
        }
        internal String GetLoBitsAsStr(String function_symbol, UInt32 offset = 0) {
            return String.Format("0x{0:X4}", GetLoBitsFrom(function_symbol, offset)).ToLower();
        }
    }
}
