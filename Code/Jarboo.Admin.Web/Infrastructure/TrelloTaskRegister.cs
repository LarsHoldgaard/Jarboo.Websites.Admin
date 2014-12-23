using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.ThirdParty;

using TrelloNet;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class TrelloTaskRegister : ITaskRegister
    {
        private ITrello trello = null;

        private void EnsureTrello()
        {
            if (trello != null)
            {
                return;
            }

            try
            {
                trello = new Trello("46f467359a59f251c2e156457f85e31a");
                trello.Authorize("824811c32225f61d7aaaa405fd7ee55ff53aac3c50b51c3f4cea099ffe0a52ef");
            }
            catch (Exception ex)
            {
                trello = null;

                throw;
            }
        }

        private Board OpenBoard(string customerName)
        {
            EnsureTrello();

            var boardName = customerName + " tasks";

            var boards = trello.Boards.Search(boardName);
            var board = boards.FirstOrDefault();
            if (board == null)
            {
                throw new Exception("'" + boardName + "' board not found");
            }

            return board;
        }

        public void Register(string customerName, string taskTitle)
        {
            EnsureTrello();

            var board = this.OpenBoard(customerName);

            var lists = trello.Lists.ForBoard(board);
            var list = lists.FirstOrDefault();
            if (list == null)
            {
                list = trello.Lists.Add("Tasks", board);
            }

            trello.Cards.Add(taskTitle, list);
        }

        public void Unregister(string customerName, string taskTitle)
        {
            EnsureTrello();

            var board = this.OpenBoard(customerName);

            var lists = trello.Lists.ForBoard(board);
            var list = lists.FirstOrDefault();
            if (list == null)
            {
                return;
            }

            var card = trello.Cards.ForList(list).FirstOrDefault(x => x.Name == taskTitle);
            if (card != null)
            {
                trello.Cards.Delete(card);
            }
        }
    }
}