using System;
using System.IO;
using System.Linq;

namespace ShandalarToCockatrice
{
    static class Program
    {
        const string sourceDir = "C:/program files (x86)/magictg/decks";
        const string targetDir = "C:/users/james/desktop/decks";

        static void Main()
        {
            var allFiles = Directory.GetFiles(sourceDir).Take(1);
            foreach (var f in allFiles)
            {
                Console.WriteLine($"Processing {f}");
                var shandalarDeck = Parser.ParseDeck(f);
                var deck = Mapper.MapDeck(shandalarDeck);
                Validator.Validate(deck);
                var targetPath = Path.Combine(targetDir, $"{deck.Name}.cod");
                Writer.WriteDeck(targetPath, deck);
            }

            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
