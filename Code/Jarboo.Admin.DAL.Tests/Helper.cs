using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Tests
{
    public static class Helper
    {
        public static void Suppress(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        public static TaskStepEnum LastStep()
        {
            var step = TaskStepEnum.Specification;
            while (true)
            {
                var next = TaskStep.Next(step);
                if (!next.HasValue)
                {
                    return step;
                }
                step = next.Value;
            }
        }
    }
}
