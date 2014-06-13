using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Dapper;

namespace GroceryListKeeperUpper.Models
{
    [Table("Lists")]
    public class List
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Owner { get; set; }
    }
}