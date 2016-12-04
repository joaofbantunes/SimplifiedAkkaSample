using Akka.Actor;
using CodingMilitia.AkkaSampleApplication.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodingMilitia.AkkaSampleApplication.Actors
{
    public class StuffDistributionActor : ReceiveActor
    {
        private readonly IActorRef _consoleWriterActor;
        private readonly Dictionary<int, IActorRef> _stuffHandlers;

        public StuffDistributionActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
            _stuffHandlers = new Dictionary<int, IActorRef>();

            ReceiveAsync<StuffWrapper>(HandleStuffWrapperAsync);
            ReceiveAsync<StuffCountRequestWrapper>(HandleStuffCountRequestWrapperAsync);
        }

        private Task HandleStuffWrapperAsync(StuffWrapper stuffWrapper)
        {
            IActorRef stuffHandler = GetStuffHandler(stuffWrapper.StuffHandlerId);
            stuffHandler.Tell(stuffWrapper.Stuff);
            return Task.CompletedTask;
        }

        private Task HandleStuffCountRequestWrapperAsync(StuffCountRequestWrapper stuffCountRequestWrapper)
        {
            IActorRef stuffHandler = GetStuffHandler(stuffCountRequestWrapper.StuffHandlerId);
            stuffHandler.Tell(new StuffCountRequestWithOriginalSender { OriginalSender = Sender, StuffCountRequest = stuffCountRequestWrapper.StuffCountRequest });
            return Task.CompletedTask;
        }

        private IActorRef GetStuffHandler(int id)
        {
            IActorRef stuffHandler;
            if (!_stuffHandlers.TryGetValue(id, out stuffHandler))
            {
                stuffHandler = Context.ActorOf(Props.Create(() => new StuffActor(id, _consoleWriterActor)));
                _stuffHandlers.Add(id, stuffHandler);
                _consoleWriterActor.Tell(new ConsoleOutput { Text = $"Created new actor with id {id}" });
            }
            else
            {
                _consoleWriterActor.Tell(new ConsoleOutput { Text = $"Reusing existing actor with id {id}" });
            }

            return stuffHandler;
        }
    }
}
