using System;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Reactive.Linq;
using Itera.RxDemo.Models;
using SignalR.Reactive;


[assembly: OwinStartup("StartUp",typeof(Itera.RxDemo.StartUp))]
namespace Itera.RxDemo
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            new Configuration().EnableRxSupport(app);

            var timer = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => DateTime.Now.ToLongTimeString())
                .ToClientside().Observable<RxHub>("Time");


        }
    }
}
