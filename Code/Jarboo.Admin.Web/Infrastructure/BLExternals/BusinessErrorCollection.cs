using System.Web.Mvc;

using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Web.Infrastructure.BLExternals
{
    public class BusinessErrorCollection : IBusinessErrorCollection
    {
        private readonly ModelStateDictionary _modelState;

        public BusinessErrorCollection(ModelStateDictionary modelState)
        {
            this._modelState = modelState;
        }

        public void Add(string property, string error)
        {
            this._modelState.AddModelError(property, error);
        }

        public bool HasErrors()
        {
            return !this._modelState.IsValid;
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