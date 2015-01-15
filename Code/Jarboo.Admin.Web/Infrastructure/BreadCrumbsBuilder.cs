using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class BreadCrumbsBuilder
    {
        private StringBuilder stringBuilder;

        public BreadCrumbsBuilder()
        {
            stringBuilder = new StringBuilder();
            stringBuilder.Append("");
        }

        public BreadCrumbsBuilder Add(string url, string label)
        {
            stringBuilder.Append(string.Format(@"<li>
                    <strong>
                        <a href='{0}'>{1}</a>
                    </strong>
                </li>", url, label));
            return this;
        }
        public BreadCrumbsBuilder Add(string label)
        {
            stringBuilder.Append(string.Format("<li>{0}</li>", label));
            return this;
        }

        public MvcHtmlString Done()
        {
            stringBuilder.Append("");
            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        public static BreadCrumbsBuilder Start()
        {
            return new BreadCrumbsBuilder();
        }
    }
}