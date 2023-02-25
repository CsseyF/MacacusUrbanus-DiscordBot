using MacacusUrbanusBot.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacacusUrbanusBot.Service
{
    public class RogerQuotesService : IRogerQuotesService
    {
        private readonly string[] file = File.ReadAllLines("../../../Source/Texts/RodrigoQuotes.txt");

        public IEnumerable<string> ListAllQuotes()
        {
            return file.ToList();
        }

        public string GetRandomQuote()
        {
            var rnd = new Random();
            var r = rnd.Next(file.Count());
            return file[r];
        }
    }
}
