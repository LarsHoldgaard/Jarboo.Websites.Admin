using System.ComponentModel.DataAnnotations;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.Models.Time
{
    public class TimeViewModel : DAL.Entities.SpentTime
    {
        public Position? Roles { get; set; }
    }
}