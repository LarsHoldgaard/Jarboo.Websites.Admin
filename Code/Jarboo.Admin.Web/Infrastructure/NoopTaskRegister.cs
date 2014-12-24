﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.ThirdParty;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class NoopTaskRegister : ITaskRegister
    {
        public void Register(string customerName, string taskTitle)
        { }

        public void Unregister(string customerName, string taskTitle)
        { }
    }
}