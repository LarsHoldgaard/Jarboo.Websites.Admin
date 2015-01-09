using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Jarboo.Admin.BL.Tests
{
    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public void SetupFixture()
        {
            AutoMapperConfig.RegisterMappers();
        }
    }
}
