using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Security;

namespace GroceryListKeeperUpper.Features.Authentication
{
    public class UserIdentity : IUserIdentity
    {
        public int Id
        {
            get { return Convert.ToInt32(this.Claims.First()); }
        }

        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}