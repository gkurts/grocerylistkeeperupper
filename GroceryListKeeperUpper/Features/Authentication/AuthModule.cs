using System.Collections.Generic;
using System.Linq;
using Dapper;
using GroceryListKeeperUpper.Models;
using GroceryListKeeperUpper.Modules;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.ModelBinding;

namespace GroceryListKeeperUpper.Features.Authentication
{
    public class AuthModule : BaseModule
    {
        public AuthModule(ITokenizer tokenizer) : base("/api/v1/auth")
        {
            Post["/"] = p =>
            {
                LoginModel model = this.Bind<LoginModel>();

                UserIdentity useridentity;

                using (var cnn = Connection)
                {
                    User user = cnn.Query<User>(
                        "select * from users where email = @username",
                        new {username = model.Username}).FirstOrDefault();

                    if (user == null)
                        return HttpStatusCode.Unauthorized;


                    if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                        return HttpStatusCode.Unauthorized;

                    useridentity = new UserIdentity
                    {
                        UserName = model.Username,
                        Claims = new List<string> { user.Id.ToString(), user.Email == "gkurts@gregkurts.com" ? "Admin" : "User" }
                    };
                }

                if (useridentity != null)
                {
                    var token = tokenizer.Tokenize(useridentity, Context);

                    return Response.AsJson(new { token });
                }

                return HttpStatusCode.Unauthorized;
            };
        }
    }
}