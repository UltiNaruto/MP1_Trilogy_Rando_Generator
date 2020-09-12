using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    class DOL_Patch<T> : IDolPatch
    {
        private readonly String Path;
        private readonly UInt32 Address;
        private readonly Object Value;

        public DOL_Patch(String path, UInt32 addr, T val) : base(path)
        {
            this.Path = path;
            this.Address = addr;
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
                    buf = Encoding.ASCII.GetBytes(((String)val).ToCharArray(), 0, ((String)val).Length);

                using (var bW = new BinaryWriter(File.OpenWrite(filePath)))
                {
                    bW.BaseStream.Position = this.GetFileAddress(off);
                    if (bW.BaseStream.Position == 0)
                        throw new Exception("Cannot patch at "+String.Format("0x{0:X8}", off)+"!\nMemory block isn't pre initialized!");
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
