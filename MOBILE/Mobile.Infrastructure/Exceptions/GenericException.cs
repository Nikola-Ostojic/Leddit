using System;

namespace Mobile.Infrastructure.Exceptions
{
    public class GenericException : Exception
    {
        public GenericException() : base("An unknown error happened")
        {
        }
    }
}
