using FakeItEasy;

using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Tests
{
    public class Factory
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
            IUnitOfWork unitOfWork = null,
            ITaskRegister taskRegister = null,
            ITaskStepEmployeeStrategy taskStepEmployeeStrategy = null)
        {
            unitOfWork = unitOfWork ?? A.Fake<IUnitOfWork>();
            taskRegister = taskRegister ?? A.Fake<ITaskRegister>();
            taskStepEmployeeStrategy = taskStepEmployeeStrategy ?? A.Fake<ITaskStepEmployeeStrategy>();

            return new EmployeeService(unitOfWork, taskRegister, taskStepEmployeeStrategy);
        }

        public static ProjectService CreateProjectService(
            IUnitOfWork unitOfWork = null,
            ITaskRegister taskRegister = null)
        {
            unitOfWork = unitOfWork ?? A.Fake<IUnitOfWork>();
            taskRegister = taskRegister ?? A.Fake<ITaskRegister>();

            return new ProjectService(unitOfWork, taskRegister);
        }

        public static TaskStepEmployeeStrategy CreateStrategy(IUnitOfWork unitOfWork)
        {
            return new TaskStepEmployeeStrategy(unitOfWork);
        }
    }
}
