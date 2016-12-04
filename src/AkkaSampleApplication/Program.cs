using Akka.Actor;
using CodingMilitia.AkkaSampleApplication.Actors;
using CodingMilitia.AkkaSampleApplication.Messages;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodingMilitia.AkkaSampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncContext.Run(MainAsync);
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Starting application");
            using (var system = ActorSystem.Create("system"))
            {
                var consoleWriter = system.ActorOf<ConsoleWriterActor>();
                var stuffDistributor = system.ActorOf(Props.Create(() => new StuffDistributionActor(consoleWriter)));

                for (int i = 0; i < 5; ++i)
                {
                    stuffDistributor.Tell(new StuffWrapper { StuffHandlerId = i, Stuff = new Stuff { Text = $"Message to stuff handler {i}" } });
                }

                for (int i = 0; i < 10; ++i)
                {
                    stuffDistributor.Tell(new StuffWrapper { StuffHandlerId = i, Stuff = new Stuff { Text = $"Message to stuff handler {i}" } });
                }

                var askResults = new List<dynamic>();
                for (int i = 0; i < 10; ++i)
                {
                    var result = stuffDistributor.Ask<StuffCountResponse>(new StuffCountRequestWrapper { StuffHandlerId = i, StuffCountRequest = new StuffCountRequest() });
                    askResults.Add(new { HandlerId = i, StuffCountTask = result });
                }
                foreach (var result in askResults)
                {
                    Console.WriteLine($"Stuff handler {result.HandlerId} received {(await result.StuffCountTask).Count} messages");
                }
            }
            Console.WriteLine("Exiting application");
        }
    }
}
