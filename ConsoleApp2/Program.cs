using System;

namespace TestCode
{

    class Program
    {
        private const double msWait = 1000;

        public static MessageSystem.VoidState Input;
        public static MessageSystem.VoidState Physics;
        public static MessageSystem.VoidState Render;
        private static void Main(string[] args)
        {
            SetupEvents.RegisterEvent(typeof(CreateEvent));
            SetupEvents.RegisterEvent(typeof(CreateMessage));
            SetupEvents.RegisterEvent(typeof(RegisterRender));
            SetupEvents.RegisterEvent(typeof(RegisterInput));
            SetupEvents.RegisterEvent(typeof(RegisterPhysics));
            SetupEvents.RegisterEvent<string[]>(Boop);
            SetupEvents.RegisterEvent<GameState>(Boop);
            SetupEvents.RegisterEvent<GameState>(RenderFunc);
            SetupEvents.RegisterEvent<GameState>(PhysicsFunc);
            SetupEvents.RegisterEvent<GameState>(InputFunc);
            SetupEvents.RegisterEvent<string[]>(Beep.Boop);
            Console.WriteLine("Running event Boop");
            string[] boop = {"Hi", "Hello"};
            EventManager.NotifyEvent("Boop", boop);
            Console.WriteLine("Registering message 'Bang'");
            MessageSystem.PushMessage("Bang");
            Console.WriteLine("Registering message 'Bang'");
            MessageSystem.PushMessage("Bang");
            Console.WriteLine("Registering message 'Bang'");
            MessageSystem.PushMessage("Bang");
            Console.WriteLine("Running message 'Bang' handling 2 messages");
            GameState bibbly = new GameState();
            MessageSystem.HandleMessages(bibbly, 2);
            Console.WriteLine("Running message 'Bang' handling 2 messages");
            MessageSystem.HandleMessages(bibbly, 2);
            Console.WriteLine("Starting Main Loop, press any key to break");
            while (!Console.KeyAvailable)
            {
                DateTime currTime = DateTime.Now;
                Console.WriteLine("Registering message 'Bang'");
                MessageSystem.PushMessage("Bang");
                Console.WriteLine("Registering message 'Bang'");
                MessageSystem.PushMessage("Bang");
                Console.WriteLine("Running message 'Bang' handling 2 messages");
                MessageSystem.HandleMessages(bibbly, 2);
                Input?.Invoke(bibbly);
                Physics?.Invoke(bibbly);
                Render?.Invoke(bibbly);
                while (DateTime.Now < currTime.AddMilliseconds(msWait))
                {
                    if (Console.KeyAvailable)
                    {
                        return;
                    }
                }
            }
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

        [RegisterRender("Render")]
        public static void RenderFunc(GameState state)
        {
            Console.WriteLine("    Registered Program.RenderFunc() as Render Function");
        }

        [RegisterInput("Input")]
        public static void InputFunc(GameState state)
        {
            Console.WriteLine("    Registered Program.InputFunc() as Input Function");
        }

        [RegisterPhysics("Physics")]
        public static void PhysicsFunc(GameState state)
        {
            Console.WriteLine("    Registered Program.PhysicsFunc() as Physics Function");
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