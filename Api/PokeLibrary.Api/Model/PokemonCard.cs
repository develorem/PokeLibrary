using System.Collections.Generic;

namespace PokeLibrary.Api.Model
{
    public class PokemonCard
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int NationalPokedexNumber { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlHiRes { get; set; }
        public IEnumerable<string> Types { get; set; }
        public string Supertype { get; set; }
        public string Subtype { get; set; }
        public string Hp { get; set; }
        public IEnumerable<string> RetreatCost { get; set; }
        public int ConvertedRetreatCost { get; set; }
        public string Number { get; set; }
        public string Artist { get; set; }
        public string Rarity { get; set; }
        public string Series { get; set; }
        public string Set { get; set; }
        public string SetCode { get; set; }
        public IEnumerable<Attack> Attacks { get; set; }
        public IEnumerable<Weakness> Weaknesses { get; set; }

        public int Count { get; set; } // Not part of the public api, but we use it EVERYWHERE
    }

    public class Attack
    {
        public IEnumerable<string> Cost { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Damage { get; set; }
        public int ConvertedEnergyCost { get; set; }
    }

    public class Weakness
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class SingleCardResult
    {
        public PokemonCard Card { get; set; }
    }
    public class MultipleCardsResult
    {
        public IEnumerable<PokemonCard> Cards { get; set; }
    }
}
