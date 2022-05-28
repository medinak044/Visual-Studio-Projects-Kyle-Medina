using PokemonReviewApp_2.Models;

namespace PokemonReviewApp_2.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
    }
}
