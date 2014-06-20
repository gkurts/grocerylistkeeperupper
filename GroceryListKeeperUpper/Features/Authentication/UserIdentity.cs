using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Nancy.Security;

namespace GroceryListKeeperUpper.Features.Authentication
{
    public class UserIdentity : IUserIdentity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}