using Akka.Actor;
using CodingMilitia.AkkaSampleApplication.Messages;
using System.Threading.Tasks;
using System;

namespace CodingMilitia.AkkaSampleApplication.Actors
{
    public class StuffActor : ReceiveActor
    {
        private int _messageCount;

        private readonly int _id;
        private readonly IActorRef _consoleWriterActor;

        public StuffActor(int id, IActorRef consoleWriterActor)
        {
            _id = id;
            _consoleWriterActor = consoleWriterActor;

            ReceiveAsync<Stuff>(HandleStuffAsync);
            ReceiveAsync<StuffCountRequest>(HandleStuffCountAsync);
        }

        private Task HandleStuffAsync(Stuff stuff)
        {
            ++_messageCount;
            _consoleWriterActor.Tell(new ConsoleOutput { Text = $"Actor {_id} received message {_messageCount}. Message text: \"{stuff.Text}\"" });
            return Task.CompletedTask;
        }

        private Task HandleStuffCountAsync(StuffCountRequest request)
        {
            Sender.Tell(new StuffCountResponse { Count = _messageCount });
            return Task.CompletedTask;
        }

    }
}
