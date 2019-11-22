using Publisher.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Framework.Messages
{
    public abstract class BaseMessage : Command
    {
        public string Month { get; set; }
    }
}
