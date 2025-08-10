using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ConversationParticipant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConversationParticipantId { get; set; }
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(ConversationId))]
        public Conversation Conversation { get; set; }

    }
}
