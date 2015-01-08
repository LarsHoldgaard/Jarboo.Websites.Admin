using FakeItEasy;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.BL.ThirdParty;
using Jarboo.Admin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Tests.Services
{
    public class TaskServiceBuilder
    {
        public TaskServiceBuilder()
        {
            unitOfWork = A.Fake<IUnitOfWork>();
            taskRegister = A.Fake<ITaskRegister>();
            folderCreator = A.Fake<IFolderCreator>();
            taskStepEmployeeStrategy = A.Fake<ITaskStepEmployeeStrategy>();
        }

        private IUnitOfWork unitOfWork;
        private ITaskRegister taskRegister;
        private IFolderCreator folderCreator;
        private ITaskStepEmployeeStrategy taskStepEmployeeStrategy;

        public TaskServiceBuilder TaskRegister(ITaskRegister taskRegister)
        {
            this.taskRegister = taskRegister;
            return this;
        }
        public TaskServiceBuilder FolderCreator(IFolderCreator folderCreator)
        {
            this.folderCreator = folderCreator;
            return this;
        }
        public TaskServiceBuilder UnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            return this;
        }
        public TaskServiceBuilder TaskStepEmployeeStrategy(ITaskStepEmployeeStrategy taskStepEmployeeStrategy)
        {
            this.taskStepEmployeeStrategy = taskStepEmployeeStrategy;
            return this;
        }

        public TaskService Build()
        {
            return new TaskService(unitOfWork, taskRegister, folderCreator, taskStepEmployeeStrategy);
        }
    }
}
