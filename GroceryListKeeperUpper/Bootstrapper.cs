using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Authentication.Token.Storage;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace GroceryListKeeperUpper
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            StaticConfiguration.DisableErrorTraces = false;

            container.Register(typeof (JsonSerializer), typeof (CustomJsonSerializer));
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);


            Tokenizer t = new Tokenizer(configurator =>
            {
                configurator.TokenExpiration(() => TimeSpan.FromDays(5));
                configurator.KeyExpiration(() => TimeSpan.FromDays(10));
            });
            
            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(t));

        }

    }
}