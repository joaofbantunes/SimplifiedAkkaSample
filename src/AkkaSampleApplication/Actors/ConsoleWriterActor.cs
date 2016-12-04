using Akka.Actor;
using CodingMilitia.AkkaSampleApplication.Messages;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.AkkaSampleApplication.Actors
{
    public class ConsoleWriterActor : ReceiveActor
    {
        public ConsoleWriterActor()
        {
            ReceiveAsync<ConsoleOutput>(HandleConsoleOutputAsync);
        }

        private Task HandleConsoleOutputAsync(ConsoleOutput output)
        {
            Console.WriteLine(output.Text);
            return Task.CompletedTask;
        }
    }
}
