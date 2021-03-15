using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ShandalarToCockatrice
{
    static class Program
    {
        const string sourceDir = "C:/program files (x86)/magictg/decks";

        static void Main(string[] args)
        {
            var allFiles = Directory.GetFiles(sourceDir).Take(1);
            foreach (var f in allFiles)
            {
                Console.WriteLine($"Parsing {f}");
                var shandalarDeck = Parser.ParseDeck(f);
                var deck = Mapper.MapDeck(shandalarDeck);
                Validator.Validate(deck);
                Console.WriteLine(JsonConvert.SerializeObject(deck, Formatting.Indented));
            }

            Console.Read();
        }
    }
}
