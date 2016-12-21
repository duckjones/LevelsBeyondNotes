using LevelsBeyondNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace LevelsBeyondNotes
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            if(!NotesRepo.CreateDbIfNotExists())
            { 
                // Email the Sys Admin and let him/her know the database is offline and their day is about to get worse
            }
        }
    }
}
