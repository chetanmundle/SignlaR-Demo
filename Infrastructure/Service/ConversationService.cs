using App.Core.Interface;
using App.Core.Interface.IServices;
using Common.Dtos.ConversationDtos;
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
    public class ConversationService : IConversationService
    {
        public readonly IAppDbContext _appDbContext;

        public ConversationService(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<AppResponse> CreateConversationAsync(CreateConversationRequest request)
        {
            var existingConversation = await _appDbContext.Conversations
                    .AsNoTracking()
                    .Include(c => c.Participants)
                    .Where(c =>
                        !c.IsDeleted &&
                        c.IsActive &&
                        c.Participants.Count == request.UserIds.Count &&
                        c.Participants.All(p =>
                            !p.IsDeleted &&
                            p.IsActive &&
                            request.UserIds.Contains(p.UserId)
                        )
                    )
                    .FirstOrDefaultAsync();

            if(existingConversation != null )
            {
                return AppResponse.Response(false, $"Conversation already Exist with name : {existingConversation.ConversationName}", HttpStatusCodes.Conflict);
            }

            int countOfParticipants = request.UserIds.Count;

            var newConversation = new Conversation
            {
                ConversationName = countOfParticipants == 2 ? "" : request.ConversationName,
                IsOneToOne = countOfParticipants == 2 ? true : false,                
            };

            await _appDbContext.Conversations.AddAsync(newConversation);
            await _appDbContext.SaveChangesAsync();

            foreach(var userId in request.UserIds)
            {
                var participant = new ConversationParticipant
                {
                    ConversationId = newConversation.ConversationId,
                    UserId = userId,
                };

                await _appDbContext.ConversationParticipant.AddAsync(participant);
            }

            await _appDbContext.SaveChangesAsync();

            return AppResponse.Response(true, "Conversation Created Successfully", HttpStatusCodes.OK);

        }

        public async Task<AppResponse<List<GetMyConversationResponseDto>>> GetMyConversationAsync(int userId)
        {
            var conversations = await _appDbContext.Conversations
                .AsNoTracking()
                .Where(c => c.IsActive && !c.IsDeleted && c.Participants.Any(p => p.UserId == userId && p.IsActive && !p.IsDeleted))
                .Select(c => new GetMyConversationResponseDto
                {
                    ConversationId = c.ConversationId,
                    ConversationName = c.Participants.Where(p => p.IsActive && !p.IsDeleted).Count() == 2
                                ? c.Participants.Where(p => p.UserId != userId).Select(p => p.User.Name).FirstOrDefault()
                                : c.ConversationName,
                    IsOneToOne = c.IsOneToOne
                })
                .ToListAsync();

            if (conversations == null)
                return AppResponse.Fail<List<GetMyConversationResponseDto>>([], "No Conversation Found", HttpStatusCodes.NotFound);

            return AppResponse.Success(conversations);
        }
    }
}
