using System;

namespace TypicalMeepo.Core.Exceptions
{
    internal class TypicalMeepoException : Exception
    {
        public TypicalMeepoException(string message) : base(message) { }

        public TypicalMeepoException(string message, Exception ex) : base(message, ex) { }
    }
}