using ActiveMenu.Model;
using ActiveMenu.ViewModel.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using ActiveMenu.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace ActiveMenu.Controllers
{
    public class appController : ApiController
    {


        [System.Web.Http.HttpPost]
        public async Task<JObject> foodDetail([FromBody] FDVM model)
        {
            string result = "";
            using (Context context = new Context())
            {
                Guid id = new Guid(model.ID);
                itemDetail rst = context.items.Where(x => x.itemID == id).Select(x=> new itemDetail {calory = x.calory,  itemID = x.itemID.ToString(), categoryID = x.categoryID.ToString(), cookTime = "10", description = x.desctiption, imageList = ConfigurationManager.AppSettings["domain"] + "images/items/" + ConfigurationManager.AppSettings["name"] + "/" + x.imageList,  prepTime = "10", price = x.price, servingTime = "10", title = x.title  , ingredient = x.ingredient, orderDetails = "", arModel = ""  }).First();
                Guid ctID = new Guid(rst.categoryID);
                List<suggestedItemsList> lst = await context.items.Where(x => x.categoryID == ctID).Select(x => new suggestedItemsList { cookTime = "1", imageList = ConfigurationManager.AppSettings["domain"] + "images/items/" + ConfigurationManager.AppSettings["name"] + "/" + x.imageList, itemID = x.itemID.ToString(), prepTime = "10", description = x.desctiption, price = x.price, title = x.title, servingTime = "0" }).ToListAsync();
                foodDetailVM resultmodel = new foodDetailVM();
                resultmodel.itemDetail = rst;
                resultmodel.suggestedItemsList = lst;
                result = JsonConvert.SerializeObject(resultmodel);
               
            }
            JObject jObject = JObject.Parse(result);
            return jObject;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> Main([FromBody] getMainVM model)
        {
            Guid ddd = Guid.NewGuid();//cadc9a63-9c8a-4034-a7ff-907b8bf73aff -- d53f8f88-fa0e-415e-aa37-cea998a8e5f8
            string result = "";
            using (Context context = new Context())
            {
                Guid id = new Guid(model.gu);
                restaurant rst = context.restaurants.SingleOrDefault(x => x.restaurantID == id);
                
                List<category> lst = await context.categories.Where(x => x.restaurantID == id).OrderByDescending(x=>x.priority).Select(x=>x).ToListAsync();
                
                List<CategoryVM> lstFinal = new List<CategoryVM>();
               
                foreach (var item in lst)
                {
                    List<Item> itemList = await context.items.Where(x => x.categoryID == item.categoryID).ToListAsync();
                    List<ItemVM> itemFinal = itemList.Select(x => new ItemVM { id = x.itemID, image = ConfigurationManager.AppSettings["domain"] + "images/items/" + ConfigurationManager.AppSettings["name"] + "/"  + x.imageList.Split(',').First() ,title =x.title, desc = x.desctiption, price = (int)x.price }).ToList();
                    CategoryVM catf = new CategoryVM()
                    {
                        title = item.title,
                        id = item.categoryID,
                        isVisible = item.isVisible,
                        image = ConfigurationManager.AppSettings["domain"]+"images/Categories/" + ConfigurationManager.AppSettings["name"]+"/" + item.imageAddress.Split(',')[0],
                        foodList = itemFinal,
                        icon = ""
                    };
                    lstFinal.Add(catf);
                };

                appindexVM modeltoSend = new appindexVM()
                {
                    data = lstFinal,
                    image = "http://menumotive.com/" + rst.imageAddress,
                    title = rst.title,
                    garson = rst.garson
                };

                result = JsonConvert.SerializeObject(modeltoSend);
            }
           

            JObject jObject = JObject.Parse(result);
            return jObject;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> setOrder([FromBody] setOrderVM model)
        {
            string result = "";
            JObject jObject = JObject.Parse(result);
            return jObject;
        }
    }
}
