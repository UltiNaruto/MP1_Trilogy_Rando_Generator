using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    class DOL_ScrollCode_Patch : IPatch
    {
        const UInt32 MEM_BASE_ADDR = 0x80000000;
        const UInt32 MEM_DOL_START_ADDR = MEM_BASE_ADDR + 0x3F00;
        const UInt32 MEM_DOL_END_ADDR = MEM_DOL_START_ADDR + 0x132C100;
        const UInt32 MEM_ADDR_TO_FILE_ADDR = 0x3FA0;

        private readonly String Path;
        private readonly UInt32 StartAddress;
        private readonly UInt32 EndAddress;
        private readonly Int32 Offset;

        public DOL_ScrollCode_Patch(String path, UInt32 start, UInt32 end, Int32 offset)
        {
            var tmp = default(UInt32);
            if(start > end)
            {
                tmp = end;
                end = start;
                start = tmp;
            }
            if (offset < 0)
            {
                if (start + offset < MEM_DOL_START_ADDR || end > MEM_DOL_END_ADDR)
                    throw new Exception("addr out of memory boundaries");
            }
            else if(offset > 0)
            {
                if (start < MEM_DOL_START_ADDR || end + offset > MEM_DOL_END_ADDR)
                    throw new Exception("addr out of memory boundaries");
            }
            else
            {
                if (start < MEM_DOL_START_ADDR || end > MEM_DOL_END_ADDR)
                    throw new Exception("addr out of memory boundaries");
            }
            this.Path = path;
            this.StartAddress = start - MEM_BASE_ADDR - MEM_ADDR_TO_FILE_ADDR;
            this.EndAddress = end - MEM_BASE_ADDR - MEM_ADDR_TO_FILE_ADDR;
            this.Offset = offset;
        }
        
        // [A3C - AA0] -= 4
        internal override void Apply()
        {
            var len = default(Int32);
            var bytes = default(UInt32[]);
            var i = default(Int32);
            if(StartAddress % 4 != 0)
                throw new Exception("StartAddress must be aligned to 4 bytes");
            if(EndAddress % 4 != 0)
                throw new Exception("EndAddress must be aligned to 4 bytes");
            
            len = (Int32)(EndAddress - StartAddress)/4;
            bytes = new UInt32[len];

            i = 0;
            using (var file = File.OpenRead(Path))
            using (var bR = new BinaryReader(file))
            {
                bR.BaseStream.Position = StartAddress;
                while (i < len)
                    bytes[i++] = BitConverter.ToUInt32(bR.ReadBytes(4), 0);
            }

            i = 0;
            using (var file = File.OpenWrite(Path))
            using (var bW = new BinaryWriter(file))
            {
                bW.BaseStream.Position = StartAddress + Offset;
                while(i<bytes.Length)
                    bW.Write(BitConverter.GetBytes(bytes[i++]));
            }
        }
    }
}
