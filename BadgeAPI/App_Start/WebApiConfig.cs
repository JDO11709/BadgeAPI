
using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;

//[assembly: WebActivator.PreApplicationStartMethod(typeof(BadgeAPI.WebApiConfig), "Start")]

namespace BadgeAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Attribute rounting
            config.MapHttpAttributeRoutes();

        }
    }
}