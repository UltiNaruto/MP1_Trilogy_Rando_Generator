﻿using ppcasm_cs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    internal class DolPatch
    {
        protected struct MemSection
        {
            public UInt32 FileAddress;
            public UInt32 MemoryAddress;
            public UInt32 Size;
        }

        public enum PatchType
        {
            AddSection,
            BytePatching,
            ScrollCode
        }

        public enum SectionType
        {
            TEXT,
            DATA
        }

        protected List<MemSection> textSections = new List<MemSection>();
        protected List<MemSection> dataSections = new List<MemSection>();

        #region Add Section properties
        private readonly UInt32 StartAddress;
        private readonly UInt32 EndAddress;
        private readonly Int32 Offset;
        #endregion

        #region Byte Patching properties
        private readonly UInt32 Address;
        private readonly byte[] Datas;
        #endregion

        #region Scroll Code properties
        private readonly UInt32 Size;
        private readonly SectionType _SectionType;
        #endregion

        private readonly String Path;
        private readonly PatchType _PatchType;

        protected DolPatch(String path)
        {
            int i = 0;
            MemSection section = default(MemSection);

            if (!File.Exists(path))
                throw new FileNotFoundException(this.Path + " doesn't exist");

            this.Path = path;

            using (var file = File.OpenRead(this.Path))
            using (var reader = new BinaryReader(file))
            {
                for (i = 0; i < 7; i++)
                {
                    section = new MemSection();
                    reader.BaseStream.Position = i * 4;
                    section.FileAddress = BitConverter.ToUInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                    if (section.FileAddress != 0)
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

        public DolPatch(String path, UInt32 start, Int32 offset) : this(path)
        {
            this.StartAddress = start;
            this.EndAddress = (uint)((int)start + offset);
            if (this.GetFileAddress((UInt32)(this.StartAddress + offset)) == 0 ||
                this.GetFileAddress((UInt32)(this.EndAddress + offset)) == 0)
                throw new Exception("addr out of memory boundaries");
            this.Offset = offset;
            this._PatchType = PatchType.ScrollCode;
        }

        public DolPatch(String path, SectionType type, UInt32 mem_addr, UInt32 size) : this(path)
        {
            this.Address = mem_addr;
            this.Size = size;
            this._SectionType = type;
            this._PatchType = PatchType.AddSection;
        }

        public DolPatch(String path, PPC_Instruction instruction) : this(path)
        {
            this._PatchType = PatchType.BytePatching;
            this.Address = instruction.Address;
            this.Datas = instruction.Value;
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

        void AddTextSection()
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

        void AddDataSection()
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

        bool OverwriteBytes(String filePath, UInt32 off)
        {
            try
            {
                using (var bW = new BinaryWriter(File.OpenWrite(filePath)))
                {
                    bW.BaseStream.Position = this.GetFileAddress(off);
                    if (bW.BaseStream.Position == 0)
                        throw new Exception("Cannot patch at " + String.Format("0x{0:X8}", off) + "!\nMemory block isn't pre initialized!");
                    if (Datas != null)
                        bW.Write(Datas);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal void Apply()
        {
            var len = default(Int32);
            var i = default(Int32);
            var bytes = default(UInt32[]);

            if(_PatchType == PatchType.ScrollCode)
            {
                if (this.StartAddress % 4 != 0)
                    throw new Exception("StartAddress must be aligned to 4 bytes");
                if (this.EndAddress % 4 != 0)
                    throw new Exception("EndAddress must be aligned to 4 bytes");

                len = (Int32)(EndAddress - StartAddress) / 4;
                bytes = new UInt32[len];

                i = 0;
                using (var file = File.OpenRead(Path))
                using (var bR = new BinaryReader(file))
                {
                    bR.BaseStream.Position = this.GetFileAddress(StartAddress);
                    if (bR.BaseStream.Position == 0)
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
                        throw new Exception("Cannot patch at " + String.Format("0x{0:X8}", StartAddress + Offset) + "!\nMemory block isn't pre initialized!");
                    while (i < bytes.Length)
                        bW.Write(BitConverter.GetBytes(bytes[i++]));
                }
            }
            if(_PatchType == PatchType.AddSection)
            {
                if (this._SectionType == SectionType.TEXT)
                    this.AddTextSection();
                else if (this._SectionType == SectionType.DATA)
                    this.AddDataSection();
                else
                    throw new Exception("Unknown type of section");
            }
            if (_PatchType == PatchType.BytePatching)
            {
                if (!OverwriteBytes(this.Path, this.Address))
                    throw new Exception("Failed to patch " + this.Path + "!");
            }
        }
    }
}
