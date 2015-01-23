using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.Integration.GoogleDrive
{
    public interface IGoogleDriveConfiguration
    {
        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
        string GoogleRefreshToken { get; }
        string GoogleLocalUserId { get; }
        string GoogleDrivePath { get; }
        string GoogleDriveTemplatePath { get; }
    }
}
