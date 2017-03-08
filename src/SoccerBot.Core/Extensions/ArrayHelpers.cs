using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core
{
    public static class ArrayHelpers
    {
        public static byte[] ToByteArray(this char[] chBuffer, int start = 0, int? len = null)
        {
            if (!len.HasValue)
            {
                len = chBuffer.Length;
            }

            var actualLength = Math.Min(chBuffer.Length, len.Value - start);
            var buffer = new byte[actualLength];
            for (var idx = start; idx < actualLength; ++idx)
            {
                buffer[idx] = (byte)chBuffer[idx];
            }

            return buffer;
        }

        public static char[] ToCharArray(this byte[] byteBuffer, int start = 0, int? len = null)
        {
            if(!len.HasValue)
            {
                len = byteBuffer.Length;
            }

            var actualLength = Math.Min(byteBuffer.Length, len.Value - start);
            var chBuffer = new char[actualLength];
            for (var idx = start; idx < actualLength; ++idx)
            {
                chBuffer[idx] = (char)byteBuffer[idx];
            }

            return chBuffer;
        }

        public static byte[] ToByteArray(this string str)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(str);
        }
    }
}
