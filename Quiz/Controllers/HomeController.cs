using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
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
            if (!ModelState.IsValid)
            {
                var obj = new User();
                return View("Index",obj);
            }
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
                obj.question.question = string.Format(obj.question.question, user.Username);
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

        public ActionResult HowMuchDoYouKnow(int id)
        {
            User user;
            try
            {
                user = _context.Users.Single(m => m.Id == id);
                
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }
            Score obj = new Score
            {
                UserId = user.Id
            };

            //HttpContext.Session.Add("userId", id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult CreateParticipant(Score score)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("HowMuchDoYouKnow", new {id = score.UserId});
            }
            score.score = 0;
            _context.Scores.Add(score);
            _context.SaveChanges();
            HttpContext.Session.Add("participantId", score.Id);
            try
            {
                var ans = _context.UserAns.Where(m => m.UserId == score.UserId).Include("Question").ToList();
                HttpContext.Session.Add("userAns", ans);
                HttpContext.Session.Add("user", _context.Users.Single(m => m.Id == score.UserId));
                HttpContext.Session.Add("score", 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return HttpNotFound();
            }

            var model = new EnterQuizViewModel
            {
                Question = _context.Questions.First()
            };
            return View("EnterQuiz",model);
        }
        [HttpPost]
        public ActionResult EnterQuiz(EnterQuizViewModel model)
        {
            var ansList = ((List<UserAns>)HttpContext.Session["userAns"]).AsQueryable().Single(m => m.Id==model.Question.Id);
            EnterQuizViewModel viewModel;
            if (ansList.Answer == model.Answer)
            {
                var score = (int) HttpContext.Session["score"];
                HttpContext.Session["score"] = score + 1;
            }
            var questions = _context.Questions;
            try
            {
                var question = questions.Single(m => m.Id == model.Question.Id);
                var qList = questions.ToList();
                viewModel = new EnterQuizViewModel
                {
                    Question = qList[qList.IndexOf(question)+1]
                };
            }
            catch (Exception e)
            {
                var score = (int) HttpContext.Session["participantId"];
                var obj = _context.Scores.Single(m => m.Id == score);
                obj.score = (byte)HttpContext.Session["score"];
                _context.SaveChanges();
                var num = ((User) HttpContext.Session["user"]).Id;
                HttpContext.Session.Remove("user");
                HttpContext.Session.Remove("participantId");
                HttpContext.Session.Remove("userAns");
                HttpContext.Session.Remove("score");
                return RedirectToAction("HowMuchDoYouKnow",new {id = num});
            }
            return View(viewModel);
        }
    }
}
