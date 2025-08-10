using App.Core.Interface;
using App.Core.Interface.IServices;
using Common.Dtos.MessageDtos;
using Common.GenericResponse;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class MessageService : IMessageService
    {
        private readonly IAppDbContext _appDbContext;

        public MessageService(IAppDbContext appDbContext)
        {
             _appDbContext = appDbContext;
        }

        public async Task<AppResponse> SaveMessageAsync(SaveMessaageReq req)
        {
            var message = new Messages
            {
                MessageContent = req.MessageContent,
                UserId = req.UserId,
                ConversationId = req.ConversationId,
            };

            await _appDbContext.Messages.AddAsync(message);
            await _appDbContext.SaveChangesAsync();

            return AppResponse.Response(true, "Message Saved Successfully", HttpStatusCodes.OK);
        }

        public async Task<AppResponse<List<GetMessagesResponseDto>>> GetMessagesByConversationIdAsync(int conversationId)
        {
            var messages = await _appDbContext.Messages
                .AsNoTracking()
                .Where(m => m.ConversationId == conversationId && m.IsActive && !m.IsDeleted)
                .Select(m => new GetMessagesResponseDto
                {
                    ConversationId = m.ConversationId,
                    MessageContent = m.MessageContent,
                    UserId = m.UserId,
                    MessageId = m.MessageId
                })
                .ToListAsync();

            if (!messages.Any())
                return AppResponse.Fail<List<GetMessagesResponseDto>>([], "No message Found", HttpStatusCodes.NotFound);

            return AppResponse.Success(messages);
        }
    }
}
