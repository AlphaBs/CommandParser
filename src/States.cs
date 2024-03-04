namespace CommandParser;

static class TokenizerStates
{
    public static readonly ITokenizerStateMachine Init = new InitState();
    public static readonly ITokenizerStateMachine Key = new KeyState();
    public static readonly ITokenizerStateMachine QuotedKey = new QuotedKeyState();
    public static readonly ITokenizerStateMachine Value = new ValueState();
    public static readonly ITokenizerStateMachine QuotedValue = new QuotedValueState();
}

public interface ITokenizerStateMachine
{
    ITokenizerStateMachine PutSpace(TokenBuilder current, char next);
    ITokenizerStateMachine PutChar(TokenBuilder current, char next);
    ITokenizerStateMachine PutKeyValueDelimiter(TokenBuilder current, char next);
    ITokenizerStateMachine PutQuote(TokenBuilder current, char next);
    void End(TokenBuilder current);
}

class InitState : ITokenizerStateMachine
{
    public void End(TokenBuilder current)
    {
        return;
    }

    public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
    {
        current.Append(next);
        return TokenizerStates.Key;
    }

    public ITokenizerStateMachine PutKeyValueDelimiter(TokenBuilder current, char next)
    {
        current.CompleteKey();
        return TokenizerStates.Value;
    }

    public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
    {
        current.Append(next);
        return TokenizerStates.QuotedKey;
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
        current.CompleteKey();
    }

    public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
    {
        current.Append(next);
        return TokenizerStates.Key;
    }

    public ITokenizerStateMachine PutKeyValueDelimiter(TokenBuilder current, char next)
    {
        current.CompleteKey();
        return TokenizerStates.Value;
    }

    public ITokenizerStateMachine PutQuote(TokenBuilder current, char next)
    {
        current.Append(next);
        return TokenizerStates.QuotedKey;
    }

    public ITokenizerStateMachine PutSpace(TokenBuilder current, char next)
    {
        current.CompleteKey();
        return TokenizerStates.Init;
    }
}

class QuotedKeyState : ITokenizerStateMachine
{
    public void End(TokenBuilder current)
    {
        current.CompleteKey();
    }

    public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
    {
        current.Append(next);
        return TokenizerStates.QuotedKey;
    }

    public ITokenizerStateMachine PutKeyValueDelimiter(TokenBuilder current, char next)
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

class ValueState : ITokenizerStateMachine
{
    public void End(TokenBuilder current)
    {
        current.CompleteValue();
    }

    public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
    {
        current.Append(next);
        return TokenizerStates.Value;
    }

    public ITokenizerStateMachine PutKeyValueDelimiter(TokenBuilder current, char next)
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
        current.CompleteValue();
        return TokenizerStates.Init;
    }
}

class QuotedValueState : ITokenizerStateMachine
{
    public void End(TokenBuilder current)
    {
        current.CompleteValue();
    }

    public ITokenizerStateMachine PutChar(TokenBuilder current, char next)
    {
        current.Append(next);
        return TokenizerStates.QuotedValue;
    }

    public ITokenizerStateMachine PutKeyValueDelimiter(TokenBuilder current, char next)
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