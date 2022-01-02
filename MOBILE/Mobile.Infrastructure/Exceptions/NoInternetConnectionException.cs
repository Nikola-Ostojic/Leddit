using System;

namespace Mobile.Infrastructure.Exceptions
{
    public class NoInternetConnectionException : Exception
    {
        public NoInternetConnectionException() : base("No Internet connection")
        {

        }
    }
}
