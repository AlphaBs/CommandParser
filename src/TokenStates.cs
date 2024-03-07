namespace CommandParser;

interface ITokenizerStateMachine
{
    ITokenizerStateMachine PutSpace(TokenBuilder current, char next);
    ITokenizerStateMachine PutKeyPrefix(TokenBuilder current, char next);
    ITokenizerStateMachine PutChar(TokenBuilder current, char next);
    ITokenizerStateMachine PutKeyValueSeparator(TokenBuilder current, char next);
    ITokenizerStateMachine PutQuote(TokenBuilder current, char next);
    void End(TokenBuilder current);
}

static class TokenizerStates
{
    public static readonly ITokenizerStateMachine Init = new InitState();
    public static readonly ITokenizerStateMachine Key = new KeyState();
    public static readonly ITokenizerStateMachine QuotedKey = new QuotedKeyState();
    public static readonly ITokenizerStateMachine KeyValueSeparator = new KeyValueSeparatorState();
    public static readonly ITokenizerStateMachine Value = new ValueState();
    public static readonly ITokenizerStateMachine QuotedValue = new QuotedValueState();

    class InitState : ITokenizerStateMachine
    {
        public void End(TokenBuilder current)
        {
            return;
        }

        public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutKeyPrefix(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Key;
        }

        public ITokenizerStateMachine PutKeyValueSeparator(TokenBuilder current, char next)
        {
            current.Append(next);
            current.Complete(TokenType.KeyValueSeparator);
            return TokenizerStates.KeyValueSeparator;
        }

        public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedValue;
        }

        public ITokenizerStateMachine PutSpace(TokenBuilder current, char next)
        {
            return TokenizerStates.Init;
        }
    }

    class KeyState : ITokenizerStateMachine
    {
        public void End(TokenBuilder current)
        {
            current.Complete(TokenType.Key);
        }

        public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Key;
        }

        public ITokenizerStateMachine PutKeyPrefix(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Key;
        }

        public ITokenizerStateMachine PutKeyValueSeparator(TokenBuilder current, char next)
        {
            current.Complete(TokenType.Key);
            current.Append(next);
            current.Complete(TokenType.KeyValueSeparator);
            return TokenizerStates.KeyValueSeparator;
        }

        public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedKey;
        }

        public ITokenizerStateMachine PutSpace(TokenBuilder current, char next)
        {
            current.Complete(TokenType.Key);
            return TokenizerStates.Init;
        }
    }

    class QuotedKeyState : ITokenizerStateMachine
    {
        public void End(TokenBuilder current)
        {
            current.Complete(TokenType.Key);
        }

        public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedKey;
        }

        public ITokenizerStateMachine PutKeyPrefix(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedKey;
        }

        public ITokenizerStateMachine PutKeyValueSeparator(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedKey;
        }

        public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Key;
        }

        public ITokenizerStateMachine PutSpace(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedKey;
        }
    }

    class KeyValueSeparatorState : ITokenizerStateMachine
    {
        public void End(TokenBuilder current)
        {
            return;
        }

        public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutKeyPrefix(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutKeyValueSeparator(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedValue;
        }

        public ITokenizerStateMachine PutSpace(TokenBuilder current, char next)
        {
            return TokenizerStates.Init;
        }
    }

    class ValueState : ITokenizerStateMachine
    {
        public void End(TokenBuilder current)
        {
            current.Complete(TokenType.Value);
        }

        public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutKeyPrefix(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutKeyValueSeparator(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedValue;
        }

        public ITokenizerStateMachine PutSpace(TokenBuilder current, char next)
        {
            current.Complete(TokenType.Value);
            return TokenizerStates.Init;
        }
    }

    class QuotedValueState : ITokenizerStateMachine
    {
        public void End(TokenBuilder current)
        {
            current.Complete(TokenType.Value);
        }

        public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedValue;
        }

        public ITokenizerStateMachine PutKeyPrefix(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedValue;
        }

        public ITokenizerStateMachine PutKeyValueSeparator(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedValue;
        }

        public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.Value;
        }

        public ITokenizerStateMachine PutSpace(TokenBuilder current, char next)
        {
            current.Append(next);
            return TokenizerStates.QuotedValue;
        }
    }
}