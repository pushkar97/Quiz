using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quiz.Models;
using Quiz.ViewModel;
namespace Quiz.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            User obj = new User();
            return View(obj);
        }
        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("CreateQuiz", new { uid = user.Id});
        }

        [Route("Home/CreateQuiz/{uid}/{id?}")]
        public ActionResult CreateQuiz(int uid, int id = 1)
        {
            User user = _context.Users.Single(m => m.Id == uid);
            try
            {
                CreateQuizViewModel obj = new CreateQuizViewModel
                {
                    question = _context.Questions.Single(c => c.Id == id),
                    UserId = uid
                };
                obj.question.question = String.Format(obj.question.question, user.Username);
                return View(obj);
            }
            catch (Exception e)
            {
                return View("QuizCreated",user);
            }
        }

        [HttpPost]
        public ActionResult Save(CreateQuizViewModel model)
        {
            UserAns obj = new UserAns
            {
                UserId = model.UserId,
                QuestionId = model.question.Id,
                Answer = model.answer
            };
            _context.UserAns.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("CreateQuiz", new { uid = model.UserId, id = ++model.question.Id });
        }

        public ActionResult EnterQuiz(int id)
        {

            return View();
        }
    }
}
