using System;
using Dapper;

namespace GroceryListKeeperUpper.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }
    }
}