using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltS.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BeltS.Controllers
{
    public class UserController : Controller
    {
        private BeltSContext _context;

        public UserController(BeltSContext context)
        {
            _context = context;
        }
        // GET: /Home///////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // List<Connection> allC= _context.Connection.ToList();
            // foreach(var c in allC){
            //    _context.Connection.Remove(c); 
            //    _context.SaveChanges();
            // }
            // List<User> allU= _context.User.ToList();
            // foreach(var u in allU){
            //    _context.User.Remove(u); 
            //    _context.SaveChanges();
            // }


            ViewBag.NewError = "";
            ViewBag.errors = new List<object>();
            return View();
        }
        [HttpGet]
        [Route("Register")]
        public IActionResult GetResgister()
        {
            ViewBag.NewError = "";
            ViewBag.errors = new List<object>();
            return View("Index");
        }
        // ////////////////////////Register////////////////////////////////////////////////////////////////////////////
        [HttpPostAttribute]
        [RouteAttribute("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //check if a user with this email already exists
                User new_user = _context.User.SingleOrDefault(user1 => user1.Email == model.Email);
                if (new_user == null)
                {
                    //if they dont exist make a new one

                    User NewUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Description = model.Description,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Password = model.Password
                    };
                    _context.Add(NewUser);
                    _context.SaveChanges();
                    //find the user we just registered in database and put them in session
                    User curuser = _context.User.SingleOrDefault(user2 => user2.Email == NewUser.Email);
                    HttpContext.Session.SetInt32("users_id", curuser.UserId);
                    HttpContext.Session.SetString("UserName", curuser.FirstName);
                    HttpContext.Session.SetString("UserLName", curuser.LastName);

                    return RedirectToAction("Profile");
                }
                //the user wasnt null
                ViewBag.NewError = "A User with this email already exists, please login";
                ViewBag.errors = new List<string>();
                return View("Index");
            }
            //the modelstate wasnt valid
            ViewBag.NewError = "";
            // ViewBag.errors = ModelState.Values;
            ViewBag.errors = new List<object>();
            return View("Index");
        }
        // ////////////////////////Login //////////////////////////////////////////////////////////////////////
        [HttpPostAttribute]
        [RouteAttribute("Login")]
        public IActionResult Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                if (Email == null || Password == null)
                {
                    ViewBag.NewError = "Please type in both an Email and Password";
                    ViewBag.errors = new List<object>();
                    return View("Index");
                }
                User cur_user = _context.User.SingleOrDefault(user1 => user1.Email.ToString() == Email);
                if (cur_user != null)
                {
                    if ((string)cur_user.Password == Password)
                    {
                        HttpContext.Session.SetInt32("users_id", cur_user.UserId);
                        HttpContext.Session.SetString("UserName", cur_user.FirstName);
                        HttpContext.Session.SetString("UserLName", cur_user.LastName);

                        return RedirectToAction("Profile");
                    }
                    ViewBag.NewError = "Password is incorrect!";
                    ViewBag.errors = new List<String>();
                    return View("Index");
                }

                ViewBag.errors = new List<object>(); ViewBag.NewError = "We could not find your email please register";
                return View("Index");
            }
            ViewBag.errors = ModelState.Values;
            ViewBag.NewError = "";
            return View("Index");
        }
        // //////////////////////////USER PROFILE //////////////////////////////
        [HttpGetAttribute]
        [RouteAttribute("professional_profile")]
        public IActionResult Profile()
        {
            int? CurUserId = HttpContext.Session.GetInt32("users_id");

            User cur_user = _context.User.SingleOrDefault(user1 => user1.UserId == CurUserId);
            if (cur_user != null)
            {
                //cur_user.Description = "I like fuzzy sweaters a lot";
                //_context.SaveChanges();
                ViewBag.Desc = cur_user.Description;
                User Allcon = _context.User
                    .Where(user1 => user1.UserId == CurUserId)
                    .Include(user => user.Connections)
                    .SingleOrDefault();
                    List<Connection> statusPend = _context.Connection
                    .Where(c => c.Status == "Pending")
                    .Where(c => c.CurUserId == CurUserId)
                    .Include(c=> c.NetUser)
                    .ToList();
                    List<Connection> statusAcc = _context.Connection
                    .Where(c => c.Status == "Accept")
                    .Where(c => c.CurUserId == CurUserId)
                    .Include(c=> c.NetUser)
                    .ToList();
                    ViewBag.Accept = statusAcc;
                ViewBag.Pend = statusPend;
                ViewBag.MyConec = Allcon.Connections;
                ViewBag.UserId = HttpContext.Session.GetInt32("users_id");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.UserLName = HttpContext.Session.GetString("UserLName");

                return View("Profile");
            }
            ViewBag.NewError = "You are not logged in, please register or login";
            ViewBag.errors = new List<String>();
            return View("Index");

        }
        // //////////////////
        [HttpGetAttribute]
        [RouteAttribute("users")]
        public IActionResult ShowAll()
        {

            int? MyUserId = HttpContext.Session.GetInt32("users_id");
            User cur_user = _context.User.SingleOrDefault(user1 => user1.UserId == MyUserId);
            if (cur_user != null)
            {


                ViewBag.MyUserId = MyUserId;
                
                List<User> DisplayUser = new List<User>();
                List<User> OtherUsers = _context.User.Where(u => u.UserId != MyUserId)
                .Include(u => u.Connections)
                .ToList();
                int count = 0;
                foreach (var user in OtherUsers)
                {
                    
                    foreach (var connec in user.Connections)
                    {
                        if(connec.CurUserId == MyUserId || connec.NetUserId == MyUserId){
                            count= count + 1;
                        }
                    }
                    if(count == 0){
                        DisplayUser.Add(user);
                }
                }
                
                
                ViewBag.OtherUsers = DisplayUser;

                return View("ShowAll");
            }
            ViewBag.NewError = "You are not logged in, please register or login";
            ViewBag.errors = new List<String>();
            return View("Index");
        }
        // //////////////////////////////LOGOUT ////////////////////////////////////
        [HttpPostAttribute]
        [RouteAttribute("Logout")]

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }
        // ///////////////////////////CONNECT////////////////////////
        [HttpPostAttribute]
        [RouteAttribute("Connect/{NetUserId}")]
        public IActionResult Connect(int? NetUserId, Connection conbody)
        {
            System.Console.WriteLine("!!!!!!!!!!!!!!!!HERE");
            System.Console.WriteLine(NetUserId);
            int? MyUserId = HttpContext.Session.GetInt32("users_id");
            User MyUser = _context.User.SingleOrDefault(user1 => user1.UserId == MyUserId);
            User NetworkU = _context.User.SingleOrDefault(user1 => user1.UserId == NetUserId);

            Connection CheckConnec = _context.Connection.Where(c => c.CurUserId == MyUserId && c.NetUserId == NetUserId).SingleOrDefault();

            if (CheckConnec == null && NetworkU != null)
            {
                Connection NewCon = new Connection
                {
                    CurUserId = MyUser.UserId,
                    NetUserId = NetworkU.UserId,
                    CurUser = MyUser,
                    NetUser = NetworkU,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Add(NewCon);
                _context.SaveChanges();

                System.Console.WriteLine("!!!!!!!!!!!!!ID should be!!!HERE");
                Connection ThisConnec = _context.Connection.Where(c => c.CurUserId == MyUserId && c.NetUserId == NetUserId).SingleOrDefault();
                NetworkU.Connections.Add(ThisConnec);
                _context.SaveChanges();
                MyUser.Connections.Add(ThisConnec);
                _context.SaveChanges();
                foreach (var c in MyUser.Connections)
                {
                    System.Console.WriteLine(c.NetUserId);
                    System.Console.WriteLine(NetworkU.UserId);
                    System.Console.WriteLine("COMPARISON!!!!!!!!!!!! it was pushed!");
                }

                // System.Console.WriteLine(ThisConnec.ConnecId);
                // User cur_user = _context.User.SingleOrDefault(user1 => user1.UserId == MyUserId);
                // ViewBag.MyUserId = MyUserId;
                // List<User> DisplayUser = new List<User>();
                // List<User> OtherUsers = _context.User.Where(u => u.UserId != MyUserId)
                // .Include(u => u.Connections)
                // .ToList();
                // foreach (var user in OtherUsers)
                // {

                //     foreach (var connec in user.Connections)
                //     {
                //         Connection exist = _context.Connection
                //         .Where(c => c.CurUserId == MyUserId || c.NetUserId == MyUserId)
                //         .SingleOrDefault();
                //         if (exist == null)
                //         {
                //             DisplayUser.Add(user);
                //         }
                //     }
                // }
                // ViewBag.OtherUsers = DisplayUser;
                return RedirectToAction("ShowAll");
            }
            System.Console.WriteLine("EXIST!!!!!!!!!!!!!!!!!!!!!!");
            return RedirectToAction("ShowAll");
        }
        [HttpGetAttribute]
        [RouteAttribute("users/{id}")]
        public IActionResult ShowOne(int id)
        {
            User show = _context.User.SingleOrDefault(user1 => user1.UserId == id);
            if(show != null){
                ViewBag.Show = show;
                ViewBag.NewError= "";
                return View("ShowOne");
            }
            ViewBag.Show = "";
            ViewBag.NewError = "Sorry we couldnt find the user, please try again";
            return View("ShowOne");
        }
        [HttpPostAttribute]
        [RouteAttribute("Ignore/{id}")]
        public IActionResult Ignore(int id)
        {
            Connection thisConnec = _context.Connection.SingleOrDefault(c => c.ConnecId == id);
            thisConnec.Status = "Ignore";
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
        [HttpPostAttribute]
        [RouteAttribute("Accept/{id}")]
        public IActionResult Accept(int id)
        {
            Connection thisConnec = _context.Connection.SingleOrDefault(c => c.ConnecId == id);
            thisConnec.Status = "Accept";
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }

    }
}
