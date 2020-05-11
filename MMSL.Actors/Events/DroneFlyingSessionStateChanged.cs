using System;

namespace Harvested.AI.Actors.Events {
    public class DroneFlyingSessionStateChanged {
        public enum SessionState {
            Started,
            Stopped
        }
        public long OwnerId { get; private set; }
        public Guid SessionId { get; private set; }
        public Guid DroneId { get; private set; }
        public SessionState State { get; private set; }

        public DroneFlyingSessionStateChanged(Guid sessionId, Guid droneId, SessionState state, long ownerId) {
            SessionId = sessionId;
            DroneId = droneId;
            State = state;
            OwnerId = ownerId;
        }
    }
}
