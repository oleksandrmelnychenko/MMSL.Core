namespace MMSL.Actors.Messages {
    public abstract class ActorsBaseMessage<T> {
        public T Message { get; }

        public string HubMethod { get; }

        protected ActorsBaseMessage(T message, string hubMethod) {
            Message = message;
            HubMethod = hubMethod;
        }
    }
}
