using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using exam3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace exam3.Controllers
{
    public class HomeController : Controller
    {

        private DBContext context;
        private PasswordHasher<RegisterView> RegisterHasher = new PasswordHasher<RegisterView>();
        private PasswordHasher<LoginView> LoginHasher = new PasswordHasher<LoginView>();

        public HomeController(DBContext dbc)
        {
            context = dbc;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.valErrors = ModelState.Values;
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterView NewUser)
        {
            User Validate = context.Users.Where(user => user.UserName == NewUser.UserName).SingleOrDefault();
            if (ModelState.IsValid && Validate == null)
            {
                User ValidUser = new User()
                {
                    UserName = NewUser.UserName,
                    UserAlias = NewUser.UserAlias,
                    Email = NewUser.Email,
                    Password = NewUser.Password
                };
                context.Users.Add(ValidUser);
                context.SaveChanges();
                Validate = context.Users.Where(user => user.UserName == ValidUser.UserName).SingleOrDefault();
                HttpContext.Session.SetInt32("UserId", (int)Validate.UserId);
                return Redirect("Idea");
            }
            else
            {
                ViewBag.valErrors = ModelState.Values;
                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginView User)
        {
            if (ModelState.IsValid)
            {
                User Validate = context.Users.Where(user => user.Email == User.Email).SingleOrDefault();
                if (Validate != null)
                {
                    if ((string)Validate.Password == User.Password)
                    {
                        HttpContext.Session.SetInt32("UserId", (int)Validate.UserId);
                        return Redirect("Idea");
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Bad Password");
                        ViewBag.valErrors = ModelState.Values;
                        return View("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "No account for that Username");
                    ViewBag.valErrors = ModelState.Values;
                    return View("Index");
                }
            }
            else
            {
                ViewBag.valErrors = ModelState.Values;
                return View("Index");
            }
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        [HttpGet("Idea")]
        public IActionResult Idea()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Redirect("Index");
            }
            ViewBag.CurrentUser = context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId")).Single();
            ViewBag.AllIdeas = context.Ideas.Include(u => u.Owner).Include(l => l.LikedBy).OrderByDescending(l => l.LikedBy.Count).ToList();
            return View("Idea");

        }
        [HttpPost("newidea")]
        
        public IActionResult PostNewIdea(string UserIdea)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Redirect("Index");
            }
            if (UserIdea.Length > 0)
            {
                Idea NewIdea = new Idea()
                {
                    UserId = (int)HttpContext.Session.GetInt32("UserId"),
                    UserIdea = UserIdea
                };
                context.Ideas.Add(NewIdea);
                context.SaveChanges();
            }
            return Redirect("Idea");
        }
        [HttpGet("like/{ideaId}")]
        
        public IActionResult PostLike(int ideaId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Redirect("Index");
            }
            Like NewLike = new Like()
            {
                IdeaId = ideaId,
                UserId = (int)HttpContext.Session.GetInt32("UserId")
            };
            context.Likes.Add(NewLike);
            context.SaveChanges();
            return Redirect("/Idea");
        }
        [HttpGet("users/{userId}")]
        
        public IActionResult UserInfo(int userId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Redirect("Index");
            }
            ViewBag.SelectedUser = context.Users.Where(u => u.UserId == userId).Include(i => i.UsersIdeas).Include(l => l.UserLikes).Single();
            return View();
        }
        [HttpGet("bright_ideas/{ideaId}")]
        public IActionResult IdeaInfo(int ideaId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Redirect("Index");
            }
            ViewBag.SelectedIdea = context.Ideas.Where(i => i.IdeaId == ideaId).Include(u => u.Owner).Include(l => l.LikedBy).ThenInclude(u => u.User).Single();
            ViewBag.LikedBy =  context.Likes.Where(i => i.IdeaId == ideaId).ToList();
            return View();
        }
        [HttpGet("deleteidea/{ideaId}")]
        public IActionResult DeleteIdea(int ideaId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Redirect("Index");
            }
            var ToDelete = new Idea { IdeaId = ideaId };
            context.Ideas.Attach(ToDelete);
            context.Ideas.Remove(ToDelete);
            context.SaveChanges();
            return Redirect("/Idea");
        }
        
    }
}

