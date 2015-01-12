using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public enum DocumentationType
    {
        SetupGuide,
        CodingStandard,
        ProjectInfo 
    }

    public class Documentation : BaseEntity
    {
        public int DocumentationId { get; set; }
        [Required]
        public string DocumentLink { get; set; }
        public DocumentationType Type { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
