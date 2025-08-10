using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.ChatHubDtos
{
    public class ReciveMessageDto
    {
        public  int ConversationId { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}
