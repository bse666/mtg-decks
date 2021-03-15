using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ShandalarToCockatrice
{
    static class Writer
    {
        public static void WriteDeck(string path, Deck deck)
        {
            var doc = deck.ToDocument();
            using var stream = File.OpenWrite(path);
            doc.Save(stream);
        }

        static XDocument ToDocument(this Deck deck)
        {
            return new XDocument(
                new XElement("cockatrice_deck", new XAttribute("version", "1"),
                new[]
                {
                    new XElement("deckname", deck.Name),
                    new XElement("comments", ""),
                    new XElement("zone", new XAttribute("name", "main"),
                        deck.Cards.Select(item => new XElement("card",
                            new XAttribute("number", item.Count),
                            new XAttribute("price", "0"),
                            new XAttribute("name", item.Name)
                        ))
                    ),
                    new XElement("zone", new XAttribute("name", "sideboard"),
                        deck.Sideboard.Select(item => new XElement("card",
                            new XAttribute("number", item.Count),
                            new XAttribute("price", "0"),
                            new XAttribute("name", item.Name)
                        ))
                    )
                })
            );
        }
    }
}
