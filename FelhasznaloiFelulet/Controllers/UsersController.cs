using FelhasznaloiFelulet.Data;
using FelhasznaloiFelulet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace FelhasznaloiFelulet.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        public IActionResult Index(Users searchObj)
        {
            var lastName = searchObj.Lastname;
            var firstName = searchObj.Firstname;
            var swift = searchObj.BankSwift;
            var countryID = searchObj.UserAddress.CountryID;
            var city = searchObj.UserAddress.City;
            if(swift == "null")
            {
                swift = null;
            }
            if (countryID == "null")
            {
                countryID = null;
            }
            IEnumerable<Users> objUsersList;
            IEnumerable<Address> objAddressList;
            objUsersList = _db.Users.ToList();
            objAddressList = _db.Address.ToList();
            foreach (var obj in objUsersList)
            {
                Address address = _db.Address.Find(obj.ID);
                Bank bank = _db.Bank.Find(obj.BankSwift);
                Countries country = _db.Countries.Find(address.CountryID);
                address.Country = country;
                obj.UserAddress = address;
                obj.UserBank = bank;
            }
            //if (lastName == null && firstName == null && swift == null && countryID == null && city == null)
            //{
            //    return RedirectToAction("Index");
            //}
                if (lastName != null)
                {
                    objUsersList = objUsersList.Where(u => u.Lastname == lastName);
                }
                if (firstName != null)
                {
                    objUsersList = objUsersList.Where(u => u.Firstname == firstName);
                }
                if (swift != null)
                {
                    objUsersList = objUsersList.Where(u => u.BankSwift == swift);
                }
                if (countryID != null)
                {
                    objUsersList = objUsersList.Where(u => u.UserAddress.CountryID == countryID);
                }
                if (city != null)
                {
                    objUsersList = objUsersList.Where(u => u.UserAddress.City == city);
                }
            ViewData["userList"] = objUsersList;
            IEnumerable<Countries> objCountriesList = _db.Countries.ToList();
            ViewData["Countries"] = objCountriesList;
            IEnumerable<Bank> objBankList = _db.Bank.ToList();
            ViewData["Banks"] = objBankList;
            return View();
        }
        public IActionResult EditUser(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFromDb = _db.Users.Find(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            Address address = _db.Address.FirstOrDefault(a => a.ID == id);
            Bank bank = _db.Bank.Find(userFromDb.BankSwift);
            Countries country = _db.Countries.Find(address.CountryID);
            address.Country = country;
            userFromDb.UserBank = bank;
            userFromDb.UserAddress = address;
            userFromDb.UserAddress.ID = id;

            IEnumerable<Countries> objCountriesList = _db.Countries.ToList();
            ViewData["Countries"] = objCountriesList;
            IEnumerable<Bank> objBankList = _db.Bank.ToList();
            ViewData["Banks"] = objBankList;

            return View(userFromDb);
        }
        [HttpPost]
        public IActionResult EditUser(Users obj, Address objAddress, int id, int addressid)
        {
            if (obj.UserAddress.City == null || obj.UserAddress.Number == null)
            {
                TempData["error"] = "Minden mező kitöltése kötelező!";
                return RedirectToAction("AddUser");
            }
            if (ModelState.IsValid)
            {
                objAddress = _db.Address.FirstOrDefault(a => a.ID == addressid);
                objAddress.City = obj.UserAddress.City;
                objAddress.Number = obj.UserAddress.Number;
                Countries country = _db.Countries.Find(obj.UserAddress.Country.ID);
                objAddress.Country = country;

                Bank bank = _db.Bank.Find(obj.BankSwift);
                obj.UserBank = bank;
                obj.UserAddress = objAddress;

                _db.Users.Update(obj);
                _db.Address.Update(objAddress);
                _db.SaveChanges();
                TempData["success"] = "A felhasználó szerkesztve lett!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Minden mező kitöltése kötelező!";
            return RedirectToAction("EditUser",obj);
        }
        public IActionResult Index()
        {

            IEnumerable<Users> objUsersList = _db.Users.ToList();
            IEnumerable<Address> objAddressList = _db.Address.ToList();
            foreach (var obj in objUsersList)
            {
                Address address = _db.Address.FirstOrDefault(a => a.ID == obj.UserAddress.ID);
                Bank bank = _db.Bank.Find(obj.BankSwift);
                Countries country = _db.Countries.Find(address.CountryID);
                address.Country = country;
                obj.UserAddress = address;
                obj.UserBank = bank;
            }
            ViewData["userList"] = objUsersList;
            IEnumerable<Countries> objCountriesList = _db.Countries.ToList();
            ViewData["Countries"] = objCountriesList;
            IEnumerable<Bank> objBankList = _db.Bank.ToList();
            ViewData["Banks"] = objBankList;
            return View();
        }
        //GET
        public IActionResult AddUser()
        {
            IEnumerable<Countries> objCountriesList = _db.Countries.ToList();
            ViewData["Countries"] = objCountriesList;
            IEnumerable<Bank> objBankList = _db.Bank.ToList();
            ViewData["Banks"] = objBankList;
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(Users obj, Address objAddress) 
        {
            if(obj.UserAddress.City == null || obj.UserAddress.Number == null)
            {
                TempData["error"] = "Az összes mező kitöltése kötelező!";
                return RedirectToAction("AddUser");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    objAddress.City = obj.UserAddress.City;
                    objAddress.Number = obj.UserAddress.Number;
                    Countries country = _db.Countries.Find(obj.UserAddress.Country.ID);
                    objAddress.Country = country;

                    Bank bank = _db.Bank.Find(obj.BankSwift);
                    obj.UserBank = bank;
                    obj.UserAddress = objAddress;

                    _db.Users.Add(obj);
                    _db.Address.Add(objAddress);
                    _db.SaveChanges();
                    TempData["success"] = "Az új felhasználó létre lett hozva!";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "Az összes mező kitöltése kötelező!";
                return RedirectToAction("AddUser");
            }
        }
        //[Route("Users/CreateJson/lista")]
        //IEnumerable<Users> lista
        public IActionResult CreateJson()
        {
            IEnumerable<Users> objUsersList = _db.Users.ToList();
            IEnumerable<Address> objAddressList = _db.Address.ToList();
            foreach (var obj in objUsersList)
            {
                Address address = _db.Address.AsNoTracking().SingleOrDefault(a => a.ID == obj.ID);
                Bank bank = _db.Bank.AsNoTracking().SingleOrDefault(b => b.Swift == obj.BankSwift);
                Countries country = _db.Countries.AsNoTracking().SingleOrDefault(c => c.ID == address.CountryID);
                address.Country = country;
                obj.UserAddress = address;
                obj.UserBank = bank;
            }
            Object users = objUsersList as Object;

            //Object users = lista as Object;
            //Object users = lista;

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(json);
            writer.Flush();
            stream.Position = 0;
            TempData["success"] = "A JSON fájl létre lett hozva!";
            return File(stream, "text/json", "jsondata.json");
            
            //return RedirectToAction("Index");

        }
        public IActionResult DeleteUser(int id, int addressid)
        {
            Users user = _db.Users.FirstOrDefault(u => u.ID == id);
            Address address = _db.Address.FirstOrDefault(a => a.ID == addressid);

            _db.Address.Remove(address);
            _db.Users.Remove(user);
            _db.SaveChanges();
            TempData["success"] = "A felhasználó törölve lett az adatbázisból!";

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult ReadFromJson(IFormCollection form, IFormFile postedFile)
        {
            using (var ms = new MemoryStream())
            {
                postedFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);
                // act on the Base64 data

                byte[] data = Convert.FromBase64String(s);
                string json = Encoding.UTF8.GetString(data);

                

                IEnumerable<Users> readUserList = JsonConvert.DeserializeObject<IEnumerable<Users>>(json);
                List<Bank> readBankListBefore = new List<Bank>();
                List<Countries> readCountriesListBefore = new List<Countries>();
                List<Address> readAddressListBefore = new List<Address>();
                foreach(var obj in readUserList)
                {
                    readBankListBefore.Add(obj.UserBank);
                }
                foreach(var obj in readUserList)
                {
                    readCountriesListBefore.Add(obj.UserAddress.Country);
                }
                foreach(var obj in readUserList)
                {
                    readAddressListBefore.Add(obj.UserAddress);
                }
                IEnumerable<Bank> readBankList = readBankListBefore as IEnumerable<Bank>;
                IEnumerable<Countries> readCountriesList = readCountriesListBefore as IEnumerable<Countries>;
                IEnumerable<Address> readAddressList = readAddressListBefore as IEnumerable<Address>;

                foreach (var obj in readUserList)
                {
                    Bank actualBank = _db.Bank.AsNoTracking().SingleOrDefault(b => b.Swift == obj.UserBank.Swift);                    
                    if (actualBank == null)
                    {
                        _db.Bank.Add(obj.UserBank);
                    }
                    else if (actualBank.Swift != obj.UserBank.Swift || actualBank.Name != obj.UserBank.Name || actualBank.SeatAddress != obj.UserBank.SeatAddress)
                    {
                        _db.Bank.Update(obj.UserBank);
                    }
                    Countries actualCountries = _db.Countries.AsNoTracking().SingleOrDefault(c => c.ID == obj.UserAddress.Country.ID);
                    if (actualCountries == null)
                    {
                        _db.Countries.Add(obj.UserAddress.Country);
                    }
                    else if (actualCountries.ID != obj.UserAddress.Country.ID || actualCountries.Name != obj.UserAddress.Country.Name || actualCountries.CountryTel != obj.UserAddress.Country.CountryTel || actualCountries.isEU != obj.UserAddress.Country.isEU)
                    {
                        _db.Countries.Update(obj.UserAddress.Country);
                    }
                    Address actualAddress = _db.Address.AsNoTracking().SingleOrDefault(a => a.ID == obj.UserAddress.ID);
                    if (actualAddress == null)
                    {
                        _db.Address.Add(obj.UserAddress);
                    }
                    else if (actualAddress.ID != obj.UserAddress.ID || actualAddress.City != obj.UserAddress.City || actualAddress.Number != obj.UserAddress.Number || actualAddress.CountryID != obj.UserAddress.CountryID)
                    {
                        _db.Address.Update(obj.UserAddress);
                    }
                    Users actualUsers = _db.Users.AsNoTracking().SingleOrDefault(u => u.ID == obj.ID);
                    if (actualUsers == null)
                    {
                        _db.Users.Add(obj);
                    }
                    else if (actualUsers.ID != obj.ID || actualUsers.Lastname != obj.Lastname || actualUsers.Firstname != obj.Firstname || actualUsers.AccountNumber != obj.AccountNumber || actualUsers.BankSwift != obj.BankSwift || actualUsers.Mobile != obj.Mobile)
                    {
                        _db.Users.Update(obj);
                    }
                    
                    _db.SaveChanges();
                }
                TempData["success"] = "A JSON fileból be lettek olvasva az adatok és az adatbázis frissült!";
                return RedirectToAction("Index");
            }
        }
    }
}
