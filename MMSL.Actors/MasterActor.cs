using Akka.Actor;

namespace MMSL.Actors {
    public class MasterActor : ReceiveActor
    {
        public MasterActor()
        {
            //ActorReferenceManager.Instance.Add(
            //    ActorNames.DRONE_CONNECTION_ACTOR,
            //    Context.ActorOf(Context.DI().Props<DroneConnectionServerActor>(), ActorNames.DRONE_CONNECTION_ACTOR)
            //);

            //ActorReferenceManager.Instance.Add(
            //    ActorNames.DRONE_INFO_DELIVERING_ACTOR,
            //    Context.ActorOf(Context.DI().Props<DroneInfoDeliveringActor>(), ActorNames.DRONE_INFO_DELIVERING_ACTOR)
            //    );
        }
    }
}
