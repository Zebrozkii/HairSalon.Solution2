// using Microsoft.AspNetCore.Mvc;
// using HairSalon.Models;
// using System.Collections.Generic;
//
// namespace HairSalon.Controllers
// {
//     public class SpecialtyController : Controller
//     {
//         [HttpGet("/specialty")]
//         public ActionResult Index()
//         {
//             List<Specialty> allSpecialties = Specialty.GetAll();
//             return View(allSpecialties);
//         }
//
//         [HttpGet("/specialty/new")]
//         public ActionResult New(int id)
//         {
//             Dictionary<string, object> model = new Dictionary<string, object>();
//             Specialty selectedSpecialty = Specialty.Find(id);
//             model.Add("specialty", selectedSpecialty);
//             return View(model);
//         }
//
//         [HttpPost("/specialty")]
//         public ActionResult Create(string specialtyName)
//         {
//             Specialty newSpecialty = new Specialty(specialtyName);
//             newSpecialty.Save();
//             return RedirectToAction("Index");
//         }
//
//         [HttpGet("specialty/{id}")]
//         public ActionResult Show(int id)
//         {
//             Dictionary<string, object> model = new Dictionary<string, object>();
//             Specialty selectedSpecialty = Specialty.Find(id);
//             List<Stylist> specialtyStylists = selectedSpecialty.GetStylists();
//             List<Stylist> allStylists = Stylist.GetAll();
//             model.Add("selectedSpecialty", selectedSpecialty);
//             model.Add("specialtyStylists", specialtyStylists);
//             model.Add("allStylists", allStylists);
//             return View(model);
//         }
//
//         [HttpPost("/specialty/{specialtyId}/stylists/new")]
//         public ActionResult AddStylist(int specialtyId, int stylistId)
//         {
//             Specialty specialty = Specialty.Find(specialtyId);
//             Stylist stylist = Stylist.Find(stylistId);
//             specialty.AddStylist(stylist);
//             return RedirectToAction("Show", new { id = specialtyId });
//         }
//     }
// }
