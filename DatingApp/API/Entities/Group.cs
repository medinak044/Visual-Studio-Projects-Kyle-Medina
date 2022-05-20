using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Group
    {
        public Group() { } // When constructing tables using Entity Framework, needs empty constructor

        public Group(string name)
        {
            Name = name;
        }

        [Key]
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}
