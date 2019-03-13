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
            return RedirectToAction("Index");
        }

        [HttpGet("/stylists/{id}")]
        public ActionResult Show(int id)
        {
          Dictionary<string, object> model = new Dictionary<string, object>();
          Stylist selectedStylist = Stylist.Find(id);
          List<Client> stylistClient = selectedStylist.GetClients();
          List<Client> allClients = Client.GetAll();
          List<Specialty> stylistSpecialty = selectedStylist.GetSpecialties();
          List<Specialty> allSpecialty = Specialty.GetAll();
          model.Add("stylist", selectedStylist);
          model.Add("stylistClient", stylistClient);
          model.Add("allClients", allClients);
          model.Add("stylistSpecialty", stylistSpecialty);
          model.Add("allSpecialty", allSpecialty);
          return View(model);
        }
        [HttpPost("/stylists/{stylistId}/clients/new")]
            public ActionResult AddClient(int stylistId, int clientId)
            {
              Stylist stylist = Stylist.Find(stylistId);
              Client client = Client.Find(clientId);
              stylist.AddClient(client);
              return RedirectToAction("Show", new { id = stylistId });
        }


        [HttpPost("/stylists/{stylistId}/delete")]
        public ActionResult Delete(int stylistId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist stylist = Stylist.Find(stylistId);
            stylist.Delete();
            model.Add("stylist", stylist);
            return View(model);
        }

        [HttpGet("/stylists/delete")]
        public ActionResult DeleteStylist(int stylistId)
        {
            Stylist stylist = Stylist.Find(stylistId);
            stylist.Delete();
            return RedirectToAction("Index");
        }

        [HttpGet("/stylists/deleteAll")]
        public ActionResult DeleteAll()
        {
            Stylist.ClearAll();
            return View();
        }

        [HttpGet("/stylists/{stylistId}/edit")]
        public ActionResult Edit(int stylistId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist stylist = Stylist.Find(stylistId);
            model.Add("stylist", stylist);
            return View(model);
        }

        [HttpPost("/stylists/{stylistId}")]
        public ActionResult Update(int stylistId, string editName)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist selectedStylist = Stylist.Find(stylistId);
            List<Client> stylistClients = selectedStylist.GetClients();
            List<Specialty> specialtyStylists = selectedStylist.GetSpecialties();
            List<Specialty> allSpecialties = Specialty.GetAll();
            model.Add("specialtyStylists", specialtyStylists);
            model.Add("stylist", selectedStylist);
            model.Add("clients", stylistClients);
            model.Add("allSpecialties", allSpecialties);
            selectedStylist.Edit(editName);
            return View("Show", model);
        }

        [HttpPost("/stylists/{stylistId}/specialty/new")]
        public ActionResult AddSpecialty(int stylistId, int specialtyId)
        {
            Stylist stylist = Stylist.Find(stylistId);
            Specialty specialty = Specialty.Find(specialtyId);
            stylist.AddSpecialty(specialty);
            return RedirectToAction("Show", new { id = stylistId });
        }
    }
}
