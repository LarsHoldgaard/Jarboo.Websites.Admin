using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Integration;
using Jarboo.Admin.Integration.GoogleDrive;
using Jarboo.Admin.Integration.Mandrill;
using Jarboo.Admin.Integration.Noop;
using Jarboo.Admin.Integration.Trello;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration;

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
            if (Configuration.Instance.UseTrello)
            {
                kernel.Bind<ITrelloConfiguration>().ToConstant(Configuration.Instance);
                kernel.Bind<ITaskRegister>().To<TrelloTaskRegister>().InRequestScope();
            }
            else
            {
                kernel.Bind<ITaskRegister>().To<NoopTaskRegister>().InRequestScope();
            }

            if (Configuration.Instance.UseGoogleDrive)
            {
                kernel.Bind<IGoogleDriveConfiguration>().ToConstant(Configuration.Instance);
                kernel.Bind<IFolderCreator>().To<GoogleDriveFolderCreator>().InRequestScope();
            }
            else
            {
                kernel.Bind<IFolderCreator>().To<NoopFolderCrator>().InRequestScope();
            }

            if (Configuration.Instance.UseNotifier)
            {
                kernel.Bind<IMandrillConfiguration>().ToConstant(Configuration.Instance);
                kernel.Bind<INotifier>().To<MandrillNotifier>().InRequestScope();
            }
            else
            {
                kernel.Bind<INotifier>().To<NoopNotifier>().InRequestScope();
            }

            kernel.Bind<ITaskStepEmployeeStrategy>().To<TaskStepEmployeeStrategy>().InRequestScope();

            kernel.Bind<IUnitOfWork, Context>().To<Context>().InRequestScope();
            kernel.Bind<UserManager<User>>().To<UserManager>().InRequestScope();
            kernel.Bind<RoleManager<UserRole>>().To<RoleManager>().InRequestScope();
            kernel.Bind<IAccountService>().To<AccountService>().InRequestScope();
            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<IProjectService>().To<ProjectService>().InRequestScope();
            kernel.Bind<ITaskService>().To<TaskService>().InRequestScope();
            kernel.Bind<IEmployeeService>().To<EmployeeService>().InRequestScope();
            kernel.Bind<IDocumentationService>().To<DocumentationService>().InRequestScope();
        }
    }
}
