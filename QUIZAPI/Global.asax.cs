using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using QuizAPI.TokenManager;

namespace QuizAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // GlobalConfiguration.Configuration.MessageHandlers.Add(new APIKeyMessageHandler());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new ValidateTokenHandler());
        }
    }
}
