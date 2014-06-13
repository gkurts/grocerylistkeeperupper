using System.Data.Common;
using System.Web;
using Nancy;

namespace GroceryListKeeperUpper.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };
        }
    }
}