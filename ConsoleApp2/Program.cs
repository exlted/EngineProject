using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestCode
{
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
            SetupEvents.RegisterEvent<GameState>(Boop);
            SetupEvents.RegisterEvent<string[]>(Beep.Boop);
            Console.WriteLine("Running event Boop");
            string[] boop = {"Hi", "Hello"};
            EventManager.NotifyEvent("Boop", boop);
            Console.WriteLine("Registering message 'Bang'");
            MessageSystem.pushMessage("Bang");
            Console.WriteLine("Registering message 'Bang'");
            MessageSystem.pushMessage("Bang");
            Console.WriteLine("Registering message 'Bang'");
            MessageSystem.pushMessage("Bang");
            Console.WriteLine("Running message 'Bang' handling 2 messages");
            GameState bibbly = new GameState();
            MessageSystem.HandleMessages(bibbly, 2);
            Console.WriteLine("Running message 'Bang' handling 2 messages");
            MessageSystem.HandleMessages(bibbly, 2);
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

        [CreateEvent("Boop")]
        public static void Boop(string[] blah)
        {
            //Do Stuff
            Console.WriteLine("    Registered Program.Boop() as Event");
        }

        [CreateMessage("Bang")]
        public static void Boop(GameState state)
        {
            Console.WriteLine("    Registered Program.Boop() as Message");
        }
    }

    class Beep
    {
        [CreateEvent("Boop")]
        public static void Boop(string[] blah)
        {
            //Do Stuff
            Console.WriteLine("    Registered Beep.Boop() as Event");
        }
    }
}