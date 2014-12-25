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
            stringBuilder.Append("<div class='breadcrumbs small'>");
        }

        public BreadCrumbsBuilder Add(string url, string label)
        {
            stringBuilder.Append(string.Format(@"<span class='item'>
                    <strong>
                        <a href='{0}'>{1}</a>
                    </strong>
                </span>", url, label));
            return this;
        }
        public BreadCrumbsBuilder Add(string label)
        {
            stringBuilder.Append(string.Format("<span class='item'>{0}</span>", label));
            return this;
        }

        public MvcHtmlString Done()
        {
            stringBuilder.Append("</div>");
            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        public static BreadCrumbsBuilder Start()
        {
            return new BreadCrumbsBuilder();
        }
    }
}