using CommandParser;

Console.WriteLine("Input arguments: ");
int num = 1;
foreach (var arg in args)
{
    Console.WriteLine($"#{num}: {arg}");
}
Console.WriteLine();
Console.WriteLine("Serialized input: ");
var input = string.Join(" ", args);
Console.WriteLine(input);
Console.WriteLine();

Console.WriteLine("Parsed arguments: ");
var result = Parser.ParseArguments(input).ToArray();
num = 1;
foreach (var arg in result)
{
    Console.WriteLine($"#{num}");
    Console.WriteLine($"key: {arg.Key}");
    if (arg.Values != null)
    {
        foreach (var v in arg.Values)
            Console.WriteLine($"value: {v}");
    }
    Console.WriteLine();
    num++;
}

Console.WriteLine("Serialized results: ");
Console.WriteLine(string.Join<KeyValueArgument>(" ", result));