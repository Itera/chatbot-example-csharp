using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Alfred.Dialogs;
using Alfred.Models;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;

namespace Alfred
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
