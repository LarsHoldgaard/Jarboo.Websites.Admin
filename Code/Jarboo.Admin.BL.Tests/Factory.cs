using FakeItEasy;
using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Microsoft.AspNet.Identity;
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
            IAuth auth = null,
            ICacheService cacheService = null,
            ITaskRegister taskRegister = null,
            IFolderCreator folderCreator = null,
            ITaskStepEmployeeStrategy taskStepEmployeeStrategy = null,
            INotifier notifier = null)
        {
            unitOfWork = unitOfWork ?? A.Fake<IUnitOfWork>();
            auth = auth ?? A.Fake<IAuth>();
            cacheService = cacheService ?? A.Fake<ICacheService>();
            A.CallTo(() => auth.Can(A<string>._, A<string>._)).Returns(true);
            taskRegister = taskRegister ?? A.Fake<ITaskRegister>();
            folderCreator = folderCreator ?? A.Fake<IFolderCreator>();
            taskStepEmployeeStrategy = taskStepEmployeeStrategy ?? A.Fake<ITaskStepEmployeeStrategy>();
            notifier = notifier ?? A.Fake<INotifier>();

            return new TaskService(unitOfWork, auth, cacheService, taskRegister, folderCreator, taskStepEmployeeStrategy, notifier);
        }

        public static EmployeeService CreateEmployeeService(
            IUnitOfWork unitOfWork = null,
            IAuth auth = null,
            ICacheService cacheService = null,
            ITaskRegister taskRegister = null,
            ITaskStepEmployeeStrategy taskStepEmployeeStrategy = null,
            UserManager<User> userManager = null)
        {
            unitOfWork = unitOfWork ?? A.Fake<IUnitOfWork>();
            auth = auth ?? A.Fake<IAuth>();
            cacheService = cacheService ?? A.Fake<ICacheService>();
            A.CallTo(() => auth.Can(A<string>._, A<string>._)).Returns(true);
            taskRegister = taskRegister ?? A.Fake<ITaskRegister>();
            taskStepEmployeeStrategy = taskStepEmployeeStrategy ?? A.Fake<ITaskStepEmployeeStrategy>();
            userManager = userManager ?? A.Fake<UserManager<User>>();

            return new EmployeeService(unitOfWork, auth, cacheService, taskRegister, taskStepEmployeeStrategy, userManager);
        }

        public static ProjectService CreateProjectService(
            IUnitOfWork unitOfWork = null,
            IAuth auth = null,
            ICacheService cacheService = null,
            ITaskRegister taskRegister = null)
        {
            unitOfWork = unitOfWork ?? A.Fake<IUnitOfWork>();
            auth = auth ?? A.Fake<IAuth>();
            cacheService = cacheService ?? A.Fake<ICacheService>();
            A.CallTo(() => auth.Can(A<string>._, A<string>._)).Returns(true);
            taskRegister = taskRegister ?? A.Fake<ITaskRegister>();

            return new ProjectService(unitOfWork, auth, cacheService, taskRegister);
        }

        public static TaskStepEmployeeStrategy CreateStrategy(IUnitOfWork unitOfWork)
        {
            return new TaskStepEmployeeStrategy(unitOfWork);
        }
    }
}
