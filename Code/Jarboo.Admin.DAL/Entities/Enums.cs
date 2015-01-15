using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    public static class Enums
    {
        public static Position GetPosition(this TaskStepEnum step)
        {
            switch (step)
            {
                case TaskStepEnum.Specification:
                    return Position.ProjectLeader;
                case TaskStepEnum.Architecture:
                    return Position.Architecture;
                case TaskStepEnum.Developing:
                    return Position.Developer;
                case TaskStepEnum.CodeReview:
                    return Position.CodeReviewer;
                case TaskStepEnum.Test:
                    return Position.Tester;
                default:
                    throw new Exception("Unknown task step: " + step);
            }
        }

        public static string GetLetter(this TaskType type)
        {
            switch (type)
            {
                case TaskType.Bug:
                    return "B";
                case TaskType.Feature:
                    return "F";
                case TaskType.Project:
                    return "P";
                default:
                    throw new Exception("Unnknown task type " + type);
            }
        }
    }
}
