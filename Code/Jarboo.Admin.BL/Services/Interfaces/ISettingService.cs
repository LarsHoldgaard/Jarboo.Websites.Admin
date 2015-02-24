using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface ISettingService
    {
        void Edit(SettingEdit model, IBusinessErrorCollection errors);
        Setting GetCurrentSetting();
    }
}
