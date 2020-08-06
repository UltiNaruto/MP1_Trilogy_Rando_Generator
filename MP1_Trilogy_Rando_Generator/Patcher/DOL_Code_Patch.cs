using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    class DOL_Code_Patch<T> : IPatch
    {
        const UInt32 MEM_BASE_ADDR = 0x80000000;
        const UInt32 MEM_DOL_START_ADDR = MEM_BASE_ADDR + 0x3F00;
        const UInt32 MEM_DOL_END_ADDR = MEM_DOL_START_ADDR + 0x132C100;
        const UInt32 MEM_ADDR_TO_FILE_ADDR = 0x3FA0;

        private readonly String Path;
        private readonly UInt32 Address;
        private readonly Object Value;

        public DOL_Code_Patch(String path, UInt32 addr, T val)
        {
            if (addr < MEM_DOL_START_ADDR || addr > MEM_DOL_END_ADDR)
                throw new Exception("addr out of memory boundaries");
            this.Path = path;
            this.Address = addr - MEM_BASE_ADDR - MEM_ADDR_TO_FILE_ADDR;
            this.Value = val;
        }

        bool OverwriteBytes(String filePath, UInt32 off, Object val)
        {
            try
            {
                byte[] buf = null;
                if (val.GetType() == typeof(Int16))
                    buf = BitConverter.GetBytes((Int16)val).Reverse().ToArray();
                if (val.GetType() == typeof(Int32))
                    buf = BitConverter.GetBytes((Int32)val).Reverse().ToArray();
                if (val.GetType() == typeof(Int64))
                    buf = BitConverter.GetBytes((Int64)val).Reverse().ToArray();
                if (val.GetType() == typeof(Byte))
                    buf = new byte[] { (Byte)val };
                if (val.GetType() == typeof(UInt16))
                    buf = BitConverter.GetBytes((UInt16)val).Reverse().ToArray();
                if (val.GetType() == typeof(UInt32))
                    buf = BitConverter.GetBytes((UInt32)val).Reverse().ToArray();
                if (val.GetType() == typeof(UInt64))
                    buf = BitConverter.GetBytes((UInt64)val).Reverse().ToArray();
                if (val.GetType() == typeof(Char))
                    buf = new byte[] { (Byte)val };
                if (val.GetType() == typeof(Single))
                    buf = BitConverter.GetBytes((Single)val).Reverse().ToArray();
                if (val.GetType() == typeof(Double))
                    buf = BitConverter.GetBytes((Double)val).Reverse().ToArray();
                if (val.GetType() == typeof(String))
                    buf = Encoding.ASCII.GetBytes((String)val);

                using (var bW = new BinaryWriter(File.OpenWrite(filePath)))
                {
                    bW.BaseStream.Position = off;
                    if (buf != null)
                        bW.Write(buf);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal override void Apply()
        {
            if (!OverwriteBytes(this.Path, this.Address, this.Value))
            {
                Console.WriteLine("Failed to patch " + this.Path + "!");
                return;
            }
        }
    }
}
