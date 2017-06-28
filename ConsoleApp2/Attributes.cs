
using System;
using System.Reflection;

internal abstract class BaseAttr : Attribute
{
    public readonly string Name;

    protected BaseAttr(string name)
    {
        Name = name;
    }

    public abstract void Register(MemberInfo i, Attribute j);
}

[Hook("event")]
[AttributeUsage(AttributeTargets.Method)]
internal class CreateEvent : BaseAttr
{
    public CreateEvent(string name) : base(name)
    {
    }

    public override void Register(MemberInfo i, Attribute j)
    {
        var attr = j as CreateEvent;
        if (attr != null) Console.WriteLine("  Registerd event as {0}", attr.Name);
        var functionInfo = i as MethodInfo;
        if (functionInfo != null)
        {
            var function = functionInfo.CreateDelegate(typeof(EventManager.VoidString));
            if (function is EventManager.VoidString vs)
            {
                if (attr != null) EventManager.SubscribeToEvent(attr.Name, vs);
            }
        }
    }
}

[Hook("message")]
[AttributeUsage(AttributeTargets.Method)]
internal class CreateMessage : BaseAttr
{
    public CreateMessage(string name) : base(name)
    {
    }

    public override void Register(MemberInfo i, Attribute j)
    {
        var attr = j as CreateMessage;
        if (attr != null) Console.WriteLine("  Registerd message as {0}", attr.Name);
        var functionInfo = i as MethodInfo;
        if (functionInfo != null)
        {
            var function = functionInfo.CreateDelegate(typeof(MessageSystem.VoidState));
            if (function is MessageSystem.VoidState vs)
            {
                if(attr != null) MessageSystem.registerMessage(attr.Name, vs);
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Class)]
internal class HookAttribute : BaseAttr
{
    public HookAttribute(string name) : base(name)
    {
    }

    public override void Register(MemberInfo i, Attribute j)
    {
        throw new NotImplementedException();
    }
}
