namespace PokemonReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string Text { get; set; }
        public Review Reviewer { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
