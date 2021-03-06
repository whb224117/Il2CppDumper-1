﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Il2CppDumper
{
    class MachoFat : MyBinaryReader
    {
        public Fat[] fats;

        public MachoFat(Stream stream) : base(stream)
        {
            //BigEndian
            Position += 4;
            var size = BitConverter.ToInt32(ReadBytes(4).Reverse().ToArray(), 0);
            fats = new Fat[size];
            for (var i = 0; i < size; i++)
            {
                Position += 8;
                fats[i] = new Fat();
                fats[i].file_offset = BitConverter.ToUInt32(ReadBytes(4).Reverse().ToArray(), 0);
                fats[i].size = BitConverter.ToUInt32(ReadBytes(4).Reverse().ToArray(), 0);
                Position += 4;
            }
            for (var i = 0; i < size; i++)
            {
                Position = fats[i].file_offset;
                fats[i].magic = ReadUInt32();
            }
        }

        public byte[] GetMacho(int index)
        {
            Position = fats[index].file_offset;
            return ReadBytes((int)fats[index].size);
        }
    }
}
