using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Integration.GoogleDrive;
using Jarboo.Admin.Integration.Mandrill;
using Jarboo.Admin.Integration.Noop;
using Jarboo.Admin.Web.Infrastructure;
using Microsoft.AspNet.Identity;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Jarboo.Admin.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Jarboo.Admin.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Jarboo.Admin.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.DataProtection;
    using Jarboo.Admin.BL.Services.Interfaces;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICacheService>().To<HttpCacheService>().InSingletonScope();
            kernel.Bind<ITaskRegister>().To<NoopTaskRegister>().InRequestScope();

            kernel.Bind<IFolderCreator>().To<GoogleDriveFolderCreator>().InRequestScope();

            kernel.Bind<IUrlConstructor>().To<UrlConstructor>();
            kernel.Bind<INotifier>().To<MandrillNotifierEmailer>().InRequestScope();
            kernel.Bind<IEmailer>().To<MandrillNotifierEmailer>().InRequestScope();
            kernel.Bind<ITaskStepEmployeeStrategy>().To<TaskStepEmployeeStrategy>().InRequestScope();

            kernel.Bind<IUserTokenProvider<User, string>>().ToMethod((x) =>
            {
                var provider = new DpapiDataProtectionProvider("Sample");
                return new DataProtectorTokenProvider<User>(provider.Create("EmailConfirmation"));
            }).InRequestScope();
            kernel.Bind<IAuth>().To<BLAuth>().InRequestScope();
            kernel.Bind<IUnitOfWork, Context>().To<Context>().InRequestScope();
            kernel.Bind<UserManager<User>>().To<UserManager>().InRequestScope();
            kernel.Bind<RoleManager<UserRole>>().To<RoleManager>().InRequestScope();
            kernel.Bind<IAccountService>().To<AccountService>().InRequestScope();
            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<IProjectService>().To<ProjectService>().InRequestScope();
            kernel.Bind<ITaskService>().To<TaskService>().InRequestScope();
            kernel.Bind<IEmployeeService>().To<EmployeeService>().InRequestScope();
            kernel.Bind<IDocumentationService>().To<DocumentationService>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            kernel.Bind<ISpentTimeService>().To<SpentTimeService>().InRequestScope();
            kernel.Bind<IQuizService>().To<QuizService>().InRequestScope();
            kernel.Bind<ICommentService>().To<CommentService>().InRequestScope();
            kernel.Bind<IQuestionService>().To<QuestionService>().InRequestScope();
            kernel.Bind<IAnswerService>().To<AnswerService>().InRequestScope();
            kernel.Bind<ISettingService>().To<SettingService>().InRequestScope();
        }
    }
}
