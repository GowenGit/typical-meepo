using System;

namespace TypicalMeepo.Core.Events
{
    public delegate void MessageReceivedHandler<in T>(Guid id, T data);
}
