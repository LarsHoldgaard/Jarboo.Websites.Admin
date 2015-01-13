using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

using NUnit.Framework;

namespace Jarboo.Admin.DAL.Tests.Entities
{
    [TestFixture]
    public class EnumsTests
    {
        [Test]
        public void GetPosition_WhenUnknownStep_Throws()
        {
            Assert.Throws<Exception>(() => ((TaskStepEnum)(-1)).GetPosition());
        }

        [Test]
        public void GetPosition_KnowsAllSteps()
        {
            var values = Enum.GetValues(typeof(TaskStepEnum));
            foreach (var value in values)
            {
                var en = (TaskStepEnum)value;
                en.GetPosition();
            }
        }

        [Test]
        public void GetLetter_WhenUnknownType_Throws()
        {
            Assert.Throws<Exception>(() => ((TaskType)(-1)).GetLetter());
        }

        [Test]
        public void GetLetter_KnowsAllTypes()
        {
            var values = Enum.GetValues(typeof(TaskType));
            foreach (var value in values)
            {
                var en = (TaskType)value;
                en.GetLetter();
            }
        }
    }
}
