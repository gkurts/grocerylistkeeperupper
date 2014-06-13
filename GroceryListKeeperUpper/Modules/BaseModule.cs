using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using GroceryListKeeperUpper.Features.Authentication;
using Nancy;

namespace GroceryListKeeperUpper.Modules
{
    public abstract class BaseModule : NancyModule
    {
        private SqlConnection _connection;
        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                return _connection;
            }
        }

        private UserIdentity _currentUser;
        public UserIdentity CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = Context.CurrentUser as UserIdentity;
                return _currentUser;
            }
        }

        private int _userId;
        public int UserId
        {
            get
            {
                if (_userId == 0)
                    _userId = Convert.ToInt32(Context.CurrentUser.Claims.First());
                return _userId;
            }
        }

        protected BaseModule(string baseuri) : base(baseuri)
        {
        }

        protected BaseModule()
        {
        }

    }
}