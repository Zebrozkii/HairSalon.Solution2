using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;

namespace HairSalon.Controllers
{
    public class StylistController : Controller
    {

        [HttpGet("/stylists")]
        public ActionResult Index()
        {
            List<Stylist> allStylists = Stylist.GetAll();
            return View(allStylists);
        }

        [HttpGet("/stylists/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/stylists")]
        public ActionResult Create(string stylistName)
        {
            Stylist newStylist = new Stylist(stylistName);
            newStylist.Save();
            List<Stylist> allStylists = Stylist.GetAll();
            return View("Index", allStylists);
        }

        [HttpGet("/stylists/{id}/{sortBy}")]
        public ActionResult SortByDueDate(int id, string sortBy)
        {
            return RedirectToAction("Show", new { id = id, sortBy });
        }

        [HttpGet("/stylists/{id}")]
        public ActionResult Show(int id, string sortBy = "")
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist selectedStylist = Stylist.Find(id);
            List<Client> stylistClients = selectedStylist.GetClients(sortBy);
            List<Client> allClients = Client.GetAll();
            model.Add("stylist", selectedStylist);
            // Console.WriteLine("category {0} {1}", selectedCategory.GetId(), selectedCategory.GetName());
            model.Add("stylistClients", stylistClients);
            // Console.WriteLine("items {0}", categoryItems.Count);
            model.Add("allClients", allClients);
            return View(model);
        }

        [HttpPost("/stylists/{stylistId}/clients/new")]
        public ActionResult AddItem(int stylistId, int clientId)
        {
            Stylist stylist = Stylist.Find(stylistId);
            Client client = Client.Find(clientId);
            stylist.AddClient(client);
            return RedirectToAction("Show", new { id = stylistId });
        }
    }
}
