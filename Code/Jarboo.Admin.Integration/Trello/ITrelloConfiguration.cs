using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.Integration.Trello
{
    public interface ITrelloConfiguration
    {
        string TrelloApiKey { get; }
        string TrelloToken { get; }
    }
}
