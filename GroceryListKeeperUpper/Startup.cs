using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using GroceryListKeeperUpper.Features.Authentication;
using GroceryListKeeperUpper.Models;
using JWT;
using Microsoft.Owin.Extensions;
using Owin;
using Owin.StatelessAuth;

namespace GroceryListKeeperUpper
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app
                .RequiresStatelessAuth(new MySecureTokenValidator(new ConfigProvider()),
                    new StatelessAuthOptions() {IgnorePaths = new List<string>{"/api/v1/auth"}})
                .UseNancy();
        }
    }

    public class MySecureTokenValidator : ITokenValidator
    {
        private readonly IConfigProvider configProvider;

        public MySecureTokenValidator(IConfigProvider configProvider)
        {
            this.configProvider = configProvider;
        }

        public ClaimsPrincipal ValidateUser(string token)
        {
            Trace.WriteLine("checking token");
            try
            {
                //Claims don't deserialize :(
                //var jwttoken = JsonWebToken.DecodeToObject<JwtToken>(token, configProvider.GetAppSetting("securekey"));

                var decodedtoken = JsonWebToken.DecodeToObject(token, configProvider.GetAppSetting("secret")) as Dictionary<string, object>;
                Trace.WriteLine(decodedtoken);

                var jwttoken = new JwtToken()
                {
                    Expiry = (DateTime)decodedtoken["Expiry"],
                    UserId = (int)decodedtoken["UserId"]
                };

                if (decodedtoken.ContainsKey("Claims"))
                {
                    var claims = new List<Claim>();

                    for (int i = 0; i < ((ArrayList)decodedtoken["Claims"]).Count; i++)
                    {
                        var type = ((Dictionary<string, object>)((ArrayList)decodedtoken["Claims"])[i])["Type"].ToString();
                        var value = ((Dictionary<string, object>)((ArrayList)decodedtoken["Claims"])[i])["Value"].ToString();
                        claims.Add(new Claim(type, value));
                    }
                    claims.Add(new Claim("UserId",  jwttoken.UserId.ToString()));
                    jwttoken.Claims = claims;
                }

                Trace.WriteLine(jwttoken.Expiry);
                if (jwttoken.Expiry < DateTime.UtcNow)
                {
                    Trace.WriteLine("expired token");
                    return null;
                }

                return new ClaimsPrincipal(new ClaimsIdentity(jwttoken.Claims, "Token"));
            }
            catch (SignatureVerificationException)
            {
                Trace.WriteLine("signature verification failed");
                return null;
            }
        }
    }


}