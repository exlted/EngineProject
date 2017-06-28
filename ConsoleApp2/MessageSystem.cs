using System.Collections.Generic;

class GameState
{
    public float DT;
    public double GameTime;

    public Dictionary<string, object> State;
}

static class MessageSystem
{
    public delegate void VoidState(GameState state);

    private static Queue<string> messagesToHandle = new Queue<string>();

    private static Dictionary<string, VoidState> registeredMessages = new Dictionary<string, VoidState>();

    public static void RegisterMessage(string message, VoidState func)
    {
        if (registeredMessages.ContainsKey(message))
        {
            registeredMessages[message] += func;
        }
        else
        {
            registeredMessages.Add(message, func);
        }
    }

    public static void PushMessage(string message)
    {
        messagesToHandle.Enqueue(message);
    }

    public static void HandleMessages(GameState state, int numMessages = 10)
    {
        while (numMessages > 0 && messagesToHandle.Count > 0)
        {
            numMessages--;
            var message = messagesToHandle.Dequeue();
            registeredMessages[message](state);
        }
    }
}