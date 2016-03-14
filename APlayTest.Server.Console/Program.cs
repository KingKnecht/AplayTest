﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Server.Factories;
using APlayTest.Services;

namespace APlayTest.Server.Console
{
    public class Program
    {
        public static void Main()
        {
            var server = new APlayServer(Int32.Parse(Properties.Settings.Default.ServerPort), new ProjectManagerFactory(new ProjectManagerService()));
                       
        }
    }
}
