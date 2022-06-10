namespace Practice_WebAPI_01.Models
{
    public class HeroUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Credit { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ICollection<Weapon> Weapons { get; set; }
    }
}
