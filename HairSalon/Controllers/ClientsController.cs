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
        [HttpGet("/clients/new")]
        public ActionResult New()
        {
          return View();
        }

        [HttpPost("/clients")]
        public ActionResult Create(string newClientName)
        {
          Client newClient = new Client(newClientName);
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
          model.Add("allStylists", allStylists);
          return View(model);
        }
      }
    }
