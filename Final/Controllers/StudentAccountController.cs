using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final.Models;

namespace Final.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentAccountController : Controller
    {
        private McqsDatabaseEntities db = new McqsDatabaseEntities();
        int s_id = 1;
        public ActionResult Index()
        {
            var query = from test in db.Tests
                        join teacher in db.Teachers on test.t_id equals teacher.t_id
                        join student in db.Students on teacher.t_id equals student.t_id 
                        join entry in (from entry in db.Takes 
                            where entry.s_id != s_id
                            select entry) on test.test_id equals entry.test_id
                        where student.s_id == s_id 
                        select test;
            return View(query.ToList());
        }
        public ActionResult Results()
        {
            var query = from result in db.Takes
                        join test in db.Tests on result.test_id equals test.test_id
                        where result.s_id == s_id
                        select test;
            var query1 = from result in db.Takes
                         where result.s_id == s_id
                         select result;
            StudentTestResultViewModel vm = new StudentTestResultViewModel();
            vm.tests = query.ToList();
            vm.Takes = query1.ToList();
            return View(vm);
        }
        public ActionResult TakeTest(int? id)
        {
            ViewBag.name = db.Tests.Find(id).name;
            ViewBag.time = db.Tests.Find(id).duaration;
            var testQuestions = db.Questions.Where(q => q.test_id == id).ToList();
            ViewBag.id = id;
            return View(testQuestions);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void TakeTest(FormCollection form)
        {
            int test_id = Convert.ToInt32(form["id"]);
            var questions = db.Questions.Where(q => q.test_id == test_id);
            int marks = 0;
            foreach (var question in questions)
            {
                if (question.answer == Convert.ToInt32(form[question.statement]))
                {
                    marks++;    
                }
            }
            Take take = new Take();
            take.test_id = test_id;
            take.s_id = s_id;
            take.marks = marks;
            db.Takes.Add(take);
            db.SaveChanges();
            RedirectToAction("Index", "StudentAccount");
        }
	}
}