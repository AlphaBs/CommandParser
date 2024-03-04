namespace CommandParser;

public static class Parser
{
    public static IEnumerable<KeyValueArgument> ParseArguments(IEnumerable<char> input)
    {
        var tokens = Tokenize(input);
        var args = ParseTokens(tokens);
        foreach (var arg in args)
        {
            yield return arg;
        }
    }

    public static IEnumerable<Token> Tokenize(IEnumerable<char> input)
    {
        ITokenizerStateMachine state = new InitState();
        Token? token;
        TokenBuilder current = new();

        foreach (char c in input)
        {
            if (char.IsWhiteSpace(c))
                state = state.PutSpace(current, c);
            else if (c == '=')
                state = state.PutKeyValueDelimiter(current, c);
            else if (c == '"')
                state = state.PutQuote(current, c);
            else 
                state = state.PutChar(current, c);

            if (current.TryGetToken(out token))
                yield return token!;
        }
        state.End(current);
        if (current.TryGetToken(out token))
            yield return token!;
    }

    public static IEnumerable<KeyValueArgument> ParseTokens(IEnumerable<Token> tokens)
    {
        Token? lastKey = null;
        foreach (var current in tokens)
        {
            if (lastKey == null && current.Type == TokenType.Key)
            {
                lastKey = current;
            }
            else if (lastKey == null && current.Type == TokenType.Value)
            {
                yield return new KeyValueArgument("", current.Value);
            }
            else if (lastKey != null && current.Type == TokenType.Key)
            {
                yield return new KeyValueArgument(lastKey.Value, null);
                lastKey = current;
            }
            else if (lastKey != null && current.Type == TokenType.Value)
            {
                yield return new KeyValueArgument(lastKey.Value, current.Value);
                lastKey = null;
            }
        }

        if (lastKey != null)
            yield return new KeyValueArgument(lastKey.Value, null);
    }
}