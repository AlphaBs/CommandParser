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
        ITokenizerStateMachine state = TokenizerStates.Init;
        TokenBuilder current = new();

        foreach (char next in input)
        {
            if (char.IsWhiteSpace(next))
                state = state.PutSpace(current, next);
            else if (next == '=')
                state = state.PutKeyValueSeparator(current, next);
            else if (next == '"')
                state = state.PutQuote(current, next);
            else 
                state = state.PutChar(current, next);

            foreach (var token in current.PopTokens())
                yield return token;
        }

        state.End(current);
        foreach (var token in current.PopTokens())
                yield return token;
    }

    public static IEnumerable<KeyValueArgument> ParseTokens(IEnumerable<Token> tokens)
    {
        IParserStateMachine state = ParserStates.Init;
        ArgumentBuilder current = new();

        foreach (var next in tokens)
        {
            switch (next.Type)
            {
                case TokenType.Key:
                    state = state.PutKey(current, next.Value);
                    break;
                case TokenType.Value:
                    state = state.PutValue(current, next.Value);
                    break;
                case TokenType.KeyValueSeparator:
                    state = state.PutKeyValueSeparator(current, next.Value);
                    break;
            }

            foreach (var arg in current.PopArguments())
                yield return arg;
        }

        state.End(current);
        foreach (var arg in current.PopArguments())
            yield return arg;
    }
}