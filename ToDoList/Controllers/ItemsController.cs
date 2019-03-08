using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {
        [HttpGet("/clients")]
        public ActionResult Index()
        {
            List<Client> allClients = Client.GetAll();
            return View(allClients);
        }

        [HttpGet("/clients/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/clients")]
        public ActionResult Create(string clientName, DateTime dueDate)
        {
            Client newClient = new Client(clientName, dueDate);
            newClient.Save();
            List<Client> allClients = Client.GetAll();
            return View("Index", allClients);
        }

        [HttpGet("/clients/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Client selectedClient = Client.Find(id);
            List<Stylist> clientStylists = selectedClient.GetStylist();
            List<Stylist> allStylists = Stylist.GetAll();
            model.Add("selectedClient", selectedClient);
            model.Add("clientStylists", clientStylists);
            model.Add("allStylist", allStylists);
            return View(model);
        }

        [HttpPost("/clients/{clientId}/stylists/new")]
        public ActionResult AddCategory(int clientId, int stylistId)
        {
            Client client = Client.Find(clientId);
            Stylist stylist = Stylist.Find(stylistId);
            client.AddStylist(stylist);
            return RedirectToAction("Show",  new { id = clientId });
        }

        [HttpPost("/clients/delete")]
        public ActionResult DeleteAll()
        {
            Client.ClearAll();
            return View();
        }

        [HttpGet("/clients/{clientId}/edit")]
        public ActionResult Edit(int stylistId, int clientId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Client client = Client.Find(clientId);
            model.Add("client", client);
            return View(model);
        }

        [HttpPost("/clients/{clientId}")]
        public ActionResult Update(int stylistId, int clientId, string newName, DateTime newDueDate)
        {
            Client client = Client.Find(clientId);
            client.Edit(newName, newDueDate);
            Dictionary<string, object> model = new Dictionary<string, object>();
            List<Stylist> clientStylists = client.GetStylists();
            List<Stylist> allStylists = Stylist.GetAll();
            model.Add("selectedClient", client);
            model.Add("itemStylists", clientStylists);
            model.Add("allStylist", allStylists);
            return View("Show", model);
        }

        [HttpGet("/clients/{clientId}/delete")]
        public ActionResult Delete(int stylistId, int clientId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Client client = Client.Find(clientId);
            client.Delete();
            model.Add("client", client);
            return View(model);
        }

        [HttpPost("/clients/deleted")]
        public ActionResult DeleteClient(int clientId)
        {
            Client client = Client.Find(clientId);
            client.Delete();
            return RedirectToAction("Index");
        }
    }
}
