using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.UI.WebControls.WebParts;
using Dapper;
using GroceryListKeeperUpper.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Security;

namespace GroceryListKeeperUpper.Modules
{
    public class ListModule : BaseModule
    {
        public ListModule() : base("/api/v1/lists")
        {
            this.RequiresAuthentication();

            //get all lists
            Get["/"] = p =>
            {
                using (var cnn = Connection)
                {
                    return Response.AsJson(cnn.GetList<List>(new {owner = UserId}));
                }
            };

            Delete["/{id:int}"] = p =>
            {
                int listId = int.Parse(p.id);

                using (var cnn = Connection)
                {
                    var sql = @"delete from Items where ListId = @id delete from Lists where Id = @id and Owner = @owner";
                    cnn.QueryMultiple(sql, new {id = listId, owner = UserId});
                }

                return HttpStatusCode.OK;
            };

            //create list
            Post["/Create"] = p =>
            {
                ListModel model = this.Bind<ListModel>();
                List list = new List
                {
                    Title = model.Title,
                    Owner = UserId
                };

                using (var cnn = Connection)
                {
                    int id =cnn.Query<int>(
                            "insert into Lists (Title, owner) values (@title, @owner); select cast(scope_identity() as int)",
                            new {title = list.Title, owner = list.Owner}).First();
                    list.Id = id;
                }

                return Response.AsJson(list);
            };

            //get list by id
            Get["/{id:int}"] = p =>
            {
                int listId = int.Parse(p.id);

                using (var cnn = Connection)
                {
                    var list = cnn.Query<List>("select * from Lists where Id = @id and owner = @owner",
                        new {id = listId, owner = UserId}).First();
                    var items = cnn.GetList<Item>(new {ListId = listId, owner = UserId});

                    var ItemList = new
                    {
                        List = list,
                        Items = items
                    };

                    return Response.AsJson(ItemList);

                }
            };

            Put["/{listid:int}/item/{itemid:int}"] = p =>
            {
                UpdateItemModel model = this.Bind<UpdateItemModel>();

                using (var cnn = Connection)
                {
                    var item = cnn.Query<Item>("select * from Items where Id = @id and owner = @owner",
                        new {id = model.Id, owner = UserId}).FirstOrDefault();
                    item.Title = model.Title;
                    item.Qty = model.Qty;
                    item.Got = model.Got;

                    cnn.Update(item);

                    return Response.AsJson(item);
                }
            };

            //add item to list by id
            Post["/{id:int}/Items"] = p =>
            {
                AddItemModel model = this.Bind<AddItemModel>();
                int listId = int.Parse(p.id);

                Item item = new Item
                {
                    Created = DateTime.UtcNow,
                    Got = false,
                    Qty = model.Qty,
                    Title = model.Title,
                    ListId = listId,
                    Owner = UserId
                };

                using (var cnn = Connection)
                {
                    int id = cnn.Query<int>(
                        "insert into Items (qty, created, got, listid, title, owner) values (@qty, @created, @got, @listid, @title, @owner)" +
                        "select cast(scope_identity() as int)",
                        new
                        {
                            qty = item.Qty,
                            created = item.Created,
                            got = item.Got,
                            listid = item.ListId,
                            title = item.Title,
                            owner = item.Owner
                        }).First();

                    item.Id = id;

                }

                return Response.AsJson(item);
            };

            Delete["/{listid:int}/Items/{itemid:int}"] = p =>
            {

                int itemid = int.Parse(p.itemid);

                using (var cnn = Connection)
                {
                    var res = cnn.Execute("delete from Items where Id = @id and owner = @owner",
                        new {id = itemid, owner = UserId});
                }
                return HttpStatusCode.OK;
            };



        }
    }

    public class ListResponse
    {
        public string ErrorMessage { get; set; }
        public List List { get; set; }
    }

    public class ListModel
    {
        public string Title { get; set; }
    }

    public class AddItemModel
    {
        public string Title { get; set; }
        public int Qty { get; set; }
    }

    public class UpdateItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Qty { get; set; }
        public bool Got { get; set; }
    }

    public class DeleteListModel
    {
        public int ListId { get; set; }
    }

    public class DeleteItemModel
    {
        public int ListId { get; set; }
        public int ItemId { get; set; }
    }

    public class UpdateQuantityModel
    {
        public int ListId { get; set; }
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }
}