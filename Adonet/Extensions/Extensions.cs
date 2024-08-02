using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Adonet.Extensions
{
    public static class Extensions
    {
        public static bool isValidChoice(this char symbol)
        {
            if (symbol.ToString().ToLower().Equals("y") || symbol.ToString().ToLower().Equals("n"))
            {
                return true;
            }
            return false;
        }
    }
}
