using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.ConversationDtos
{
    public class CreateConversationRequest
    {
        public string? ConversationName { get; set; }
        public List<int> UserIds { get; set; }
    }

    public class GetMyConversationResponseDto
    {
        public int ConversationId { get; set; }
        public string? ConversationName { get; set; }
        public bool IsOneToOne { get; set; }
    }
}
