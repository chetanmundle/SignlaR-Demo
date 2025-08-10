using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.MessageDtos
{
    public class SaveMessaageReq
    {
        public string MessageContent { get; set; }
        public int UserId { get; set; }
        public int ConversationId { get; set; }
    }

    public class GetMessagesResponseDto
    {
        public int MessageId { get; set; }
        public string MessageContent { get; set; }
        public int UserId { get; set; }
        public int ConversationId { get; set; }
    }
}
