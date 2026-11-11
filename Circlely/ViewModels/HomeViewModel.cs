namespace Circlely.ViewModels
{
    public class HomeViewModel
    {
        public List<PostViewModel> Posts { get; set; } = new();

        public int NotificationCount { get; set; }

        public int MessageCount { get; set; }
    }
}
