﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

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

        public static RouteData ParentRouteData(this ViewContext context)
        {
            while (context.ParentActionViewContext != null)
            {
                context = context.ParentActionViewContext;
            }

            return context.RouteData;
        }

        public static MvcForm BeginFormX(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, FormMethod method, object htmlAttributes)
        {
            return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, routeValues, method, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

    }
}