using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestingApplicationForEngine
{
    class BaseAttr : Attribute
    {
        public string name;

        public BaseAttr(string name)
        {
            this.name = name;
        }
    }

    [HookAttribute("event")]
    [AttributeUsage(AttributeTargets.Method)]
    class CreateEvent : BaseAttr
    {
        public CreateEvent(string name) : base(name)
        {
        }
    }

    [HookAttribute("message")]
    [AttributeUsage(AttributeTargets.Method)]
    class CreateMessage : BaseAttr
    {
        public CreateMessage(string name) : base(name)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    class HookAttribute : BaseAttr
    {
        public HookAttribute(string name) : base(name)
        {
        }
    }

    class SetupEvents
    {
        public static void RegisterEvent(Action function)
        {
            PrintTestInfo(function.GetMethodInfo());
            function();
        }

        struct StoreType
        {
            public Type type;
            public string str;

            public StoreType(Type t, string s)
            {
                type = t;
                str = s;
            }
        }

        private static List<StoreType> attributes = new List<StoreType>();

        public static void RegisterEvent(Type type)
        {
            PrintTestInfo(type.GetTypeInfo(), type);
        }

        private static void PrintTestInfo(MemberInfo t, Type type = null)
        {
            Console.WriteLine("Registration information for {0}", t);

            var attrs = t.GetCustomAttributes();

            foreach (Attribute attr in attrs)
            {
                if (attr is HookAttribute e)
                {
                    attributes.Add(new StoreType(type, e.name));
                    Console.WriteLine("   Registered {0} attribute", e.name);
                }
                else
                {
                    foreach (StoreType storedType in attributes)
                    {
                        if (attr.GetType() == storedType.type)
                        {
                            if (attr is BaseAttr f)
                            {
                                Console.WriteLine("  Registerd {0} as {1}", storedType.str, f.name);
                            }
                            else
                            {
                                Console.WriteLine("  Registerd {0}", storedType.str);
                            }
                        }
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SetupEvents.RegisterEvent(typeof(CreateEvent));
            SetupEvents.RegisterEvent(typeof(CreateMessage));
            SetupEvents.RegisterEvent(boop);
            SetupEvents.RegisterEvent(Beep.boop);
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

        [CreateMessage("Bang")]
        [CreateEvent("Boop")]
        public static void boop()
        {
            //Do Stuff
            Console.WriteLine("Registered Program.boop()");
        }
    }

    class Beep
    {
        [CreateEvent("Boop")]
        public static void boop()
        {
            //Do Stuff
            Console.WriteLine("Registered Beep.boop()");
        }
    }
}