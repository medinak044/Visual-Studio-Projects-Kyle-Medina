using PokemonReviewApp_2.Data;
using PokemonReviewApp_2.Interfaces;
using PokemonReviewApp_2.Models;

namespace PokemonReviewApp_2.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }
    }
}
