namespace Circlely.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ConversationMember> Members { get; set; } = new List<ConversationMember>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
