using System.Text;

namespace CommandParser;

interface IArgumentStateMachine
{
    void End(ArgumentBuilder current);
    IArgumentStateMachine Put(ArgumentBuilder current, char next);
}

static class ArgumentStates
{
    public readonly static IArgumentStateMachine Init = new InitState();
    public readonly static IArgumentStateMachine Quoted = new QuotedState();

    class InitState : IArgumentStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            current.Complete();
        }

        public IArgumentStateMachine Put(ArgumentBuilder current, char next)
        {
            if (char.IsWhiteSpace(next))
            {
                current.Complete();
                return ArgumentStates.Init;
            }
            else if (next == '"')
            {
                return ArgumentStates.Quoted;
            }
            else
            {
                current.Append(next);
                return ArgumentStates.Init;
            }
        }
    }

    class QuotedState : IArgumentStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            current.Complete();
        }

        public IArgumentStateMachine Put(ArgumentBuilder current, char next)
        {
            if (next == '"')
            {
                return ArgumentStates.Init;
            }
            else
            {
                current.Append(next);
                return ArgumentStates.Quoted;
            }
        }
    }
}

class ArgumentBuilder
{
    private readonly Queue<string> _q = new();
    private readonly StringBuilder _sb = new();

    public void Append(char c) => _sb.Append(c);

    public void Complete()
    {
        _q.Enqueue(_sb.ToString());
        _sb.Clear();
    }

    public IEnumerable<string> PopArguments()
    {
        while (_q.Any())
            yield return _q.Dequeue();
    }
}