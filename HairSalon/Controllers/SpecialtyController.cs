using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Collections.Generic;

namespace HairSalon.Controllers
{
  public class SpecialtyController : Controller
  {
    [HttpGet("/specialty")]
     public ActionResult Index()
     {
       List<Specialty> allSpecialties = Specialty.GetAll();
       return View(allSpecialties);
     }

     [HttpGet("/specialty/new")]
      public ActionResult New()
      {
        return View();
      }

      [HttpPost("/specialty")]
      public ActionResult Create(string newSpecialtyName)
      {
        Specialty newSpecialty = new Specialty(newSpecialtyName);
        newSpecialty.Save();
        List<Specialty> allSpecialties = Specialty.GetAll();
        return View("Index", allSpecialties);
      }

      [HttpGet("/specialty/{id}")]
      public ActionResult Show(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Specialty selectedSpecialty = Specialty.Find(id);
        List<Stylist> specialtyStylists = selectedSpecialty.GetStylists();
        List<Stylist> allStylists = Stylist.GetAll();
        model.Add("selectedSpecialty", selectedSpecialty);
        model.Add("specialtyStylists", specialtyStylists);
        model.Add("allStylists", allStylists);
        return View(model);
      }

      [HttpPost("/specialty/{specialtyId}/stylists/new")]
      public ActionResult AddStylist(int specialtyId, int stylistId)
      {
        Specialty specialty = Specialty.Find(specialtyId);
        Stylist stylist = Stylist.Find(stylistId);
        specialty.AddStylist(stylist);
        return RedirectToAction("Show",  new { id = specialtyId });
      }

      [HttpPost("/specialty/deleteall")]
      public ActionResult DeleteAll()
      {
        Specialty.ClearAll();
        return View();
      }

      [HttpPost("/specialty/{specialtyId}/delete")]
      public ActionResult Delete(int specialtyId)
      {
        Specialty specialty = Specialty.Find(specialtyId);
        specialty.Delete(specialtyId);
        return RedirectToAction("Index");
      }

      [HttpGet("/specialty/{specialtyId}/edit")]
      public ActionResult Edit(int specialtyId)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Specialty specialty = Specialty.Find(specialtyId);
        model.Add("specialty", specialty);
        return View(model);
      }

      [HttpPost("/specialty/{specialtyId}")]
      public ActionResult Update(int specialtyId, string newName)
      {
        Specialty specialty = Specialty.Find(specialtyId);
        specialty.Edit(newName);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Stylist> specialtyStylists = specialty.GetStylists();
        List<Stylist> allStylists = Stylist.GetAll();
        model.Add("selectedSpecialty", specialty);
        model.Add("specialtyStylists", specialtyStylists);
        model.Add("allStylists", allStylists);
        return View("Show", model);
      }
  }
}
