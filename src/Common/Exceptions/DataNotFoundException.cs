namespace MemoryBook.Common.Exceptions
{
    using System;

    public class DataNotFoundException : ArgumentException
    {
        public DataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DataNotFoundException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}