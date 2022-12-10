using FelhasznaloiFelulet.Data;
using FelhasznaloiFelulet.Models;
using Microsoft.AspNetCore.Mvc;

namespace FelhasznaloiFelulet.Controllers
{
    public class BankController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BankController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Bank> objBankList = _db.Bank.ToList();
            return View(objBankList);
        }
        //GET
        public IActionResult AddNewBank()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewBank(Bank obj)
        {
            if (obj.Swift != null)
            {
                var bankFromDb = _db.Bank.Find(obj.Swift.ToUpper());
                if (bankFromDb == null)
                {
                    if (obj.Name == null || obj.SeatAddress == null)
                    {
                        TempData["error"] = "Az összes mező kitöltése kötelező!";
                        return View(obj);
                    }
                    if (ModelState.IsValid)
                    {
                        _db.Bank.Add(obj);
                        _db.SaveChanges();
                        TempData["Success"] = "Az új bank hozzá lett adva!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "A bank SWIFT kód alapján már szerepel a rendszerben!";
                    //ModelState.AddModelError("SzerepelError", "A bank SWIFT kód alapján már szerepel a rendszerben!");
                }
            }
            else
            {
                TempData["error"] = "A Bank SWIFT kódját kötelező megadni!";
                //ModelState.AddModelError("SwiftKellError", "A Bank SWIFT kódját kötelező megadni!");
            }
            
            return View(obj);
        }
        //GET
        public IActionResult EditBank(String swift)
        {
            if(swift == null)
            {
                TempData["error"] = "Az adott bank nem található!";
                return RedirectToAction("Index");
            }
            Bank bank = _db.Bank.Find(swift);

            if(bank == null)
            {
                TempData["error"] = "Az adott bank nem található!";
                return RedirectToAction("Index");
            }
            return View(bank);
        }
        //POST
        [HttpPost]
        public IActionResult EditBank(Bank obj)
        {
            if(obj.Name == null || obj.Swift == null || obj.SeatAddress == null)
            {
                TempData["error"] = "Az összes mező kitöltése kötelező!";
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _db.Bank.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "A bank sikeresen frissítve lett!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
