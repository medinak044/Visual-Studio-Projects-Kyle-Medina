using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository: IPokemonRepository
    {
        readonly DataContext _context;

        public PokemonRepository(DataContext context)
        { _context = context; }

        // Database call
        public ICollection<Pokemon> GetPokemons()
        { return _context.Pokemon.OrderBy(p => p.Id).ToList(); }
    }
}
