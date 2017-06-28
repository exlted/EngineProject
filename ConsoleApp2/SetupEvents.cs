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
}