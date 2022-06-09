namespace Practice_WebAPI_01.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
