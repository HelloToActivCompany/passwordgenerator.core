using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public class SimpleUniversalCoder : BytesToStringUniversalCoderBase
    {
        public SimpleUniversalCoder(char[] alphabet) : base(alphabet) { }        
        protected override string ConvertBytesToStringImplementation(byte[] data)
        {
            string xBaseString = String.Empty;

            int xBase = Alphabet.Length;

            int borderValue = xBase - 1;
            int borderValueBitsCount = GetBitCount(borderValue);

            byte mask = GetMaxNBitsNumber(borderValueBitsCount);

            int stockBits = 0;
            int stockBitsCount = 0;
            int currentIndx = 0;
            int available = 8;
            while (currentIndx < data.Length)
            {
                int currentByte = data[currentIndx];

                var need = borderValueBitsCount - stockBitsCount;

                if (available < need)
                {
                    stockBits |= ((currentByte & GetMaxNBitsNumber(available)) << available) | stockBits;
                    stockBitsCount += available;
                    available = 8;
                    currentIndx++;
                }
                else
                {
                    var inEncodeTable = ((currentByte & GetMaxNBitsNumber(need)) << stockBitsCount) | stockBits;

                    stockBits = 0;
                    stockBitsCount = 0;

                    if (inEncodeTable <= borderValue)
                    {
                        xBaseString += Alphabet[inEncodeTable];

                        if (available == need)
                        {
                            available = 8;
                            currentIndx++;
                        }
                        else
                        {
                            available = 8 - need;
                            currentByte >>= need;
                        }
                    }
                    else
                    {
                        var improvedInEncodeTable = inEncodeTable & GetMaxNBitsNumber(borderValueBitsCount - 1);
                        if ((improvedInEncodeTable ^ inEncodeTable) != 0)
                        {
                            stockBits = 1;
                            stockBitsCount = 1;
                        }

                        xBaseString += Alphabet[improvedInEncodeTable];


                        if (available == need)
                        {
                            available = 8;
                            currentIndx++;
                        }
                        else
                        {
                            available -= need;
                            currentByte >>= need;
                        }
                    }
                }
            }

            if (stockBitsCount > 0)
            {
                var inEncodeTable = stockBits << (borderValueBitsCount - stockBitsCount);
                if (inEncodeTable <= borderValue)
                    xBaseString += Alphabet[inEncodeTable];
                else
                {
                    xBaseString += Alphabet[inEncodeTable & GetMaxNBitsNumber(borderValueBitsCount - 1)];
                }
            }
                        
            return xBaseString;
        }
        private int GetBitCount(int n)
        {
            int count = 0;
            while (n > 0)
            {
                count++;
                n >>= 1;
            }

            return count;
        }

        private byte GetMaxNBitsNumber(int N)
        {
            byte res = 1;
            for (int i = 0; i < N - 1; i++)
            {
                res <<= 1;
                res += 1;
            }

            return res;
        }
    }
}
