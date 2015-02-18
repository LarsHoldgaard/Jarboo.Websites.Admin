using System.ComponentModel.DataAnnotations;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.Models.Time
{
    public enum PositionRole
    {
        [Display(Name = "Project Leader")]
        ProjectLeader,
        Architecture,
        Developer,
        [Display(Name = "Code Reviewer")]
        CodeReviewer,
        Tester,
    }

    //public enum TaskStep
    //{
    //    Specification,
    //    Architecture,
    //    Developing,
    //    [Display(Name = "Code Review")]
    //    CodeReview,
    //    Test
    //}

    public class TimeViewModel : DAL.Entities.SpentTime
    {
        public PositionRole Roles { get; set; }
        public string TotalHours { get; set; }

    }
}