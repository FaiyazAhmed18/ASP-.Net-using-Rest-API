using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAPI1.Models;

namespace WebAPI1.Areas.Admin.Controllers
{
    public class SetupController : Controller
    {
        // GET: Admin/Setup
        private string baseUrl = "https://localhost:44358/"; 
        public async Task<ActionResult> Index()
        {
            List<Product> products = new List<Product>();
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl); 
                client.DefaultRequestHeaders.Clear(); 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                HttpResponseMessage res = await client.GetAsync("api/Product/GetAll"); // need to understand
                if(res.IsSuccessStatusCode)
                {
                    var productList = res.Content.ReadAsStringAsync().Result; //all the data are combnined to get a Json type string product
                    products = JsonConvert.DeserializeObject<List<Product>>(productList); //the string above line are deserialised in a list here
                }
            } 
            return View(products);
        }

        public ActionResult ProductEntry()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ProductEntry(Product product)
        {
            string message = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.PostAsJsonAsync<Product>("api/Product/Add",product); // need to understand
                if (res.IsSuccessStatusCode)
                {
                    message = "Save data successfully";
                }
                else
                {
                    message = "Save failed";
                }
                ViewBag.message = message;
            }

            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            if(id<0||id==0)
            {
                return RedirectToAction("Index");
            }
            Product product = new Product();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/Product/GetById?id="+id); // need to understand
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync().Result; //all the data are combnined to get a Json type string product
                    product = JsonConvert.DeserializeObject<Product>(data); //the string above line are deserialised in a list here
                }
            }

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Product product)
        {
            string message = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.PutAsJsonAsync<Product>("api/Product/Update", product); // need to understand
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    message = "Update failed";
                }
                ViewBag.message = message;
            }

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int productId)
        {
            if (productId < 0 || productId == 0)
            {
                return RedirectToAction("Index");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.DeleteAsync("api/Product/Delete?id=" + productId); // need to understand
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}