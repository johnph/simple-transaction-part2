using Publisher.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Framework.Messages
{
    public class TriggerMessage : BaseMessage
    {
        public IEnumerable<int> AccountNumbers { get; set; }
    }
}
