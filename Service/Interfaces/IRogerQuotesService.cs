using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacacusUrbanusBot.Service.Interfaces
{
    public interface IRogerQuotesService
    {
        IEnumerable<string> ListAllQuotes();
        string GetRandomQuote();
    }
}
