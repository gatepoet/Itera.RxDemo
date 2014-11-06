using System;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup("EnableRxSupport", typeof(SignalR.Reactive.Configuration), "EnableRxSupport")]
namespace SignalR.Reactive
{
    public class Configuration
    {
        public void EnableRxSupport(IAppBuilder app)
        {
            DependencyResolverContext.Instance = GlobalHost.DependencyResolver;
            
            if (DependencyResolverContext.Instance == null)
                throw new InvalidOperationException("DependenyResolver must be set to an instance of IDependencyResolver");

            DependencyResolverContext.Instance.EnableRxSupport();
            //ToDo 
            var config = new HubConfiguration
                {
                    EnableDetailedErrors = true
                };

            app.MapSignalR();
            //AspNetBootstrapper.Initialize();

        }
    }
}
