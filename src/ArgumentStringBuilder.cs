using System.Text;

namespace CommandParser;

public class ArgumentStringBuilder
{
    private readonly StringBuilder _sb = new();
    private List<KeyValueArgument> _arguments = new();

    public IEnumerable<KeyValueArgument> Arguments => _arguments;

    public void Add(string args)
    {
        if (string.IsNullOrEmpty(args))
            return;

        var parsedArgs = Parser.ParseArguments(args);
        foreach (var a in parsedArgs)
        {
            _arguments.Add(a);
        }

        _sb.Append(args);
        _sb.Append(" ");
    }

    public void Add(KeyValueArgument arg)
    {
        _arguments.Add(arg);

        var argStr = arg.ToString();
        if (!string.IsNullOrEmpty(argStr))
        {
            _sb.Append(arg.ToString());
            _sb.Append(" ");
        }
    }

    public bool ContainsKey(string key)
    {
        return _arguments.Any(arg => arg.Key == key);
    }

    public IEnumerable<KeyValueArgument> Find(string key)
    {
        return _arguments.Where(arg => arg.Key == key);
    }

    public string Build()
    {
        if (_sb.Length > 0)
        {
            // Add 메서드는 항상 _sb 끝에 공백 문자를 추가하기 때문에,
            // 마지막으로 추가된 공백 문자를 지우고 문자열을 만든다.
            // 예를 들어 Add("a"); Add("b"); Add("c") 를 하면 _sb 안에는
            // "a b c " 가 들어있기에 마지막 공백을 지우고 "a b c" 를 반환한다.

            return _sb.ToString(0, _sb.Length - 1);
        }
        else
        {
            return "";
        }
    }
}