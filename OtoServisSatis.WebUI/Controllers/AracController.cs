﻿using Microsoft.AspNetCore.Mvc;
using OtoServisSatis.Entities;
using OtoServisSatis.Service.Abstract;
using OtoServisSatis.WebUI.Models;
using OtoServisSatis.WebUI.Utils;
using System.Security.Claims;

namespace OtoServisSatis.WebUI.Controllers
{
    public class AracController : Controller
    {
        private readonly ICarService _serviceArac;
        private readonly IService<Musteri> _serviceMusteri;
        private readonly IUserService _service;

        public AracController(ICarService serviceArac, IService<Musteri> serviceMusteri, IUserService service)
        {
            _serviceArac = serviceArac;
            _serviceMusteri = serviceMusteri;
            _service = service;
        }


        public  async Task<IActionResult> IndexAsync(int? id)
        {
            if (id==null)
                return BadRequest();
            
            var arac = await _serviceArac.GetCustomCar(id.Value);
            if (arac == null)
                return NotFound();
            var model = new CarDetailViewModel();
            model.Arac = arac;
            if (User.Identity.IsAuthenticated)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var uguid = User.FindFirst(ClaimTypes.UserData)?.Value;
                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(email))
                {
                    var user = _service.Get(k => k.Email == email && k.UserGuid.ToString() == uguid);
                    if (user != null)
                    {
                        model.Musteri = new Musteri()
                        {
                            Adi = user.Adi,
                            Soyadi = user.Soyadi,
                            Email = user.Email,
                            Telefon = user.Telefon,
                        };
                    }
                }
            }
            return View(model);
        }
        [Route("tum-araclarimiz")]
        public  async Task<IActionResult> List()
        {
            var model = await _serviceArac.GetCustomCarList(c=>c.SatistaMi);
            return View(model);
        }
        public  async Task<IActionResult> Ara(string q)
        {
            var model = await _serviceArac.GetCustomCarList(c=>c.SatistaMi && c.Marka.Adi.Contains(q) || c.KasaTipi.Contains(q) || c.Modeli.Contains(q));
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> MusteriKayit(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceMusteri.AddAsync(musteri);
                    await _serviceMusteri.SaveAsync();
                    //await  MailHelper.SendMailAsync(musteri);
                    TempData["Message"] = "<div class='alert alert-succes'>Talebiniz Alınmıştır. Teşekkürler...</div>";
                    return Redirect("/Arac/Index/" + musteri.AracId);
                }
                catch
                {
                    TempData["Message"] = "<div class='alert alert-danger'>Hata Oluştu!</div>";
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            //olası düzeltme
            return RedirectToAction("Index", new { id = musteri.AracId });
        }
    }
}
