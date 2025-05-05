using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingReal.Model;

internal interface ISaveable<T>
{
    string ToString();
    T FromString(string input);
}
