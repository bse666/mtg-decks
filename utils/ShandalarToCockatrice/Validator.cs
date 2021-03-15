using System;
using System.Collections.Generic;
using System.Linq;

namespace ShandalarToCockatrice
{
    static class Validator
    {
        public static void Validate(Deck deck)
        {
            if (string.IsNullOrWhiteSpace(deck.Name)) throw new Exception("Deck name cannot be blank.");

            var duplicates = deck.Cards.GroupBy(Mapper.GetKey).Where(grp => grp.Count() > 1);
            if (duplicates.Any()) throw new Exception($"These cards are listed more than once in the deck ${deck.Name}: ${duplicates.Select(grp => grp.First()).ToListString()}");

            duplicates = deck.Sideboard.GroupBy(Mapper.GetKey).Where(grp => grp.Count() > 1);
            if (duplicates.Any()) throw new Exception($"These cards are listed more than once in the sideboard of ${deck.Name}: ${duplicates.Select(grp => grp.First()).ToListString()}");

            var deckWithSideboard = deck.Cards.Concat(deck.Sideboard).ConsolidateDuplicates(Enumerable.Sum).ToArray();

            var lessThan1 = deckWithSideboard.Where(x => x.Count < 1);
            if (lessThan1.Any()) throw new Exception($"These cards have a count less than 1 in the deck ${deck.Name}: ${lessThan1.ToListString()}");

            var moreThan4 = deckWithSideboard.Where(x => x.Count > 4 && !IsBasicLand(x));
            if (moreThan4.Any()) throw new Exception($"These cards have a count more than 4 in the deck ${deck.Name}: ${moreThan4.ToListString()}");

            var count = deck.Cards.Sum(x => x.Count);
            if (count < 60) throw new Exception($"The deck ${deck.Name} has less than 60 cards.");

            var sideboardCount = deck.Sideboard.Sum(x => x.Count);
            if (sideboardCount > 15) throw new Exception($"The sideboard of ${deck.Name} has more than 15 cards.");
        }

        static string ToListString(this IEnumerable<DeckItem> items) =>
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
    }
}
