using App.Core.Interface.IServices;
using Common.Dtos.ChatHubDtos;
using Common.Dtos.MessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SignalR_Backend.HubConnection
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"[ SignalR ] : User Connected: {userId}");

            await base.OnConnectedAsync();
        }

        // join the conversation 
        public async Task JoinConversation(int conversationId)
        {
            string groupName = GetConversationGroupName(conversationId);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Leave a group
        public async Task LeaveGroup(int conversationId)
        {
            string groupName = GetConversationGroupName(conversationId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        // Send to a group
        public async Task SendMessageToConversation(int conversationId, string message, int userId)
        {
            string groupName = GetConversationGroupName(conversationId);

            var response = new ReciveMessageDto
            {
                ConversationId = conversationId,
                Message = message,
                UserId = userId
            };

            // Sending Singals
            await Clients.Group(groupName).SendAsync("ReceiveConversationGroupMessage", response);

            // Saving message to Database
            var req = new SaveMessaageReq
            {
                ConversationId = conversationId,
                MessageContent = message,
                UserId = userId
            };

            await _messageService.SaveMessageAsync(req);            
        }

        private static string GetConversationGroupName(int conversationId)
        {
            return $"conversation_{conversationId}";
        }
    }
}
