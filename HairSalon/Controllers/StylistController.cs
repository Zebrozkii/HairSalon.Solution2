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
            List<Client> stylistClients = selectedStylist.GetClients();
            List<Specialty> specialtyStylists = selectedStylist.GetSpecialties();
            List<Specialty> allSpecialties = Specialty.GetAll();
            model.Add("selectedStylist", selectedStylist);
            model.Add("stylistClients", stylistClients);
            model.Add("specialtyStylists", specialtyStylists);
            model.Add("allSpecialties", allSpecialties);
            return View(model);
        }

        [HttpPost("/stylists/{stylistId}/clients")]
        public ActionResult Create(string clientName, int stylistId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist foundStylist = Stylist.Find(stylistId);
            Client newClient = new Client(clientName, stylistId);
            newClient.Save();
            List<Client> stylistClients = foundStylist.GetClients();
            model.Add("clients", stylistClients);
            model.Add("stylist", foundStylist);
            return View("Show", model);
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
