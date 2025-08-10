using Common.Dtos.MessageDtos;
using Common.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interface.IServices
{
    public interface IMessageService
    {
        Task<AppResponse> SaveMessageAsync(SaveMessaageReq req);
        Task<AppResponse<List<GetMessagesResponseDto>>> GetMessagesByConversationIdAsync(int conversationId);
    }


}
