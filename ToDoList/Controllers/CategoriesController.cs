using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {

        [HttpGet("/stylists")]
        public ActionResult Index()
        {
            List<Stylist> allStylists = Category.GetAll();
            return View(allCategories);
        }

        [HttpGet("/stylists/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/categories")]
        public ActionResult Create(string categoryName)
        {
            Category newCategory = new Category(categoryName);
            newCategory.Save();
            List<Category> allCategories = Category.GetAll();
            return View("Index", allCategories);
        }

        [HttpGet("/categories/{id}/{sortBy}")]
        public ActionResult SortByDueDate(int id, string sortBy)
        {
            return RedirectToAction("Show", new { id = id, sortBy });
        }

        [HttpGet("/categories/{id}")]
        public ActionResult Show(int id, string sortBy = "")
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Item> categoryItems = selectedCategory.GetItems(sortBy);
            List<Item> allItems = Item.GetAll();
            model.Add("category", selectedCategory);
            // Console.WriteLine("category {0} {1}", selectedCategory.GetId(), selectedCategory.GetName());
            model.Add("categoryItems", categoryItems);
            // Console.WriteLine("items {0}", categoryItems.Count);
            model.Add("allItems", allItems);
            return View(model);
        }

        [HttpPost("/categories/{categoryId}/items/new")]
        public ActionResult AddItem(int categoryId, int itemId)
        {
            Category category = Category.Find(categoryId);
            Item item = Item.Find(itemId);
            category.AddItem(item);
            return RedirectToAction("Show", new { id = categoryId });
        }
    }
}
