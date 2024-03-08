using System.Text;

namespace CommandParser;

public enum TokenType { Key, Value, KeyValueSeparator }
public record struct Token(TokenType Type, string Value);

public class TokenBuilder
{
    private readonly Queue<Token> _q = new();
    private readonly StringBuilder sb = new();

    public void Append(char c) => sb.Append(c);
    public void Clear() 
    {
        sb.Clear();
    }

    public void Complete(TokenType type)
    {
        _q.Enqueue(new Token(type, sb.ToString()));
        Clear();
    }

    public IEnumerable<Token> PopTokens()
    {
        while (_q.Any())
        {
            yield return _q.Dequeue();
        }
    }
}
