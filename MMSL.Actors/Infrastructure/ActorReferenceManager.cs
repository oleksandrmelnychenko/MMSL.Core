using System;
using System.Collections.Generic;
using Akka.Actor;

namespace MMSL.Actors.Infrastructure
{
    public sealed class ActorReferenceManager : Singleton<ActorReferenceManager>
    {

        private readonly object _lock = new object();

        private readonly Dictionary<string, IActorRef> _container =
            new Dictionary<string, IActorRef>();

        public void Add(string name, IActorRef actorRef)
        {
            lock (_lock)
            {
                if (_container.ContainsKey(name))
                    throw new Exception("Actor Reference with specified name already exists");

                _container.Add(name, actorRef);
            }
        }

        public IActorRef Get(string name)
        {
            lock (_lock)
            {
                if (!_container.ContainsKey(name))
                    throw new Exception("Manager does not contains reference to specified actor name");

                return _container[name];
            }
        }

    }
}
