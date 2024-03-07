namespace CommandParser.Tests;

public class TokenParserTests
{
    private static void assert((TokenType, string)[] input, (string, string[]?)[] expected)
    {
        var inputTokens = input.Select(tuple => new Token(tuple.Item1, tuple.Item2));
        var expectedArgs = expected.Select(tuple => KeyValueArgument.CreateWithoutValidation(
            key: tuple.Item1, 
            values: tuple.Item2)).ToArray();
        var result = Parser.ParseTokens(inputTokens).ToArray();
        Assert.Equal(expectedArgs, result);
    }

    [Fact]
    public void parse_empty()
    {
        assert([], []);
    }

    [Fact]
    public void parse_continuous_keys()
    {
        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.Key, "b")
        ],
        [
            ("a", null),
            ("b", null)
        ]);
    }

    [Fact]
    public void parse_only_values()
    {
        assert(
        [
            (TokenType.Value, "a"),
            (TokenType.Value, "b")
        ],
        [
            ("", ["a", "b"]),
        ]);
    }

    [Fact]
    public void parse_key_value_pairs()
    {
        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.Value, "b"),
            (TokenType.Key, "c"),
            (TokenType.Value, "d")
        ],
        [
            ("a", ["b"]),
            ("c", ["d"])
        ]);
    }

    [Fact]
    public void parse_key_values()
    {
        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.Value, "b"),
            (TokenType.Value, "c"),
            (TokenType.Value, "d")
        ],
        [
            ("a", ["b", "c", "d"])
        ]);

        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.Value, "b"),
            (TokenType.Value, "c"),
            (TokenType.Key, "d"),
        ],
        [
            ("a", ["b", "c"]),
            ("d", null)
        ]);
    }

    [Fact]
    public void parse_key_sep_value()
    {
        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "b")
        ],
        [
            ("a", ["b"])
        ]);
        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "b"),
            (TokenType.Key, "c"),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "d")
        ],
        [
            ("a", ["b"]),
            ("c", ["d"])
        ]);
    }

    [Fact]
    public void parse_value_after_key_sep_value()
    {
        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "b"),
            (TokenType.Value, "c")
        ], 
        [
            ("a", ["b"]),
            ("", ["c"])
        ]);
    }

    [Fact]
    public void parse_sep_without_key()
    {
        assert(
        [
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "a")
        ],
        [
            ("", ["a"])
        ]);
    }

    [Fact]
    public void parse_sep_without_value()
    {
        assert(
        [
            (TokenType.Key, "a"),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Key, "b"),
            (TokenType.KeyValueSeparator, "="),
        ],
        [
            ("a", [""]),
            ("b", [""])
        ]);
    }

    [Fact]
    public void ignore_sep_without_keyvalue()
    {
        assert(
        [
            (TokenType.KeyValueSeparator, "=")
        ],
        [
            ("", [""])
        ]);
        assert(
        [
            (TokenType.KeyValueSeparator, "="),
            (TokenType.KeyValueSeparator, "="),
            (TokenType.KeyValueSeparator, "="),
        ],
        [
            ("", [""]),
            ("", [""]),
            ("", [""]),
        ]);
    }

    [Fact]
    public void starts_with_sep()
    {
        assert(
        [
            (TokenType.KeyValueSeparator, "="),
            (TokenType.Value, "a")
        ],
        [
            ("", ["a"])
        ]);
    }
}