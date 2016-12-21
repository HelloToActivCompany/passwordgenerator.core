using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public interface IPasswordGenerator
    {
        string Generate(string url);
    }
}
