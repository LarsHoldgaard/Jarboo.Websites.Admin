using System;
using System.Linq;

using Jarboo.Admin.BL.Other;

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

        private Board OpenBoard(string boardName)
        {
            this.EnsureTrello();

            var boards = this.trello.Boards.Search(boardName);
            var board = boards.FirstOrDefault();
            if (board == null)
            {
                throw new Exception("'" + boardName + "' board not found");
            }

            return board;
        }

        public string Register(string boardName, string taskIdentifier, string folderLink)
        {
            this.EnsureTrello();

            var board = this.OpenBoard(boardName);

            var lists = this.trello.Lists.ForBoard(board);
            var list = lists.FirstOrDefault();
            if (list == null)
            {
                list = this.trello.Lists.Add("Tasks", board);
            }

            var card = this.trello.Cards.Add(taskIdentifier, list);

            try
            {
                card.Desc = folderLink ?? "Missing";
                this.trello.Cards.Update(card);
                return card.Url;
            }
            catch (Exception ex)
            {
                this.Unregister(boardName, taskIdentifier, card.Url);
                throw;
            }
        }

        public void Unregister(string boardName, string taskIdentifier, string url)
        {
            this.EnsureTrello();

            var card = this.FindCard(boardName, taskIdentifier, url);
            if (card != null)
            {
                this.trello.Cards.Delete(card);
            }
        }

        private Card FindCard(string boardName, string taskIdentifier, string url)
        {
            this.EnsureTrello();

            var board = this.OpenBoard(boardName);

            var lists = this.trello.Lists.ForBoard(board);
            var list = lists.FirstOrDefault();
            if (list == null)
            {
                return null;
            }

            return this.trello.Cards.ForList(list).FirstOrDefault(x => x.Name == taskIdentifier && x.Url == url);
        }

        public void ChangeResponsible(string boardName, string taskIdentifier, string url, string responsibleUserId)
        {
            this.EnsureTrello();

            var card = this.FindCard(boardName, taskIdentifier, url);
            if (card == null)
            {
                //throw new ApplicationException("Coudn't find the card");
                return;
            }

            if (card.IdMembers.Count == 1 && card.IdMembers[0] == responsibleUserId)
            {
                return;
            }

            foreach (var memberId in card.IdMembers)
            {
                this.trello.Cards.RemoveMember(card, new MemberId(memberId));
            }

            if (string.IsNullOrEmpty(responsibleUserId))
            {
                return;
            }

            try
            {
                this.trello.Cards.AddMember(card, new MemberId(responsibleUserId));
            }
            catch (TrelloException ex)
            {
                throw new ApplicationException("Coudn't set responsible. Check trello Id.", ex);
            }
        }

        public string DefaultBoardName(string customerName)
        {
            return customerName + " tasks";
        }

        public System.Collections.Generic.IEnumerable<string> BoardNames()
        {
            this.EnsureTrello();

            var boards = this.trello.Boards.ForMe();
            return boards.Select(x => x.Name).ToList();
        }
    }
}