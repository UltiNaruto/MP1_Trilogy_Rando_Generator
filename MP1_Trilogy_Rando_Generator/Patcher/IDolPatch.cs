using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    class IDolPatch : IPatch
    {
        struct MemSection
        {
            public UInt32 FileAddress;
            public UInt32 MemoryAddress;
            public UInt32 Size;
        }

        List<MemSection> textSections = new List<MemSection>();
        List<MemSection> dataSections = new List<MemSection>();

        public IDolPatch(String path)
        {
            int i = 0;
            MemSection section = default(MemSection);
            using (var file = File.OpenRead(path))
            using (var reader = new BinaryReader(file))
            {
                for (i = 0; i < 7; i++)
                {
                    section = new MemSection();
                    reader.BaseStream.Position = i * 4;
                    section.FileAddress = BitConverter.ToUInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                    if(section.FileAddress != 0)
                    {
                        reader.BaseStream.Position = 0x48 + i * 4;
                        section.MemoryAddress = BitConverter.ToUInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                        reader.BaseStream.Position = 0x90 + i * 4;
                        section.Size = BitConverter.ToUInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                        textSections.Add(section);
                    }
                }
                for (i = 0; i < 11; i++)
                {
                    section = new MemSection();
                    reader.BaseStream.Position = 0x1C + i * 4;
                    section.FileAddress = BitConverter.ToUInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                    if (section.FileAddress != 0)
                    {
                        reader.BaseStream.Position = 0x64 + i * 4;
                        section.MemoryAddress = BitConverter.ToUInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                        reader.BaseStream.Position = 0xAC + i * 4;
                        section.Size = BitConverter.ToUInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                        dataSections.Add(section);
                    }
                }
            }
        }

        internal UInt32 GetFileAddress(UInt32 mem_addr)
        {
            foreach(var textSection in textSections)
                if((mem_addr - textSection.MemoryAddress) < textSection.Size)
                    return textSection.FileAddress + (mem_addr - textSection.MemoryAddress);
            foreach (var dataSection in dataSections)
                if ((mem_addr - dataSection.MemoryAddress) < dataSection.Size)
                    return dataSection.FileAddress + (mem_addr - dataSection.MemoryAddress);
            return 0;
        }

        internal override void Apply()
        {
        }
    }
}
