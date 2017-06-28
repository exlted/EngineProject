using System.Collections.Generic;

static class EventManager
{
    public delegate void VoidString(string[] strings);

    private static Dictionary<string, VoidString> StringEvents = new Dictionary<string, VoidString>();

	private static VoidString RecieveAll;

	/// <summary>
	/// Subscribes the specified funtion to the specified event
	/// </summary>
	/// <param name="eventName">The event that will be subscribed to</param>
	/// <param name="functionsToRun">The function that will be subscribed to the event</param>
	/// <param name="recieveFromAll">WARNING: DANGEROUS PARAMETER: Use only if you absolutely need to subscribe to every single event</param>
    public static void SubscribeToEvent(string eventName, VoidString functionsToRun, bool recieveFromAll = false)
    {
	    if (!recieveFromAll)
	    {
			if (StringEvents.ContainsKey(eventName))
			{
				StringEvents[eventName] += functionsToRun;
			}
			else
			{
				StringEvents.Add(eventName, functionsToRun);
			}
		}
		else if (StringEvents.ContainsKey(eventName))
        {
            StringEvents[eventName] += functionsToRun;
        }
        else
        {
            StringEvents.Add(eventName, functionsToRun);
        }
	    if (recieveFromAll)
	    {
		    RecieveAll += functionsToRun;
	    }
    }
	/// <summary>
	/// Unsubscribes the specified function from the specified event
	/// </summary>
	/// <param name="eventName">The event that will be unsubscribed from</param>
	/// <param name="functionsToRemove">The function that will be unsubscribed from the event</param>
    public static void UnsubscribeFromEvent(string eventName, VoidString functionsToRemove)
    {
        if (StringEvents.ContainsKey(eventName))
        {
            StringEvents[eventName] -= functionsToRemove;
        }
	    RecieveAll -= functionsToRemove;
    }

	/// <summary>
	/// Passes the specified event data to the specified event
	/// </summary>
	/// <param name="eventName">The event which will be notified</param>
	/// <param name="eventData">The data which will be passed to the event</param>
	/// <param name="sendToAll">WARNING: DANGEROUS PARAMETER: only use when you absolutely need to send arguments to EVERY event</param>
    public static void NotifyEvent(string eventName, string[] eventData, bool sendToAll = false)
    {
	    if (!sendToAll)
	    {
		    if (RecieveAll != null) RecieveAll(eventData);
		    if (StringEvents.ContainsKey(eventName))
		    {
			    StringEvents[eventName](eventData);
		    }
	    }
	    else
		{
			if (RecieveAll != null) RecieveAll(eventData);
			foreach (KeyValuePair<string, VoidString> e in StringEvents)
			{
				e.Value(eventData);
			}
		}
    }
}