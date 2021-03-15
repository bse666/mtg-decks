using System;
using System.Collections.Generic;
using System.Linq;

namespace ShandalarToCockatrice
{
    static class Validator
    {
        public static IEnumerable<string> Validate(Deck deck)
        {
            if (string.IsNullOrWhiteSpace(deck.Name)) throw new Exception("Deck name cannot be blank.");

            var duplicates = deck.Cards.GroupBy(Mapper.GetKey).Where(grp => grp.Count() > 1);
            if (duplicates.Any()) yield return $"The deck {deck.Name} has duplicate listings for {duplicates.Select(grp => grp.First()).ToListString()}";

            duplicates = deck.Sideboard.GroupBy(Mapper.GetKey).Where(grp => grp.Count() > 1);
            if (duplicates.Any()) yield return $"The sideboard of {deck.Name} has duplicate listings for {duplicates.Select(grp => grp.First()).ToListString()}";

            var deckWithSideboard = deck.Cards.Concat(deck.Sideboard).ConsolidateDuplicates(Enumerable.Sum).ToArray();

            var lessThan1 = deckWithSideboard.Where(x => x.Count < 1);
            if (lessThan1.Any()) yield return $"The deck {deck.Name} has less than 1 of {lessThan1.ToListString()}";

            var moreThan4 = deckWithSideboard.Where(x => x.Count > 4 && !IsBasicLand(x));
            if (moreThan4.Any()) yield return $"The deck {deck.Name} has more than 4 of {moreThan4.ToListString()}";

            var count = deck.Cards.Sum(x => x.Count);
            if (count < 60) yield return $"The deck {deck.Name} has less than 60 cards.";

            var sideboardCount = deck.Sideboard.Sum(x => x.Count);
            if (sideboardCount > 15) yield return $"The sideboard of {deck.Name} has more than 15 cards.";

            var astralCards = deckWithSideboard.Where(IsAstral);
            if (astralCards.Any()) yield return $"The deck {deck.Name} has these Astral cards: {astralCards.ToListString()}";

            var anteCards = deckWithSideboard.Where(IsAnte);
            if (anteCards.Any()) yield return $"The deck {deck.Name} has these ante cards: {anteCards.ToListString()}.";
        }

        public static string ToListString(this IEnumerable<DeckItem> items) =>
            string.Join(", ", items.Select(x => x.Name));

        static bool IsBasicLand(this DeckItem item) =>
            _basicLandNames.Contains(Mapper.GetKey(item));

        static readonly string[] _basicLandNames = new[]
        {
            "plains",
            "island",
            "swamp",
            "mountain",
            "forest",
            "snow-covered plains",
            "snow-covered island",
            "snow-covered swamp",
            "snow-covered mountain",
            "snow-covered forest",
            "wastes"
        };

        public static bool IsAstral(this DeckItem item) =>
            _astralCardNames.Contains(Mapper.GetKey(item));

        static readonly string[] _astralCardNames = new[]
        {
            "aswan jaguar",
            "call from the grave",
            "faerie dragon",
            "gem bazaar",
            "goblin polka band",
            "necropolis of azar",
            "orcish catapult",
            "pandora's box",
            "power struggle",
            "prismatic dragon",
            "rainbow knights",
            "whimsy"
        };

        public static bool IsAnte(this DeckItem item) =>
            _anteCardNames.Contains(Mapper.GetKey(item));

        static readonly string[] _anteCardNames = new[]
        {
            "amulet of quoz",
            "bronze tablet",
            "contract from below",
            "darkpact",
            "demonic tutor",
            "jeweled bird",
            "rebirth",
            "tempest efreet",
            "timmerian fiends"
        };
    }
}
