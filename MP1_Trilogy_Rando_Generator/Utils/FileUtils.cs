using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    class FileUtils
    {
        internal static void NullifyFiles(String path, params String[] excludedFiles)
        {
            NullifyFiles(path, "*.*", false, excludedFiles);
        }

        internal static void NullifyFiles(String path, bool recursively, params String[] excludedFiles)
        {
            NullifyFiles(path, "*.*", recursively, excludedFiles);
        }

        internal static void NullifyFiles(String path, String filter, params String[] excludedFiles)
        {
            NullifyFiles(path, filter, false, excludedFiles);
        }

        internal static void NullifyFiles(String path, String filter, bool recursively, params String[] excludedFiles)
        {
            foreach (var file in Directory.EnumerateFiles(path, filter, recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                if(!excludedFiles.Contains(Path.GetFileName(file)))
                    File.WriteAllText(file, "");
        }

        internal static byte[] Read(String path, int index, int count, bool bigEndian = false)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path + " doesn't exist!");
            using (var file = File.OpenRead(path))
            using(var reader = new BinaryReader(file))
            {
                reader.BaseStream.Position = index;
                return bigEndian ? reader.ReadBytes(count).Reverse().ToArray() : reader.ReadBytes(count);
            }
        }

        internal static Object Read<T>(String path, int index, int count, bool bigEndian=false)
        {
            if (typeof(T) == typeof(String))
                return Encoding.ASCII.GetString(Read(path, index, count), 0, count);
            if (typeof(T) == typeof(Byte))
                return Read(path, index, count, bigEndian).FirstOrDefault();
            if (typeof(T) == typeof(UInt16))
                return BitConverter.ToUInt16(Read(path, index, count, bigEndian), 0);
            if (typeof(T) == typeof(UInt32))
                return BitConverter.ToUInt32(Read(path, index, count, bigEndian), 0);
            if (typeof(T) == typeof(UInt64))
                return BitConverter.ToUInt64(Read(path, index, count, bigEndian), 0);
            if (typeof(T) == typeof(SByte))
                return (SByte)Read(path, index, count, bigEndian).FirstOrDefault();
            if (typeof(T) == typeof(Int16))
                return BitConverter.ToInt16(Read(path, index, count, bigEndian), 0);
            if (typeof(T) == typeof(Int32))
                return BitConverter.ToInt32(Read(path, index, count, bigEndian), 0);
            if (typeof(T) == typeof(Int64))
                return BitConverter.ToInt64(Read(path, index, count, bigEndian), 0);
            if (typeof(T) == typeof(Single))
                return BitConverter.ToSingle(Read(path, index, count, bigEndian), 0);
            if (typeof(T) == typeof(Double))
                return BitConverter.ToDouble(Read(path, index, count, bigEndian), 0);
            throw new Exception("Unsupported type");
        }

        internal static void Write(String path, int index, int count, byte[] datas, bool bigEndian = false)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path + " doesn't exist!");
            using (var file = File.OpenWrite(path))
            using (var writer = new BinaryWriter(file))
            {
                writer.BaseStream.Position = index;
                writer.Write(bigEndian ? datas.Reverse().ToArray() : datas, 0, count);
            }
        }

        internal static void Write<T>(String path, int index, Object datas, bool bigEndian = false)
        {
            if (typeof(T) == typeof(String))
            {
                Write(path, index, ((String)datas).Length, Encoding.ASCII.GetBytes(((String)datas).ToCharArray(), 0, ((String)datas).Length));
                return;
            }
            if (typeof(T) == typeof(Byte))
            {
                Write(path, index, 1, BitConverter.GetBytes((Byte)datas), true);
                return;
            }
            if (typeof(T) == typeof(UInt16))
            {
                Write(path, index, 2, BitConverter.GetBytes((UInt16)datas), true);
                return;
            }
            if (typeof(T) == typeof(UInt32))
            {
                Write(path, index, 4, BitConverter.GetBytes((UInt32)datas), true);
                return;
            }
            if (typeof(T) == typeof(UInt64))
            {
                Write(path, index, 8, BitConverter.GetBytes((UInt64)datas), true);
                return;
            }
            if (typeof(T) == typeof(SByte))
            {
                Write(path, index, 1, BitConverter.GetBytes((SByte)datas), true);
                return;
            }
            if (typeof(T) == typeof(Int16))
            {
                Write(path, index, 2, BitConverter.GetBytes((Int16)datas), true);
                return;
            }
            if (typeof(T) == typeof(Int32))
            {
                Write(path, index, 4, BitConverter.GetBytes((Int32)datas), true);
                return;
            }
            if (typeof(T) == typeof(Int64))
            {
                Write(path, index, 8, BitConverter.GetBytes((Int64)datas), true);
                return;
            }
            if (typeof(T) == typeof(Single))
            {
                Write(path, index, 4, BitConverter.GetBytes((Single)datas), true);
                return;
            }
            if (typeof(T) == typeof(Double))
            {
                Write(path, index, 8, BitConverter.GetBytes((Double)datas), true);
                return;
            }
            throw new Exception("Unsupported type");
        }
    }
}
