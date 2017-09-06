using System;
using System.Collections.Generic;
using Meepo.Core.Extensions;
using TypicalMeepo.Core.Extensions;

namespace TypicalMeepo.Core.Events
{
    internal class MessageReceivedSubscriber<T>
    {
        private readonly object thisLock = new object();

        private readonly MessageReceivedHandler<T> handler;

        private readonly Dictionary<Guid, bool> messageIncomingTracker = new Dictionary<Guid, bool>();

        public MessageReceivedSubscriber(MessageReceivedHandler<T> handler)
        {
            this.handler = handler;
        }

        public void OnMessageReceived(MessageReceivedEventArgs args)
        {
            lock (thisLock)
            {
                if (!messageIncomingTracker.ContainsKey(args.Id) || !messageIncomingTracker[args.Id])
                {
                    if (Type.GetType(args.Bytes.Decode()) == typeof(T))
                    {
                        messageIncomingTracker[args.Id] = true;
                    }
                }
                else
                {
                    messageIncomingTracker[args.Id] = false;

                    handler(args.Id, args.Bytes.BytesToPackage<T>());
                }
            }
        }
    }
}
