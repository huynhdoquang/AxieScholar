using System;
using System.Net;

namespace Net.HungryBug.Core.Network
{
    public enum HttpExceptionType
    {
        Connection,
        HttpStatus,
        Parse
    }

    public class HttpException : Exception
    {
        /// <summary>
        /// Gets the <see cref="HttpExceptionType"/>
        /// </summary>
        public HttpExceptionType Type { get; }

        /// <summary>
        /// Gets the http status error code.
        /// </summary>
        public HttpStatusCode Code { get; }

        public HttpException(HttpExceptionType type, HttpStatusCode code, string message) : base(message)
        {
            this.Type = type;
            this.Code = code;
        }
    }
}
