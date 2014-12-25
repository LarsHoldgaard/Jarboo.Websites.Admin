using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Jarboo.Admin.BL.External;

namespace Jarboo.Admin.BL
{
    public static class Extensions
    {
        public static bool Validate(this object obj, IBusinessErrorCollection errors)
        {
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, context, results);
            if (isValid)
            {
                return true;
            }

            foreach (var validationResult in results)
            {
                errors.Add(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            return false;
        }
        public static TTo MapTo<TFrom, TTo>(this TFrom from, TTo to = null)
            where TFrom : class, new()
            where TTo : class, new()
        {
            return Mapper.Map(from ?? new TFrom(), to ?? new TTo());
        }
        public static TTo MapTo<TTo>(this object from)
            where TTo : class
        {
            return Mapper.Map(from, from.GetType(), typeof(TTo)) as TTo;
        }
        public static IEnumerable<TTo> MapTo<TFrom, TTo>(this IEnumerable<TFrom> from)
            where TFrom : class, new()
            where TTo : class, new()
        {
            return from.Select(x => x.MapTo<TFrom, TTo>());
        }
    }
}
