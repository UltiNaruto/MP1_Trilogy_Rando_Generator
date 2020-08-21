using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    class DOL_ScrollCode_Patch : IDolPatch
    {
        private readonly String Path;
        private readonly UInt32 StartAddress;
        private readonly UInt32 EndAddress;
        private readonly Int32 Offset;

        public DOL_ScrollCode_Patch(String path, UInt32 start, UInt32 end, Int32 offset) : base(path)
        {
            var tmp = default(UInt32);
            if(start > end)
            {
                tmp = end;
                end = start;
                start = tmp;
            }
            if(this.GetFileAddress((UInt32)(start + offset)) == 0 ||
                this.GetFileAddress((UInt32)(end + offset)) == 0)
            throw new Exception("addr out of memory boundaries");
            this.Path = path;
            this.StartAddress = start;
            this.EndAddress = end;
            this.Offset = offset;
        }
        
        // [A3C - AA0] -= 4
        internal override void Apply()
        {
            var len = default(Int32);
            var bytes = default(UInt32[]);
            var i = default(Int32);
            if(this.StartAddress % 4 != 0)
                throw new Exception("StartAddress must be aligned to 4 bytes");
            if(this.EndAddress % 4 != 0)
                throw new Exception("EndAddress must be aligned to 4 bytes");
            
            len = (Int32)(EndAddress - StartAddress)/4;
            bytes = new UInt32[len];

            i = 0;
            using (var file = File.OpenRead(Path))
            using (var bR = new BinaryReader(file))
            {
                bR.BaseStream.Position = this.GetFileAddress(StartAddress);
                if(bR.BaseStream.Position == 0)
                    throw new Exception("Cannot patch at " + String.Format("0x{0:X8}", StartAddress) + "!\nMemory block isn't pre initialized!");
                while (i < len)
                    bytes[i++] = BitConverter.ToUInt32(bR.ReadBytes(4), 0);
            }

            i = 0;
            using (var file = File.OpenWrite(Path))
            using (var bW = new BinaryWriter(file))
            {
                bW.BaseStream.Position = this.GetFileAddress((UInt32)(StartAddress + Offset));
                if (bW.BaseStream.Position == 0)
                    throw new Exception("Cannot patch at " + String.Format("0x{0:X8}", StartAddress+Offset) + "!\nMemory block isn't pre initialized!");
                while(i<bytes.Length)
                    bW.Write(BitConverter.GetBytes(bytes[i++]));
            }
        }
    }
}
