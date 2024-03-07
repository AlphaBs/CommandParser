namespace CommandParser.Tests;

public class TokenizerTests
{
    private static void assert(string input, (TokenType, string)[] expected)
    {
        var expectedTokens = expected.Select(tuple => new Token(tuple.Item1, tuple.Item2));
        var result = Parser.Tokenize(input);
        Assert.Equal(expectedTokens, result);
    }

    [Fact]
    public void parse_empty()
    {
        assert("", []);
        assert("   \n \r \t   ", []);
    }

    [Fact]
    public void parse_keys()
    {
        assert("-a", [(TokenType.Key, "-a")]);
        assert("-a --b - --", 
        [
            (TokenType.Key, "-a"), 
            (TokenType.Key, "--b"),
            (TokenType.Key, "-"),
            (TokenType.Key, "--")
        ]);
    }

    [Fact]
    public void parse_values()
    {
        assert("a b", 
        [
            (TokenType.Value, "a"),
            (TokenType.Value, "b")
        ]);
        assert("a=b", 
        [
            (TokenType.Value, "a=b")
        ]);
    }

    [Fact]
    public void ignore_spaces()
    {
        assert(" -a  \t  -b \r\n\n   c  ", [(TokenType.Key, "-a"), (TokenType.Key, "-b"), (TokenType.Value, "c")]);
    }

    [Fact]
    public void parse_key_value_separator()
    {
        assert("=", [(TokenType.KeyValueSeparator, "=")]);
    }

    [Fact]
    public void parse_key_value_pair()
    {
        assert("-a=b", 
        [
            (TokenType.Key, "-a"), 
            (TokenType.KeyValueSeparator, "="), 
            (TokenType.Value, "b")
        ]);
        assert("=a", 
        [
            (TokenType.KeyValueSeparator, "="), 
            (TokenType.Value, "a")
        ]);
        assert("-\"\"=a", 
        [
            (TokenType.Key, "-\"\""),
            (TokenType.KeyValueSeparator, "="), 
            (TokenType.Value, "a")
        ]);
        assert("-b=", 
        [
            (TokenType.Key, "-b"), 
            (TokenType.KeyValueSeparator, "=")
        ]);
        assert("-\"\"=\"\"",
        [
            (TokenType.Key, "-\"\""),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "\"\"")
        ]);
        assert("-b=\"\"", 
        [
            (TokenType.Key, "-b"), 
            (TokenType.KeyValueSeparator, "="), 
            (TokenType.Value, "\"\"")
        ]);
        assert("-a=====b", 
        [
            (TokenType.Key, "-a"), 
            (TokenType.KeyValueSeparator, "="), 
            (TokenType.Value, "====b")
        ]);
    }

    [Fact]
    public void ignore_spaces_in_quotes()
    {
        assert("\"a b\"", [(TokenType.Value, "\"a b\"")]);
        assert("-a=\"b c\"", 
        [
            (TokenType.Key, "-a"), 
            (TokenType.KeyValueSeparator, "="), 
            (TokenType.Value, "\"b c\"")
        ]);
        assert("-\"a b\"=c", 
        [
            (TokenType.Key, "-\"a b\""),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "c")
        ]);
        assert("-\"a b\"=\"c d\"", 
        [
            (TokenType.Key, "-\"a b\""),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "\"c d\"")
        ]);
    }

    [Fact]
    public void ignore_separator_in_quotes()
    {
        assert("\"a=b\"", [(TokenType.Value, "\"a=b\"")]);
        assert("a\"=\"b", [(TokenType.Value, "a\"=\"b")]);
    }

    [Fact]
    public void ignore_keyprefix_in_quotes()
    {
        assert("\"--a=b\"", [(TokenType.Value, "\"--a=b\"")]);
    }

    [Fact]
    public void parse_with_multiple_quotes()
    {
        assert("-12\"34\"56", [(TokenType.Key, "-12\"34\"56")]);
        assert("12\"34\"56", [(TokenType.Value, "12\"34\"56")]);
        assert("-=====12\"34\"56", 
        [
            (TokenType.Key, "-"), 
            (TokenType.KeyValueSeparator, "="), 
            (TokenType.Value, "====12\"34\"56")
        ]);
    }

    [Fact]
    public void end_before_closing_quote()
    {
        assert("-a\"", [(TokenType.Key, "-a\"")]);
        assert("-a=\"", 
        [
            (TokenType.Key, "-a"),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "\"")
        ]);
        assert("-a=b\"", 
        [
            (TokenType.Key, "-a"),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "b\"")
        ]);
        assert("ab\"", [(TokenType.Value, "ab\"")]);
    }
}