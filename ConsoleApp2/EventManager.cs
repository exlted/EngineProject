/* ------------------------------------------------------------------
 * THIS CODE IS LICENSED TO PHYSLIGHT DEVELOPMENTS BY BENJAMIN J KING
 * ------------------------------------------------------------------
 * 
 * Full ownership of this code is retained by Benjamin J King and no rights
 * beyond usage within Physlight Game Project 1 (The Paradox Project) is
 * inherantly provided.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void VoidObject(object[] objects);
public struct Command
{
	public string command;
	public string help;
	public string syntax;
	public VoidObject function;
}


static class ConsoleManager
{
	private static Dictionary<string, Command> Commands = new Dictionary<String, Command>();

	public static void RegisterCommand(Command newComm)
	{
		Commands.Add(newComm.command, newComm);
	}

	public static void UpdateCommand(Command updatedComm)
	{
		Commands[updatedComm.command] = updatedComm;
	}
}

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommand : Attribute
{

	private string command;
	private string help;
	private string syntax;
	private VoidObject function;

	public ConsoleCommand(string comm, string hel, string synt, VoidObject func)
	{
		command = comm;
		help = hel;
		syntax = synt;
		function = func;

		Command temp;
		temp.command = comm;
		temp.help = hel;
		temp.syntax = synt;
		temp.function = func;
		ConsoleManager.RegisterCommand(temp);
	}

	public virtual string Command
	{
		get { return command; }
	}

	public virtual string Help
	{
		get { return help; }
	}

	public virtual string Syntax
	{
		get { return syntax; }
	}

	public virtual VoidObject Function
	{
		get { return function; }
		set
		{
			function = value;
			Command temp;
			temp.command = command;
			temp.help = help;
			temp.syntax = syntax;
			temp.function = function;
			ConsoleManager.UpdateCommand(temp);
		}
	}
}


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
		else
        if (StringEvents.ContainsKey(eventName))
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