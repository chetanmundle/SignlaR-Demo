using Common.Dtos.ConversationDtos;
using Common.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interface.IServices
{
    public interface IConversationService
    {
        Task<AppResponse> CreateConversationAsync(CreateConversationRequest request);

        Task<AppResponse<List<GetMyConversationResponseDto>>> GetMyConversationAsync(int userId);
    }
}
