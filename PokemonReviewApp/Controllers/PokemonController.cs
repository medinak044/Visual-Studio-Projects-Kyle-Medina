using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController
    {
        readonly IPokemonRepository _pokemonRepository;

        public PokemonController(IPokemonRepository pokemonRepository) // Shortcut: "ctor"
        { _pokemonRepository = pokemonRepository; }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();

            if (!ModelState.IsValid) 
            { return BadRequest(ModelState); }

            return Ok(pokemons);
            // Stopped at: https://www.youtube.com/watch?v=-LAeEQSfOQk&list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0&index=6
        }
    }
}
