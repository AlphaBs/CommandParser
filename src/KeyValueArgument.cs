namespace CommandParser;

public class KeyValueArgument
{
    public KeyValueArgument(string key, IReadOnlyCollection<string>? values)
    {
        Key = key;
        Values = values;
    }

    public string Key { get; } 
    public IReadOnlyCollection<string>? Values { get; }

    public override string ToString()
    {
        if (Values == null)
            return Key;
        else if (Values.Count == 0)
            return Key + "=";
        else if (Values.Count == 1)
            return Key + "=" + Values.First();
        else 
            return Key + " " + string.Join(" ", Values);
    }

    public override int GetHashCode()
    {
        var hashcode = Key.GetHashCode();
        if (Values != null)
        {
            foreach (var value in Values)
            {
                hashcode ^= value.GetHashCode();
            }
        }
        return hashcode;
    }

    public override bool Equals(object obj)
    {
        if (obj is KeyValueArgument arg)
        {
            if (this.Key != arg.Key)
                return false;
            
            if (this.Values == null && arg.Values == null)
            {
                return true;
            }
            else if (this.Values != null && this.Values != null)
            {
                return this.Values.SequenceEqual(arg.Values);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}

public class ArgumentBuilder
{
    private readonly Queue<KeyValueArgument> _q = new();
    private readonly List<string> _values = new();

    public string Key { get; set; } = "";
    public IEnumerable<string> Values => _values;

    public void Clear()
    {
        Key = "";
        _values.Clear();
    }

    public void AddValue(string value)
    {
        _values.Add(value);
    }

    public void Complete()
    {
        string[]? valuesCopy = null;
        if (_values.Count > 0)
        {
             valuesCopy = new string[_values.Count];
            _values.CopyTo(valuesCopy);
        }
        
        _q.Enqueue(new KeyValueArgument(Key, valuesCopy));
        Clear();
    }

    public IEnumerable<KeyValueArgument> PopArguments()
    {
        while (_q.Any())
        {
            yield return _q.Dequeue();
        }
    }
}