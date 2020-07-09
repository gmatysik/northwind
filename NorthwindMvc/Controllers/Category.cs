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
    public class Category : Controller
    {

        private Northwind _db;
        public Category(Northwind db)
        {
            _db = db;
        }

        [Route("Category/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            Packt.Shared.Category category = await _db.Categories.SingleOrDefaultAsync(category => category.CategoryID == id);
            return View(category);
        }        
    }
}