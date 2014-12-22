using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class BusinessErrorCollection : IBusinessErrorCollection
    {
        private readonly ModelStateDictionary _modelState;

        public BusinessErrorCollection(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        public void Add(string property, string error)
        {
            _modelState.AddModelError(property, error);
        }

        public bool HasErrors()
        {
            return _modelState.IsValid;
        }
    }

    public static class BusinessErrorCollectionExtension
    {
        public static IBusinessErrorCollection Wrap(this ModelStateDictionary modelState)
        {
            return new BusinessErrorCollection(modelState);
        }
    }
}