using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestCode
{
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

    class SetupEvents
    {
        public static void RegisterEvent(Action function)
        {
            PrintTestInfo(function.GetMethodInfo());
        }

        public static void RegisterEvent<T>(Action<T> function)
        {
            PrintTestInfo(function.GetMethodInfo());
        }

        public static void RegisterEvent<T, TR>(Func<T, TR> function)
        {
            PrintTestInfo(function.GetMethodInfo());
        }

        struct StoreType
        {
            public readonly Type Type;
            public readonly string Str;

            public StoreType(Type t, string s)
            {
                Type = t;
                Str = s;
            }
        }

        private static readonly List<StoreType> Attributes = new List<StoreType>();

        public static void RegisterEvent(Type type)
        {
            PrintTestInfo(type.GetTypeInfo());
        }

        private static void PrintTestInfo(MemberInfo t)
        {
            if(t is MethodInfo mi)
            {
                Console.WriteLine("Registration information for {0}.{1}", mi.DeclaringType, mi.Name);
            }
            else if(t is TypeInfo ti)
            {
                Console.WriteLine("Registration information for {0}", ti);
            }
            var attrs = t.GetCustomAttributes();

            foreach (var attr in attrs)
            {
                if (attr is HookAttribute e)
                {
                    var x = t as TypeInfo;
                    if (x != null) Attributes.Add(new StoreType(x.AsType(), e.Name));
                    Console.WriteLine("   Registered {0} attribute", e.Name);
                }
                else
                {
                    foreach (var storedType in Attributes)
                    {
                        if (attr.GetType() != storedType.Type) continue;
                        if (attr is BaseAttr f)
                        {
                            f.Register(t, f);
                        }
                        else
                        {
                            Console.WriteLine("  Registerd {0}", storedType.Str);
                        }
                    }
                }
            }
        }
    }

    class Program
    {
        private static void Main(string[] args)
        {
            SetupEvents.RegisterEvent(typeof(CreateEvent));
            SetupEvents.RegisterEvent(typeof(CreateMessage));
            SetupEvents.RegisterEvent<string[]>(Boop);
            SetupEvents.RegisterEvent<string[]>(Beep.Boop);
            Console.WriteLine("Running event Boop");
            string[] boop = {"Hi", "Hello"};
            EventManager.NotifyEvent("Boop", boop);
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

        [CreateMessage("Bang")]
        [CreateEvent("Boop")]
        public static void Boop(string[] blah)
        {
            //Do Stuff
            Console.WriteLine("    Registered Program.Boop()");
        }
    }

    class Beep
    {
        [CreateEvent("Boop")]
        public static void Boop(string[] blah)
        {
            //Do Stuff
            Console.WriteLine("    Registered Beep.Boop()");
        }
    }
}