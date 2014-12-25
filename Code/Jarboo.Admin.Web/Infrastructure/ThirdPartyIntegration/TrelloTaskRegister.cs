using System;
using System.Linq;

using Jarboo.Admin.BL.ThirdParty;

using TrelloNet;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class TrelloTaskRegister : ITaskRegister
    {
        private ITrello trello = null;

        private void EnsureTrello()
        {
            if (this.trello != null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Configuration.TrelloApiKey) || string.IsNullOrEmpty(Configuration.TrelloToken))
            {
                throw new ApplicationException("Missing trello configuration");
            }

            try
            {
                this.trello = new Trello(Configuration.TrelloApiKey);
                this.trello.Authorize(Configuration.TrelloToken);
            }
            catch (Exception ex)
            {
                this.trello = null;

                throw;
            }
        }

        private Board OpenBoard(string customerName)
        {
            this.EnsureTrello();

            var boardName = customerName + " tasks";

            var boards = this.trello.Boards.Search(boardName);
            var board = boards.FirstOrDefault();
            if (board == null)
            {
                throw new Exception("'" + boardName + "' board not found");
            }

            return board;
        }

        public string Register(string customerName, string taskTitle)
        {
            this.EnsureTrello();

            var board = this.OpenBoard(customerName);

            var lists = this.trello.Lists.ForBoard(board);
            var list = lists.FirstOrDefault();
            if (list == null)
            {
                list = this.trello.Lists.Add("Tasks", board);
            }

            var card = this.trello.Cards.Add(taskTitle, list);
            return card.Url;
        }

        public void Unregister(string customerName, string taskTitle)
        {
            this.EnsureTrello();

            var board = this.OpenBoard(customerName);

            var lists = this.trello.Lists.ForBoard(board);
            var list = lists.FirstOrDefault();
            if (list == null)
            {
                return;
            }

            var card = this.trello.Cards.ForList(list).FirstOrDefault(x => x.Name == taskTitle);
            if (card != null)
            {
                this.trello.Cards.Delete(card);
            }
        }
    }
}