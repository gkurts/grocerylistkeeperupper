using System;
using Dapper;

namespace GroceryListKeeperUpper.Models
{
    [Table("Items")]
    public class Item
    {
        public Item()
        {
            Created = DateTime.UtcNow;
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public int Qty { get; set; }
        public DateTime Created { get; set; }
        public bool Got { get; set; }
        public int ListId { get; set; }
        public int Owner { get; set; }
    }
}