﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.ThirdParty
{
    public interface ITaskStepEmployeeStrategy
    {
        int SelectEmployee(TaskStepEnum step, int projectId);
    }
}
