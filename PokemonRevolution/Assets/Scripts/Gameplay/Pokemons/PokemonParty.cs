using System.Collections.Generic;

public class PokemonParty
{
    public List<Pokemon> Pokemons { get; private set; }

    public PokemonParty() { Pokemons = new List<Pokemon>();}

    public PokemonParty(List<Pokemon> pokemons) { Pokemons = pokemons; }

    public Pokemon GetFirstPokemon()
    {
        foreach (Pokemon pokemon in Pokemons)
        {
            if (!pokemon.IsFainted)
                return pokemon;
        }
        return null;
    }
}