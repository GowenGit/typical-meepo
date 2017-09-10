using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meepo;
using Meepo.Core.Configs;
using Meepo.Core.StateMachine;

namespace TypicalMeepo.Tests.Unit
{
    internal class FakeMeepoNode : IMeepoNode
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Dictionary<Guid, TcpAddress> GetServerClientInfos()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(Guid id, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void RemoveClient(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public State ServerState { get; }

        public event MessageReceivedHandler MessageReceived;

        public int GetSubscriberCount()
        {
            return MessageReceived?.GetInvocationList().Length ?? 0;
        }
    }
}
