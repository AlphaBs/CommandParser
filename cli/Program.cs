using CommandParser;

Console.WriteLine("Input arguments: ");
int num = 1;
foreach (var arg in args)
{
    Console.WriteLine($"#{num}: {arg}");
    num++;
}
Console.WriteLine();

Console.WriteLine("Parsed arguments: ");
var result = Parser.ParseToKeyValueArguments(args).ToArray();
num = 1;
foreach (var arg in result)
{
    Console.WriteLine($"#{num}");
    Console.WriteLine($"key: {arg.Key}");
    if (arg.Value != null)
    {
        Console.WriteLine($"value: {arg.Value}");
    }
    Console.WriteLine();
    num++;
}

Console.WriteLine("Serialized results: ");
Console.WriteLine(string.Join<KeyValueArgument>(" ", result));