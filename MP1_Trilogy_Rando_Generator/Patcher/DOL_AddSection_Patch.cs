using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    class DOL_AddSection_Patch : IDolPatch
    {
        public enum SectionType {
            TEXT,
            DATA
        }
        private readonly String Path;
        private readonly UInt32 Address;
        private readonly UInt32 Size;
        private readonly SectionType Type;

        public DOL_AddSection_Patch(String path, SectionType type, UInt32 mem_addr, UInt32 size) : base(path)
        {
            this.Path = path;
            this.Address = mem_addr;
            this.Size = size;
            this.Type = type;
        }

        internal void AddTextSection()
        {
            if (this.textSections.Count + 1 > 7)
                throw new Exception("Too much text sections are present! Cannot add a new text section");
            int i = this.textSections.Count;
            UInt32 size = (UInt32)new FileInfo(this.Path).Length;

            using (var file = File.OpenWrite(this.Path))
            using (var writer = new BinaryWriter(file))
            {
                writer.BaseStream.Position = i * 4;
                writer.Write(BitConverter.GetBytes(size).Reverse().ToArray());
                writer.BaseStream.Position = 0x48 + i * 4;
                writer.Write(BitConverter.GetBytes(this.Address).Reverse().ToArray());
                writer.BaseStream.Position = 0x90 + i * 4;
                writer.Write(BitConverter.GetBytes(this.Size).Reverse().ToArray());

                writer.BaseStream.Position = size;
                for (i = 0; i < this.Size; i++)
                    writer.Write(0);
            }
        }

        internal void AddDataSection()
        {
            if (this.dataSections.Count + 1 > 11)
                throw new Exception("Too much data sections are present! Cannot add a new data section");
            int i = this.textSections.Count;
            UInt32 size = (UInt32)new FileInfo(this.Path).Length;

            using (var file = File.OpenWrite(this.Path))
            using (var writer = new BinaryWriter(file))
            {
                writer.BaseStream.Position = 0x1C + i * 4;
                writer.Write(BitConverter.GetBytes(size).Reverse().ToArray());
                writer.BaseStream.Position = 0x64 + i * 4;
                writer.Write(BitConverter.GetBytes(this.Address).Reverse().ToArray());
                writer.BaseStream.Position = 0xAC + i * 4;
                writer.Write(BitConverter.GetBytes(this.Size).Reverse().ToArray());

                writer.BaseStream.Position = size;
                for (i = 0; i < this.Size; i++)
                    writer.Write(0);
            }
        }

        internal override void Apply()
        {
            if (!File.Exists(this.Path))
                throw new FileNotFoundException(this.Path + " doesn't exist");
            if (this.Type == SectionType.TEXT)
                this.AddTextSection();
            else if (this.Type == SectionType.DATA)
                this.AddDataSection();
            else
                throw new Exception("Unknown type of section");
        }
    }
}
