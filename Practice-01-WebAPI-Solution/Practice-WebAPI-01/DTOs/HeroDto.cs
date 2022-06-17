using System.ComponentModel.DataAnnotations;

namespace Practice_WebAPI_01.DTOs
{
    public class HeroDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        public int Credit { get; set; }
    }
}
