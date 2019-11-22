using System;
using System.Collections.Generic;
using System.Text;

namespace Statement.Framework.Exceptions
{
    public abstract class StatementException : Exception
    {
        public StatementException(string message)
            : base(message)
        { }

        public abstract int ErrorCode { get; }
    }

    public class StatementUnavailableException : StatementException
    {
        public StatementUnavailableException(int accountNumber, string month)
        : base($"The account statement for {accountNumber} and {month} is not available.")
        { }

        public override int ErrorCode =>
            StatementErrorCode.StatementNotAvailable;
    }

    public static class StatementErrorCode
    {
        public const int StatementNotAvailable = 2001;
    }
}
