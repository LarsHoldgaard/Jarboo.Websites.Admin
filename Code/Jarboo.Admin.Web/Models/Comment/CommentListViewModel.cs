using System.Collections.Generic;

namespace Jarboo.Admin.Web.Models.Comment
{
    public class CommentListViewModel 
    {
        public IEnumerable<DAL.Entities.Comment> Comments { get; set; }
        public int TaskId { get; set; }
    }
}