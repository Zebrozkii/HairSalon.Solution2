using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Collections.Generic;
using System;

namespace HairSalon.Controllers
{
    public class ClientsController : Controller
    {
        [HttpGet("/clients")]
        public ActionResult Index()
        {
            List<Client> allClients = Client.GetAll();
            return View(allClients);
        }

        [HttpGet("/clients/{clientid}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Client selectedClient = Client.Find(id);
            int stylistId = selectedClient.GetStylistId();
            Stylist clientStylist = Stylist.Find(stylistId);
            List<Stylist> allStylists = Stylist.GetAll();
            model.Add("selectedClient", selectedClient);
            model.Add("clientStylists", clientStylist);
            model.Add("allStylists", allStylists);

            return View(model);
        }

        [HttpPost("/stylists/{stylistId}/clients/new")]
        public ActionResult New(int stylistId)
        {
          Stylist stylist = Stylist.Find(stylistId);
          return View(stylist);
        }
        [HttpGet("/stylists/{stylistId}/clients/{clientId}")]
            public ActionResult Show(int stylistId, int clientId)
            {
                Client client = Client.Find(clientId);
                Dictionary<string, object> model = new Dictionary<string, object>();
                Stylist stylist = Stylist.Find(stylistId);
                model.Add("client", client);
                model.Add("stylist", stylist);
                return View(model);
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
        public ActionResult Update(int stylistId, int clientId, string newName)
        {
            Client client = Client.Find(clientId);
            client.Edit(newName);
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist stylist = Stylist.Find(stylistId);
            model.Add("stylist",stylist);
            model.Add("client",client);
            return View("Show", model);
        }

        [HttpGet("/clients/{clientId}/delete")]
        public ActionResult DeleteClient(int stylistId, int clientId)
        {
          Stylist stylist =Stylist.Find(stylistId);
          Client client = Client.Find(clientId);
          client.Delete();
          return View("Delete");
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
