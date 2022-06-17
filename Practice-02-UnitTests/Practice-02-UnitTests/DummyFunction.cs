using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_02_UnitTests;

public class DummyFunction
{
    public string ReturnsPikachuIfZero(int num)
    {
        if (num == 0)
        {
            return "PIKACHU!";
        }
        else
        {
            return "Squirtle...";
        }
    }
}
