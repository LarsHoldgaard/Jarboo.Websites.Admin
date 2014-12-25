using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString EnumListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (!metaData.ModelType.IsGenericType)
            {
                throw new Exception("Expected generic type");
            }
            if (metaData.ModelType.GenericTypeArguments.Length != 1)
            {
                throw new Exception("Expected only one generic argument");
            }

            var selectList = EnumHelper.GetSelectList(metaData.ModelType.GenericTypeArguments[0]);

            return htmlHelper.ListBoxFor(expression, selectList, htmlAttributes);
        }
    }
}