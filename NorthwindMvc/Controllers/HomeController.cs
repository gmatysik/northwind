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

namespace NorthwindMvc.Controllers
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private Northwind _db;

        public HomeController(ILogger<HomeController> logger, Northwind db)
        {
            _logger = logger;
            _db = db;
        }

        [IgnoreAntiforgeryToken]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                VisitorCount = (new Random()).Next(1, 1001),
                Categories = await _db.Categories.ToListAsync(),
                Products = await _db.Products.ToListAsync()
            };
            return View(model);
        }

        public IActionResult ProductsThatCostMoreThan(decimal? price)
        {
            if (!price.HasValue)
            {
                return NotFound("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50");
            }

            IEnumerable<Product> model = _db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsEnumerable() // switch to client-side
                .Where(p => p.UnitPrice > price);

            if (model.Count() == 0)
            {
                return NotFound($"No products cost more than {price:C}.");
            }
            ViewData["MaxPrice"] = price.Value.ToString("C");

            return View(model); // pass model to view
        }


        [IgnoreAntiforgeryToken]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductDetail(int? id, int? test, int a)
        {
            if(!id.HasValue)
            {
                return NotFound("Id required, for example /Home/ProductDetail/21");
            }

            var model =_db.Products.SingleOrDefault(p => p.ProductID == id);
            
            if(model == null)
            {
                return NotFound($"Product with id {id} not found");
            }
            
            return View(model);
        }    

        public IActionResult ModelBinding()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ModelBinding(Example example)
        {
            //return View(example);
            var model = new HomeModelBindingViewModel
            {
                Example = example,
                HasErrors = !ModelState.IsValid,
                ValidationErrors = ModelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => error.ErrorMessage)
            };

            return View(model);
        }

        public IActionResult Category(int id)
        {
            return View();
        }
    }
}