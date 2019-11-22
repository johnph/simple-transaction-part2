using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Framework.Messages
{
    public class StatementMessage : BaseMessage
    {
        public string Name { get; set; }
        public int AccountNumber { get; set; }
        public string Currency { get; set; }
    }
}
