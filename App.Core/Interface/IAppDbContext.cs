using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interface
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Conversation> Conversations { get; }
        DbSet<ConversationParticipant> ConversationParticipant { get; }
        DbSet<Messages> Messages { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
