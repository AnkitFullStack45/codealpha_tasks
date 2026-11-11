namespace Circlely.Models
{
    public class CallHistory
    {
        public int id { get; set; }
        public string CallerId { get; set; } = string.Empty;
        public ApplicationUser Caller { get; set; } = null!;
        public string RecieverId { get; set; } = string.Empty;
        public ApplicationUser Reciever { get; set; } = null!;
        public string CallType { get; set; } = "Video";
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string Status { get; set; } = "Completed";
    }
}
