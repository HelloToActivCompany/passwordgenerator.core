using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public interface IStringBytesConvertable
    {
        byte[] ConvertStringToBytes(String str);
        string ConvertBytesToString(byte[] bytes);
    }
}
