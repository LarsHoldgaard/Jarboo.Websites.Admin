using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FakeItEasy;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Tests;

using NUnit.Framework;

namespace Jarboo.Admin.BL.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        private ProjectEdit ValidProjectEdit(DAL.IUnitOfWork context)
        {
            return new ProjectEdit()
                       {
                           Name = "Project",
                           CustomerId = context.EnsureCustomer().CustomerId
                       };
        }
    }
}
