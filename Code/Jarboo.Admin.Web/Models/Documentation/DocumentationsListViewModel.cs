using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Documentation
{
    public class DocumentationsListViewModel
    {
        public List<DAL.Entities.Documentation> Documentations { get; set; }
        public bool ShowProject { get; set; }
        public int? ProjectId { get; set; }
    }
}