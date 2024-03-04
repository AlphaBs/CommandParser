namespace CommandParser.Tests;

public class ParserTests
{
    private static void assert(string input, (string, string?)[] expected)
    {
        var expectedArgs = expected.Select(tuple => new KeyValueArgument(tuple.Item1, tuple.Item2)).ToArray();
        var actualArgs = Parser.ParseArguments(input);
        Assert.Equal(expectedArgs, actualArgs);
    }

    [Fact]
    public void parse_empty_inputs()
    {
        assert("", []);
        assert(" \n \r \t   ", []);
    }

    [Fact]
    public void parse_simple_keys()
    {
        assert("a b c", [("a", null), ("b", null), ("c", null)]);
    }

    [Fact]
    public void ignore_spaces()
    {
        assert("  a \t  b \r\n\n  c  ", [("a", null), ("b", null), ("c", null)]);
    }

    [Fact]
    public void parse_key_value_pairs()
    {
        assert("a b=c", [("a", null), ("b", "c")]);
        assert("=a b=", [("", "a"), ("b", "")]);
        assert("a======b", [("a", "=====b")]);
    }

    [Fact]
    public void empty_key_value()
    {
        assert("=", [("", "")]);
    }

    [Fact]
    public void ignore_spaces_in_quotes()
    {
        assert("a \"b c\"", [("a", null), ("\"b c\"", null)]);
        assert("a=\"b \n c\" \"d \t e\"=f", [("a", "\"b \n c\""), ("\"d \t e\"", "f")]);
    }

    [Fact]
    public void ignore_equals_in_quotes()
    {
        assert("\"b=c\"", [("\"b=c\"", null)]);
        assert("ab\"=\"cd", [("ab\"=\"cd", null)]);
    }

    [Fact]
    public void parse_with_multiple_quotes()
    {
        assert("12\"34\"56= =\"12\"34\"56\"", [("12\"34\"56", ""), ("", "\"12\"34\"56\"")]);
        assert("a\"b\"c=====\"d\"=====", [("a\"b\"c", "====\"d\"=====")]);
    }

    [Fact]
    public void end_before_closing_quote()
    {
        assert("a\"", [("a\"", null)]);
        assert("a=\"", [("a", "\"")]);
        assert("a=\"b", [("a", "\"b")]);
    }

    [Fact]
    public void end_before_value()
    {
        assert("a=", [("a", "")]);
    }
}