using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.UIHelpers
{
    public class ChatMessage
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public bool IsUserMessage { get; set; }

        public string FullMessage => $"{Name}: {Content}";
    }
}
