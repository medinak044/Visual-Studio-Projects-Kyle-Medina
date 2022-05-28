namespace PokemonReviewApp_2.Dto
{
    // Dtos make sure that only the neccessary data from the object model is being sent
    // back to the user (prevents user sensitive data such as password hashes, payment info)
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
