namespace PokemonReviewApp_2.Dto
{
    // Dtos make sure that only the neccessary data from the object model is being sent
    // back to the user (prevents user sensitive data such as password hashes, payment info)
    // "Dtos are essentially a way to make sure you're not returning all of your data."
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
