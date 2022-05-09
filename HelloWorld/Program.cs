using HelloWorld;

void Func_1()
{
    // See https://aka.ms/new-console-template for more information
    Console.WriteLine("Hello, World!");
    string testStr = "This is a test";

    Console.WriteLine($"{testStr}."); // Template string

    int num1 = 2;
    int num2 = 3;
    int testInt = Multiply(num1, num2);

    int Multiply(int n1, int n2) { return n1 * n2; }

    Console.WriteLine($"The product of {num1} and {num2} is: {testInt}");


    // Cast from string to int
    StringToInt("6326445");
    void StringToInt(string str)
    {
        //int result = 0;
        if (int.TryParse(str, out int result))
        { Console.WriteLine($"Success: {result}"); }
        else
        { Console.WriteLine("Failure"); }
    }
}

void Func_2()
{
    List<Part> someList = new List<Part>();

    someList.Add(new Part{ PartName = "Headlight", PartId = 1 });
    someList.Add(new Part{ PartName = "Bumper", PartId = 2 });

    for (int i = 0; i < someList.Count; i++)
    { Console.WriteLine($"Partname: {someList[i].PartName}, PartId: {someList[i].PartId}"); }
}

Func_2();