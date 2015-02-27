namespace Jarboo.Admin.DAL.Entities
{
    public class Setting : BaseEntity
    {
        public int Id { get; set; }
        public string Configuration { get; set; }

        public string JarbooInfoEmail { get; set; }

        #region Google Drive
        public bool UseGoogleDrive { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string GoogleRefreshToken { get; set; }
        public string GoogleLocalUserId { get; set; }

        public string GoogleTemplatePath { get; set; }
        public string GoogleBasePath { get; set; }
        #endregion

        #region Mandrill
        public bool UseMandrill { get; set; }
        public string MandrillApiKey { get; set; }
        public string MandrillFrom { get; set; }
        public string MandrillPasswordRecoveryTemplate { get; set; }
        public string MandrillNewTaskTemplate { get; set; }
        public string MandrillNewEmployeeTemplate { get; set; }
        public string MandrillTaskResponsibleChangedNotificationSubject { get; set; }
        public string MandrillTaskResponsibleNotificationTemplate { get; set; }
        #endregion
    }
}
