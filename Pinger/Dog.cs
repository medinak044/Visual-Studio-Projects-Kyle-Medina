using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinger
{
    public class Dog : IDogWalker
    {
        public void DogBarking()
        {
            Console.WriteLine("The dog barks");
        }

        public void WalkToTheCity()
        {
            Console.WriteLine("Walk to the city");
        }

        public void WalkToTheFoodBowl()
        {
            Console.WriteLine("Walk to the food bowl");
        }

        public void WalkToThePark()
        {
            Console.WriteLine("Walk to the park");
        }
    }
}
