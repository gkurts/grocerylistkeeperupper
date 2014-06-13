using System;
using Dapper;
using GroceryListKeeperUpper.Models;
using GroceryListKeeperUpper.Modules;
using Nancy;
using Nancy.ModelBinding;

namespace GroceryListKeeperUpper.Features.Authentication
{
    public class UserModule : BaseModule
    {
        public UserModule() : base("/api/v1/users")
        {
            Post["/"] = p =>
            {
                User model = this.Bind<User>();
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                model.Created = DateTime.UtcNow;
                model.LastLogin = new DateTime(1977, 9, 27, 0, 0, 0);
                
                using (var cnn = Connection)
                {
                    cnn.Insert(model);
                }

                return HttpStatusCode.OK;
            };
        }
    }
}