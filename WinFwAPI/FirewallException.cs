﻿using System;
using System.Runtime.Serialization;

namespace WinFwAPI
{
    [Serializable]
    public class FirewallException : Exception
    {
        public FirewallException()
        {
        }

        public FirewallException(string message) : base(message)
        {
        }

        public FirewallException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FirewallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}