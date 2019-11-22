using System;

namespace Transaction.Framework.Types
{
    public class StatementDate
    {
        public StatementDate(string dateString)
        {
            StartDate = Convert.ToDateTime(dateString);
            EndDate = StartDate.AddMonths(1).AddTicks(-1);
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static implicit operator StatementDate(string date)
        {
            return string.IsNullOrEmpty(date) ? null : new StatementDate(date);
        }
    }
}
