namespace MemoryBook.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Contract
    {
        public static void RequiresNotNull<T>(T item, string paramName)
            where T : class
        {
            if (item == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void RequiresNotNullOrEmpty<T>(IEnumerable<T> items, string paramName)
        {
            if (items == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (!items.Any()) 
            {
                throw new ArgumentException(paramName);
            }
        }

        public static void RequiresNotNullOrWhitespace(string item, string paramName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException(paramName);
            }
        }
    }
}