namespace CommandParser;

interface IParserStateMachine
{
    IParserStateMachine PutKey(ArgumentBuilder current, string next);
    IParserStateMachine PutValue(ArgumentBuilder current, string next);
    IParserStateMachine PutKeyValueSeparator(ArgumentBuilder current, string next);
    void End(ArgumentBuilder current);
}

static class ParserStates
{
    public readonly static IParserStateMachine Init = new InitState();
    public readonly static IParserStateMachine Key = new KeyState();
    public readonly static IParserStateMachine KeyValueSeparator = new KeyValueSeparatorState();
    public readonly static IParserStateMachine Value = new ValueState();
    
    class InitState : IParserStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            
        }

        public IParserStateMachine PutKey(ArgumentBuilder current, string next)
        {
            current.Key = next;
            return ParserStates.Key;
        }

        public IParserStateMachine PutKeyValueSeparator(ArgumentBuilder current, string next)
        {
            return ParserStates.KeyValueSeparator;
        }

        public IParserStateMachine PutValue(ArgumentBuilder current, string next)
        {
            current.AddValue(next);
            return ParserStates.Value;
        }
    }

    class KeyState : IParserStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            current.Complete();
        }

        public IParserStateMachine PutKey(ArgumentBuilder current, string next)
        {
            current.Complete();
            current.Key = next;
            return ParserStates.Key;
        }

        public IParserStateMachine PutKeyValueSeparator(ArgumentBuilder current, string next)
        {
            return ParserStates.KeyValueSeparator;
        }

        public IParserStateMachine PutValue(ArgumentBuilder current, string next)
        {
            current.AddValue(next);
            return ParserStates.Value;
        }
    }

    class KeyValueSeparatorState : IParserStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            current.AddValue("");
            current.Complete();
        }

        public IParserStateMachine PutKey(ArgumentBuilder current, string next)
        {
            current.AddValue("");
            current.Complete();
            
            current.Key = next;
            return ParserStates.Key;
        }

        public IParserStateMachine PutKeyValueSeparator(ArgumentBuilder current, string next)
        {
            current.AddValue("");
            current.Complete();

            current.Key = "";
            return ParserStates.KeyValueSeparator;
        }

        public IParserStateMachine PutValue(ArgumentBuilder current, string next)
        {
            current.AddValue(next);
            current.Complete();
            return ParserStates.Init;
        }
    }

    class ValueState : IParserStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            current.Complete();
        }

        public IParserStateMachine PutKey(ArgumentBuilder current, string next)
        {
            current.Complete();
            current.Key = next;
            return ParserStates.Key;
        }

        public IParserStateMachine PutKeyValueSeparator(ArgumentBuilder current, string next)
        {
            current.Complete();
            return ParserStates.KeyValueSeparator;
        }

        public IParserStateMachine PutValue(ArgumentBuilder current, string next)
        {
            current.AddValue(next);
            return ParserStates.Value;
        }
    }
}