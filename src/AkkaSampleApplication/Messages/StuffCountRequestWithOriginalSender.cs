using Akka.Actor;

namespace CodingMilitia.AkkaSampleApplication.Messages
{
    public class StuffCountRequestWithOriginalSender
    {
        public IActorRef OriginalSender { get; set; }
        public StuffCountRequest StuffCountRequest { get; set; }
    }
}
