
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
                if(attr != null) MessageSystem.RegisterMessage(attr.Name, vs);
            }
        }
    }
}

[Hook("Render")]
[AttributeUsage(AttributeTargets.Method)]
internal class RegisterRender : BaseAttr
{
    public RegisterRender(string name) : base(name)
    {
    }

    public override void Register(MemberInfo i, Attribute j)
    {
        var attr = j as RegisterRender;
        if (attr != null) Console.WriteLine("  Registerd Render Function as {0}", attr.Name);
        var functionInfo = i as MethodInfo;
        if (functionInfo != null)
        {
            var function = functionInfo.CreateDelegate(typeof(MessageSystem.VoidState));
            if (function is MessageSystem.VoidState vs)
            {
                if (TestCode.Program.Render != null)
                {
                    TestCode.Program.Render += vs;
                }
                else
                {
                    TestCode.Program.Render = vs;
                }
            }
        }
    }
}

[Hook("Input")]
[AttributeUsage(AttributeTargets.Method)]
internal class RegisterInput : BaseAttr
{
    public RegisterInput(string name) : base(name)
    {
    }

    public override void Register(MemberInfo i, Attribute j)
    {
        var attr = j as RegisterInput;
        if (attr != null) Console.WriteLine("  Registerd Input Function as {0}", attr.Name);
        var functionInfo = i as MethodInfo;
        if (functionInfo != null)
        {
            var function = functionInfo.CreateDelegate(typeof(MessageSystem.VoidState));
            if (function is MessageSystem.VoidState vs)
            {
                if (attr != null)
                {
                    if (TestCode.Program.Input != null)
                    {
                        TestCode.Program.Input += vs;
                    }
                    else
                    {
                        TestCode.Program.Input = vs;
                    }
                }
            }
        }
    }
}

[Hook("Physics")]
[AttributeUsage(AttributeTargets.Method)]
internal class RegisterPhysics : BaseAttr
{
    public RegisterPhysics(string name) : base(name)
    {
    }

    public override void Register(MemberInfo i, Attribute j)
    {
        var attr = j as RegisterPhysics;
        if (attr != null) Console.WriteLine("  Registerd Physics Function as {0}", attr.Name);
        var functionInfo = i as MethodInfo;
        if (functionInfo != null)
        {
            var function = functionInfo.CreateDelegate(typeof(MessageSystem.VoidState));
            if (function is MessageSystem.VoidState vs)
            {
                if (TestCode.Program.Physics != null)
                {
                    TestCode.Program.Physics += vs;
                }
                else
                {
                    TestCode.Program.Physics = vs;
                }
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
