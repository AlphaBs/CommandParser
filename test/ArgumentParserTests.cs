namespace CommandParser.Tests;

public class ArgumentParserTests
{
    public void parse_empty()
    {
        Assert.Empty(Parser.ParseToArguments(""));
        Assert.Empty(Parser.ParseToArguments("  \n   "));
    }

    public void parse_single_word()
    {
        Assert.Equal(["word"], Parser.ParseToArguments("word"));
    }

    public void parse_multiple_word()
    {
        Assert.Equal(["aa", "bb", "cc"], Parser.ParseToArguments("aa bb cc"));
    }

    public void ignore_spaces()
    {
        Assert.Equal(["a", "b", "c"], Parser.ParseToArguments(" a \t\r b\n c "));
    }

    public void escape_with_quotes()
    {
        Assert.Equal(
            ["-key=1 2 3", "word", "-1 + 3 = 2"], 
            Parser.ParseToArguments("\"-key=1 2 3\" word \"-1 + 3 = 2\""));
    }

    public void quotes_in_word()
    {
        Assert.Equal(
            ["-key=1 2 3"], 
            Parser.ParseToArguments("-key=\"1 2 3\""));

        Assert.Equal(
            ["123456", "ab cd"], 
            Parser.ParseToArguments("12\"34\"56 a\"b c\"d"));
    }
}