namespace API.Helpers
{
    public class UserParams: PaginationParams
    {
        public string CurrentUsername { get; set; } = "lisa"; // Temp value
        public string Gender { get; set; } = "female"; // Temp value
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
        public string OrderBy { get; set; } = "lastActive";
    }
}
