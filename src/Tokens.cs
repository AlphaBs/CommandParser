using System.Text;

namespace CommandParser;

public enum TokenType { Key, Value }
public record Token(TokenType Type, string Value);

public class TokenBuilder
{
    private readonly StringBuilder sb = new();

    public bool IsCompleted { get; private set; }
    public TokenType Type { get; set; }

    public void Append(char c) => sb.Append(c);

    public void Clear() 
    {
        sb.Clear();
        IsCompleted = false;
    }

    public void CompleteKey()
    {
        Type = TokenType.Key;
        IsCompleted = true;
    }

    public void CompleteValue()
    {
        Type = TokenType.Value;
        IsCompleted = true;
    }

    public bool TryGetToken(out Token? token) 
    {
        if (IsCompleted)
        {
            token = new Token(Type, sb.ToString());
            IsCompleted = false;
            sb.Clear();
            return true;
        }
        else
        {
            token = null;
            return false;
        }
    }
}
