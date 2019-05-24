using System;

namespace ColtSmart.Data
{
    internal sealed class Check
    {
        public static T ArgumentNotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static T? ArgumentNotNull<T>(T? value, string parameterName) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string ArgumentNotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(string.Format("{0} is null or empty!", parameterName));
            }

            return value;
        }

        public static void ArgumentValid(Func<bool> condition, string argumentName, string exceptionMessage)
        {
            if (condition.Invoke())
            {
                throw new ArgumentException(exceptionMessage, argumentName);
            }
        }


        public static void ArgumentValid(bool valid, string argumentName, string exceptionMessage)
        {
            if (!valid)
            {
                throw new ArgumentException(exceptionMessage, argumentName);
            }
        }

        public static void OperationValid(bool valid, string exceptionMessage)
        {
            if (!valid)
            {
                throw new InvalidOperationException(exceptionMessage);
            }
        }

        public static void OperationValid(Func<bool> condition, string exceptionMessage)
        {
            if (condition.Invoke())
            {
                throw new InvalidOperationException(exceptionMessage);
            }
        }
    }
}
