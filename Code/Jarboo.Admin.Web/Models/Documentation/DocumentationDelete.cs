using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Documentation
{
    public class DocumentationDelete
    {
        public int DocumentationId { get; set; }
        public int? ProjectId { get; set; }
    }
}