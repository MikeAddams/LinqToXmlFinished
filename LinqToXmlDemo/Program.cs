using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LinqToXmlFinished
{
    // Avem un fisier xml cu o lista de carti
    // de afisat numele autorilor impreuna cu informatia despre cartile lor (titlu, data publicarii, pret)
    // autorii trebuie afisati in ordine descrescatoare dupa numarul cartilor afisate si in ordine crescatoare dupa nume
    // cartile unui autor trebuie sa fie afisate in ordine crescatoare dupa data publicarii
    // de afisat doar cartile cu pretul mai mare de $5.0
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "books.xml";
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, filename);

            XElement books = XElement.Load(filePath);

            var query = books.Descendants("book")
                .Select(x =>
                new
                {
                    author = x.Element("author").Value,
                    title = x.Element("title").Value,
                    date = x.Element("publish_date").Value,
                    price = x.Element("price").Value
                })
                .GroupBy(x => x.author)
                .OrderByDescending(x => x.Count())
                .ThenBy(x => x.Key)
                .SelectMany(x => x.OrderBy(n => n.date))
                .Where(x => float.TryParse(x.price, out float n) && n > 5 )
                .GroupBy(x => x.author); // not necessary, just to represent easier


            foreach (var book in query)
            {
                Console.WriteLine($"author: {book.Key}");

                foreach (var details in book)
                    //Console.WriteLine($"\ttitle: {details.title}");
                    Console.WriteLine($"\ttitle: {details.title} | price: {details.price} | date: {details.date}"); // for checking...

                Console.WriteLine();
            }
        }
    }
}
