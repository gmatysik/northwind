using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NorthwindMvc.Models;
using Microsoft.AspNetCore.Authorization;
using  Packt.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;

namespace NorthwindMvc.Controllers
{
    public class CustomerController : Controller
    {
        private Northwind _db;
        private IHttpClientFactory _clientFactory;

        public CustomerController(Northwind db, IHttpClientFactory clientFactory)
        {
            this._db = db;
            this._clientFactory = clientFactory;
        }

        [Route("Customer/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            //Packt.Shared.Category category = await _db.Categories.SingleOrDefaultAsync(category => category.CategoryID == id);
            //return View(category);
            return View();
        }

        public async Task<IActionResult> FindCustomer(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound("You must pass name");
            }

            var client = _clientFactory.CreateClient("NorthwindWS");
            string uri = $"api/customers/companyName/{name}";
            var request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: uri);
            
            var response = await client.SendAsync(request);

             string jsonString = await response.Content.ReadAsStringAsync();

            IList<Customer> model = JsonConvert
                .DeserializeObject<IList<Customer>>(jsonString);

            var mod = new CustomersViewModel();
            mod.Customers = model;

            return View("Index", mod);

        }

    }
}