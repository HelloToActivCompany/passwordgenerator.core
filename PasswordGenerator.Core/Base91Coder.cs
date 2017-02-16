using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public class Base91Coder
    {
        private static char[] EncodeTable = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '$',
            '%', '&', '(', ')', '*', '+', ',', '.', '/', ':', ';', '<', '=',
            '>', '?', '@', '[', ']', '^', '_', '`', '{', '|', '}', '~', '"'
        };

        public static char[] GetLowerCaseChars()
        {
            return "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        }

        public static char[] GetUpperCaseChars()
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        }

        public static char[] GetDigits()
        {
            return "0123456789".ToCharArray();
        }

        public static char[] GetSpecialChars()
        {
            return "!#$%&()*+,./:;<=>?@[]^_`{|}~\"".ToCharArray();
        }

        public static bool CharIsSpecial(char c)
        {
            return GetSpecialChars().Any(item => item == c);
        }

        public string ToBase91String(byte[] data)
        {
            string base91String = "";
            int b = 0;
            int n = 0;
            int v = 0;
            for (int i = 0; i < data.Length; i++)
            {
                b |= (byte)data[i] << n;
                n += 8;
                if (n > 13)
                {
                    v = b & 8191;
                    if (v > 88)
                    {
                        b >>= 13;
                        n -= 13;
                    }
                    else
                    {
                        v = b & 16383;
                        b >>= 14;
                        n -= 14;
                    }
                    base91String += (char)EncodeTable[v % 91];
                    base91String += (char)EncodeTable[v / 91];
                }
            }
            if (n != 0)
            {
                base91String += (char)EncodeTable[b % 91];
                if (n > 7 || b > 90)
                    base91String += (char)EncodeTable[b / 91];
            }
            return base91String;
        }
    }
}
