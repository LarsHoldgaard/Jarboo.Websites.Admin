﻿using FakeItEasy;

using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Tests.Services
{
    public class ServicesFactory
    {
        public static TaskService CreateTaskService(
            IUnitOfWork unitOfWork = null,
            ITaskRegister taskRegister = null,
            IFolderCreator folderCreator = null,
            ITaskStepEmployeeStrategy taskStepEmployeeStrategy = null)
        {
            unitOfWork = unitOfWork ?? A.Fake<IUnitOfWork>();
            taskRegister = taskRegister ?? A.Fake<ITaskRegister>();
            folderCreator = folderCreator ?? A.Fake<IFolderCreator>();
            taskStepEmployeeStrategy = taskStepEmployeeStrategy ?? A.Fake<ITaskStepEmployeeStrategy>();

            return new TaskService(unitOfWork, taskRegister, folderCreator, taskStepEmployeeStrategy);
        }

        public static EmployeeService CreateEmployeeService(
            IUnitOfWork unitOfWork = null)
        {
            unitOfWork = unitOfWork ?? A.Fake<IUnitOfWork>();

            return new EmployeeService(unitOfWork);
        }
    }
}