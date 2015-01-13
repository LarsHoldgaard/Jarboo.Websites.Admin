using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jarboo.Admin.DAL.Entities;

using NUnit.Framework;

namespace Jarboo.Admin.DAL.Tests.Entities
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void TaskTitleWithType_ReturnsCorrect()
        {
            var title = Task.TaskTitleWithType("title", TaskType.Bug);

            Assert.AreEqual("B_title", title);
        }
    }
}
