using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Services.Search.Controllers;
using DotNetNuke.Services.Search.Entities;
using DotNetNuke.Web.Api;
using DotNetNuke.Security;
using DotNetNuclear.Modules.NextGenModule.Components;
using DotNetNuclear.Modules.NextGenModule.Data;
using System.Collections.Generic;

namespace DotNetNuclear.Modules.NextGenModule.Services.Controllers
{
    [SupportedModules(FeatureController.DESKTOPMODULE_NAME)]
    public class ItemSvcController : DnnApiController
    {
        IItemRepository _itemRepository;

        public ItemSvcController()
        {
            _itemRepository = new ItemRepository();
        }

        /// <summary>
        /// API that returns Hello world
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpGet]  //[baseURL]/itemsvc/test
        [ActionName("test")]
        [AllowAnonymous]
        public HttpResponseMessage HelloWorld()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World!");
        }

        /// <summary>
        /// API that creates a new item in the item list
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/itemsvc/new
        [ValidateAntiForgeryToken]
        [ActionName("new")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage AddItem(RequestItem item)
        {
            try
            {
                Item newItem = new Item();
                newItem.ModuleId = ActiveModule.ModuleID;
                newItem.CreatedByUserId = UserInfo.UserID;
                newItem.CreatedOnDate = DateTime.Now;
                newItem.LastModifiedByUserId = UserInfo.UserID;
                newItem.LastModifiedOnDate = DateTime.Now;
                newItem.AssignedUserId = item.AssignedUserId;
                newItem.ItemName = item.ItemName;
                newItem.ItemDescription = item.ItemDescription;
                int itemId = _itemRepository.CreateItem(newItem);
                return Request.CreateResponse(HttpStatusCode.OK, itemId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that deletes an item from the item list
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/itemsvc/delete
        [ValidateAntiForgeryToken]
        [ActionName("delete")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage DeleteItem(RequestItem delItem)
        {
            try
            {
                _itemRepository.DeleteItem(delItem.ItemId, ActiveModule.ModuleID);
                return Request.CreateResponse(HttpStatusCode.OK, true.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        /// <summary>
        /// API that creates a new item in the item list
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/itemsvc/edit
        [ValidateAntiForgeryToken]
        [ActionName("edit")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage UpdateItem(Item item)
        {
            try
            {
                item.LastModifiedByUserId = UserInfo.UserID;
                item.LastModifiedOnDate = DateTime.Now;
                _itemRepository.UpdateItem(item);
                return Request.CreateResponse(HttpStatusCode.OK, true.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that returns an item list
        /// </summary>
        /// <returns></returns>
        [HttpPost,HttpGet]  //[baseURL]/itemsvc/list
        [ValidateAntiForgeryToken]
        [ActionName("list")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage GetModuleItems()
        {
            try
            {
                var itemList = _itemRepository.GetItems(ActiveModule.ModuleID);
                return Request.CreateResponse(HttpStatusCode.OK, itemList.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that returns a single item
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpGet]  //[baseURL]/itemsvc/byid
        [ValidateAntiForgeryToken]
        [ActionName("byid")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage GetItem(RequestById itemReq)
        {
            try
            {
                var item = _itemRepository.GetItem(itemReq.ItemId, ActiveModule.ModuleID);
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that searches for items
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpGet]  //[baseURL]/itemsvc/search
        [ValidateAntiForgeryToken]
        [ActionName("search")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage Search(SearchRequest request)
        {
            try
            {
                // Search lucene for only my module's search data
                SearchQuery query = new SearchQuery
                {
                    ModuleId = request.ModuleId,
                    PageSize = request.PageSize,
                    PageIndex = (request.PageNum > 0 ? request.PageNum : 1),
                    SortField = SortFields.Relevance,
                    SortDirection = SortDirections.Descending,
                    KeyWords = request.Term
                };
                SearchResults sr = SearchController.Instance.ModuleSearch(query);

                // Convert each search result to an Item
                List<Item> itemList = new List<Item>();
                foreach (SearchResult r in sr.Results)
                {
                    // Add module result if it is an item (items have a search doc description)
                    if (!String.IsNullOrEmpty(r.Description))
                        itemList.Add(FeatureController.ConvertSearchDocToItem(r));    
                }

                return Request.CreateResponse(HttpStatusCode.OK, itemList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }

    public class RequestItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int AssignedUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedCreatedOnDate { get; set; }
    }

    public class RequestById
    {
        public int ItemId { get; set; }
    }

    public class SearchRequest
    {
        public string Term { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public int ModuleId { get; set; }
    }

}