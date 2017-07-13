using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace Final.Controllers
{
    [Authorize (Roles = "user")]
    public class TeacherAccountController : Controller
    {
        private McqsDatabaseEntities db = new McqsDatabaseEntities();
        public ActionResult Index()
        {
            var teacher = db.Teachers.First(t => t.username == User.Identity.Name);
            int t_id = teacher.t_id;
            var students = db.Students.Where(s => s.t_id == t_id);
            var tests = db.Tests.Where(t => t.t_id == t_id);
            var model = new StudentTestViewModel { students = students.ToList(), tests = tests.ToList() };
            return View(model);
        }
        //------------------------------------------------------------ViewTests
        public ActionResult ViewTests(Test test)
        {
            var teacher = db.Teachers.First(t => t.username == User.Identity.Name);
            int t_id = teacher.t_id;
            test.t_id = t_id;
            if (test.name != null)
            {
                if (ModelState.IsValid)
                {
                    db.Tests.Add(test);
                    db.SaveChanges();
                }    
            }
            var tests = db.Tests.Where(t => t.t_id == t_id).ToList();

            return PartialView("ViewTests",tests);
        }
        //------------------------------------------------------------CreateTest
        public ActionResult CreateTest()
        {
            return View();
        }

        //------------------------------------------------------------ManageQuestions
        public ActionResult ManageQuestions(int id)
        {
            //this is where i will set id in session of which test is being edit
            ViewBag.test_id = id;
            return View();
        }
        //------------------------------------------------------------Questions
        public ActionResult Questions(Question question, int id)
        {

            question.test_id = id;
            if (question.statement != null)
            {
                if (ModelState.IsValid)
                {
                    db.Questions.Add(question);
                    db.SaveChanges();
                }
            }
            var questions = db.Questions.Where(q => q.test_id == question.test_id).ToList();
            return PartialView("Questions", questions);
        }
        //------------------------------------------------------------DeleteQuestion
        public ActionResult DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [HttpPost, ActionName("DeleteQuestion")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("ManageQuestions");
        }
        //------------------------------------------------------------EditQuestion
        public ActionResult EditQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.test_id = new SelectList(db.Tests, "test_id", "name", question.test_id);
            return View(question);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion([Bind(Include = "q_id,statement,op1,op2,op3,op4,answer,test_id")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageQuestions");
            }
            ViewBag.test_id = new SelectList(db.Tests, "test_id", "name", question.test_id);
            return View(question);
        }
        //------------------------------------------------------------QuestionDetails
        public ActionResult QuestionDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }
        //------------------------------------------------------------ViewStudent
        public ActionResult ViewStudents(Student student)
        {
            var teacher = db.Teachers.First(t => t.username == User.Identity.Name);
            int t_id = teacher.t_id;
            student.t_id = t_id;
            if (student.password!=null)
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                }    
            }
            
            var students = db.Students.Where(s => s.t_id == t_id).ToList();
            return PartialView("ViewStudents", students);
        }
        //------------------------------------------------------------AddStudent
        public ActionResult AddStudent()
        {
            return View();
        }
        //------------------------------------------------------------TestDetails
        public ActionResult TestDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            var questions = db.Questions.Where(q => q.test_id == test.test_id);
            var model = new TestQuestionViewModel { questions = questions, test = test };
            return View(model);
        }
        //------------------------------------------------------------DeleteTest
        public ActionResult TestDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            var questions = db.Questions.Where(q => q.test_id == test.test_id);
            var model = new TestQuestionViewModel { test = test, questions = questions };
            return View(model);
        }

        [HttpPost, ActionName("TestDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed5(int id)
        {
            Test test = db.Tests.Find(id);
            db.Tests.Remove(test);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        //------------------------------------------------------------EditTest
        public ActionResult EditTest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            ViewBag.t_id = new SelectList(db.Teachers, "t_id", "username", test.t_id);
            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTest([Bind(Include = "test_id,name,totalquestions,duaration")] Test test)
        {
            var teacher = db.Teachers.First(t => t.username == User.Identity.Name);
            int t_id = teacher.t_id;
            test.t_id = t_id;
            if (ModelState.IsValid)
            {
                db.Entry(test).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.t_id = new SelectList(db.Teachers, "t_id", "username", test.t_id);
            return View(test);
        }
        //------------------------------------------------------------DeleteStudent
        public ActionResult DeleteStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed1(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("AddStudent");
        }
        //------------------------------------------------------------StudentDetails
        public ActionResult StudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        //------------------------------------------------------------EditStudent
        public ActionResult EditStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent([Bind(Include = "s_id,rollno,password")] Student student)
        {
            var teacher = db.Teachers.First(t => t.username == User.Identity.Name);
            int t_id = teacher.t_id;
            student.t_id = t_id;
            if (ModelState.IsValid)
            {
                db.Entry(student).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddStudent");
            }
            return View(student);
        }
        
        //------------------------------------------------------------ViewResult
        [AllowAnonymous]
        public ActionResult ViewResult()
        {
            var teacher = db.Teachers.First(t => t.username == User.Identity.Name);
            int t_id = teacher.t_id;
            ViewBag.Message = "Your contact page.";
            var query = from result in db.Takes
                         join student in db.Students on result.s_id equals student.s_id
                         where student.t_id == t_id
                         select result;
            var query1 = from student in db.Students
                         join result in db.Takes on student.s_id equals result.s_id
                         where student.t_id == t_id
                         select student;
            var query2 = from test in db.Tests
                         join result in db.Takes on test.test_id equals result.test_id
                         where test.t_id == t_id
                         select test;
            StudentTestResultViewModel vm = new StudentTestResultViewModel();
            vm.Takes = query.ToList();
            vm.students = query1.ToList();
            vm.tests = query2.ToList();

            return View(vm);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}