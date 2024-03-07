namespace CommandParser.Tests;

public class KeyValueArgumentTests
{
    [Fact]
    public void disallow_empty_key()
    {
        Assert.Throws<FormatException>(() => 
            KeyValueArgument.Create("", null));
    }

    [Fact]
    public void disallow_key_without_prefix()
    {
        Assert.Throws<FormatException>(() =>
            KeyValueArgument.Create("abc", null));
    }
}