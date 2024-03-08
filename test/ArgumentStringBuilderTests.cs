namespace CommandParser.Tests;

public class ArgumentStringBuilderTests
{
    [Fact]
    public void add_strings()
    {
        var builder = new ArgumentStringBuilder();
        builder.AddArgumentString("--key");
        builder.AddArgumentString("value");
        builder.AddArgumentString("-a=b");
        builder.AddArgumentString("-c d");
        builder.AddArgumentString("--key value1 value2 -x=y value3");
        Assert.Equal("--key value -a=b -c d --key value1 value2 -x=y value3", builder.Build());
    }

    [Fact]
    public void parse_input_string()
    {
        var builder = new ArgumentStringBuilder();
        builder.AddArgumentString("--key");
        builder.AddArgumentString("value");
        builder.AddArgumentString("-a=b");
        builder.AddArgumentString("-c d");
        builder.AddArgumentString("--key value1 value2 -x=y value3");
        KeyValueArgument[] expected =
        [
            KeyValueArgument.CreateWithoutValidation("--key", null),
            KeyValueArgument.CreateWithoutValidation("", "value"),
            KeyValueArgument.CreateWithoutValidation("-a", "b"),
            KeyValueArgument.CreateWithoutValidation("-c", "d"),
            KeyValueArgument.CreateWithoutValidation("--key", "value1"),
            KeyValueArgument.CreateWithoutValidation("", "value2"),
            KeyValueArgument.CreateWithoutValidation("-x", "y"),
            KeyValueArgument.CreateWithoutValidation("", "value3"),
        ];
        Assert.Equal(expected, builder.Arguments.ToArray());
    }

    [Fact]
    public void check_key_is_contained()
    {
        var builder = new ArgumentStringBuilder();
        builder.AddArgumentString("--key");
        builder.AddArgumentString("value");
        builder.AddArgumentString("-a=b");
        builder.AddArgumentString("-c d");
        builder.AddArgumentString("--key value1 value2 -x=y value3");

        Assert.True(builder.ContainsKey("--key"));
        Assert.True(builder.ContainsKey("-a"));
        Assert.True(builder.ContainsKey("-c"));
        Assert.True(builder.ContainsKey("-x"));
        Assert.False(builder.ContainsKey("value"));
        Assert.False(builder.ContainsKey("value1"));
        Assert.False(builder.ContainsKey("value2"));
        Assert.False(builder.ContainsKey("value3"));
        Assert.False(builder.ContainsKey("b"));
        Assert.False(builder.ContainsKey("d"));
        Assert.False(builder.ContainsKey("y"));
    }

    [Fact]
    public void find_arguments_with_key()
    {
        var builder = new ArgumentStringBuilder();
        builder.AddArgumentString("--key1");
        builder.AddArgumentString("--key1=b");
        builder.AddArgumentString("--key1 c d");

        KeyValueArgument[] expected = 
        [
            KeyValueArgument.CreateWithoutValidation("--key1", null),
            KeyValueArgument.CreateWithoutValidation("--key1", "b"),
            KeyValueArgument.CreateWithoutValidation("--key1", "c"),
        ];
        Assert.Equal(expected, builder.Find("--key1"));
    }

    [Fact]
    public void find_arguments_with_no_matched_key()
    {
        var builder = new ArgumentStringBuilder();
        builder.AddArgumentString("--key1=value");

        Assert.Empty(builder.Find("--no-key"));
    }
}