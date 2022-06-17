using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_02_UnitTests.Tests;

public class DummyTests
{
    // Naming Convention: ClassName_MethodName_ExpectedResult
    public static void DummyFunction_ReturnsPikachuIfZero_ReturnString()
    {
        try
        {
            // Arrange: Go get your variables, classes, whatever you need
            int num = 0;
            DummyFunction dummyFunc = new DummyFunction();

            // Act: Execute this function/method
            string result = dummyFunc.ReturnsPikachuIfZero(num);

            // Assert: Whatever is returned, is it what you expected?
            if (result == "PIKACHU!")
            {
                Console.WriteLine("PASSED: DummyFunction_ReturnsPikachuIfZero_ReturnsString");
            }
            else
            {
                Console.WriteLine("FAILED: DummyFunction_ReturnsPikachuIfZero_ReturnsString");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
