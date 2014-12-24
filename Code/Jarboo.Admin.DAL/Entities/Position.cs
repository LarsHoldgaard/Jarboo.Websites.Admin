using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    public enum Position
    {
        ProjectLeader,
        Architecture,
        Developer,
        CodeReviewer,
        Tester,
    }

    public class EmployeePosition
    {
        [Key][Column(Order = 0)]
        public int EmployeeId { get; set; }
        [Key][Column(Order = 1)]
        public Position Position { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
