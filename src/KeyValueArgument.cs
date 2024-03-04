namespace CommandParser;

public record KeyValueArgument(string Key, string? Value)
{
    public override string ToString()
    {
        if (Value == null)
            return Key;
        else
            return Key + "=" + Value;
    }
}