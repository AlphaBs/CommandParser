namespace CommandParser;

interface IArgumentStateMachine
{
    IArgumentStateMachine Put(ArgumentBuilder current, string next);
    void End(ArgumentBuilder current);
}

static class ArgumentStates
{
    public readonly static IArgumentStateMachine Init = new InitState();
    public readonly static IArgumentStateMachine Key = new KeyState();

    class InitState : IArgumentStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            
        }

        public IArgumentStateMachine Put(ArgumentBuilder current, string next)
        {
            if (next.StartsWith("-"))
            {
                var separatorIndex = next.IndexOf("=");
                if (separatorIndex >= 0) // key=value
                {
                    var key = next.Substring(0, separatorIndex);
                    var value = (separatorIndex < next.Length - 1) 
                        ? next.Substring(separatorIndex + 1, next.Length - separatorIndex - 1)
                        : "";
                    
                    current.Key = key;
                    current.AddValue(value);
                    current.Complete();
                    return ArgumentStates.Init;
                }
                else // key
                {
                    current.Key = next;
                    return ArgumentStates.Key;
                }
            }
            else if (next == "=") // =
            {
                current.Key = "";
                current.AddValue("");
                current.Complete();
                return ArgumentStates.Init;
            }
            else if (next.StartsWith("=")) // =value
            {
                current.Key = "";
                current.AddValue(next.Substring(1));
                return ArgumentStates.Key;
            }
            else // value
            {
                current.Key = "";
                current.AddValue(next);
                return ArgumentStates.Key;
            }
        }
    }

    class KeyState : IArgumentStateMachine
    {
        public void End(ArgumentBuilder current)
        {
            current.Complete();
        }

        public IArgumentStateMachine Put(ArgumentBuilder current, string next)
        {
            if (next.StartsWith("-"))
            {
                current.Complete();

                var separatorIndex = next.IndexOf("=");
                if (separatorIndex >= 0) // key=value
                {
                    var key = next.Substring(0, separatorIndex);
                    var value = (separatorIndex < next.Length - 1) 
                        ? next.Substring(separatorIndex + 1, next.Length - separatorIndex - 1)
                        : "";
                    
                    current.Key = key;
                    current.AddValue(value);
                    current.Complete();
                    return ArgumentStates.Init;
                }
                else // key
                {
                    current.Key = next;
                    return ArgumentStates.Key;
                }
            }
            else // everything except key
            {
                current.AddValue(next);
                return ArgumentStates.Key;
            }
        }
    }
}