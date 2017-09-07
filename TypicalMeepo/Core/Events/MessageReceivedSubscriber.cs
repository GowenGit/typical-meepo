using System;
using System.Collections.Generic;
using Meepo.Core.Extensions;
using TypicalMeepo.Core.Exceptions;
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
                    try
                    {
                        if (Type.GetType(args.Bytes.Decode()) == typeof(T))
                        {
                            messageIncomingTracker[args.Id] = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new TypicalMeepoException("Failed to identify type of package!", ex);
                    }
                }
                else
                {
                    messageIncomingTracker[args.Id] = false;

                    try
                    {
                        handler(args.Id, args.Bytes.BytesToPackage<T>());
                    }
                    catch (Exception ex)
                    {
                        throw new TypicalMeepoException($"Failed to deserialize package to {nameof(T)}!", ex);
                    }
                }
            }
        }
    }
}
