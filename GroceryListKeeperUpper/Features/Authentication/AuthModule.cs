using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Dapper;
using GroceryListKeeperUpper.Models;
using GroceryListKeeperUpper.Modules;
using JWT;
using Nancy;
using Nancy.ModelBinding;

namespace GroceryListKeeperUpper.Features.Authentication
{
    public class AuthModule : BaseModule
    {
        public AuthModule(IConfigProvider config) : base("/api/v1/auth")
        {
            Post["/"] = p =>
            {
                LoginModel model = this.Bind<LoginModel>();

                User user = null;

                using (var cnn = Connection)
                {
                    user = cnn.Query<User>(
                        "select * from users where email = @username",
                        new {username = model.Username}).FirstOrDefault();
                }

                if (user == null)
                    return HttpStatusCode.Unauthorized;

                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                    return HttpStatusCode.Unauthorized;

                var jwtToken = new JwtToken
                {
                    Expiry = DateTime.UtcNow.AddDays(9999),
                    UserId = user.Id
                };

                jwtToken.Claims.Add(user.Email == "gkurts@gregkurts.com"
                    ? new Claim(ClaimTypes.Role, "Administrator")
                    : new Claim(ClaimTypes.Role, "User"));

                jwtToken.Claims.Add(new Claim(ClaimTypes.Email, user.Email));
                jwtToken.Claims.Add(new Claim(ClaimTypes.Name, user.Email));
                jwtToken.Claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));

                var token = JsonWebToken.Encode(jwtToken, config.GetAppSetting("secret"), JwtHashAlgorithm.HS256);

                return Response.AsJson(token);


            };
        }
    }

    public class JwtToken
    {
        public JwtToken()
        {
            Claims = new List<Claim>();
        }
        public List<Claim> Claims { get; set; }
        public DateTime Expiry { get; set; }
        public int UserId { get; set; }
    }
}