namespace CommandParser;

public static class Parser
{
    public static IEnumerable<KeyValueArgument> ParseToKeyValueArguments(IEnumerable<string> args)
    {
        IParserStateMachine state = ParserStates.Init;
        KeyValueArgumentBuilder current = new();

        foreach (var arg in args)
        {
            state = state.Put(current, arg);

            foreach (var parsed in current.PopArguments())
                yield return parsed;
        }

        state.End(current);
        foreach (var parsed in current.PopArguments())
            yield return parsed;
    }

    public static IEnumerable<string> ParseToArguments(string input)
    {
        IArgumentStateMachine state = ArgumentStates.Init;
        ArgumentBuilder current = new();

        foreach (var c in input)
        {
            state = state.Put(current, c);

            foreach (var parsed in current.PopArguments())
                yield return parsed;
        }

        state.End(current);
        foreach (var parsed in current.PopArguments())
            yield return parsed;
    }
}