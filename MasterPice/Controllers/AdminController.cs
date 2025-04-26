using MasterPice.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasterPice.Controllers
{
	public class AdminController : Controller
	{
		private readonly MyDbContext _context;

		public AdminController(MyDbContext context)
		{
			_context = context;
		}
		public IActionResult Admin_dashboard()
		{
			var countenrolledusers = _context.EnrollmentUsers.Count();

			var countcourses = _context.Courses.Count();

			var countsections = _context.Students.Count();

			TempData["Countenrolledusers"] = countenrolledusers;
			TempData["Countcourses"] = countcourses;
			TempData["CountStudent"] = countsections;
			return View();
		}
		public IActionResult AdminLogin()
		{
			return View();
		}
		[HttpPost]
		public IActionResult AdminLogin(string email, string password)
		{
			// Check if the email and password match an admin in the database
			var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.PasswordHash == password);
			if (admin != null)
			{
				HttpContext.Session.SetString("AdminEmail", admin.Email);
				HttpContext.Session.SetString("AdminName", admin.FullName);
				HttpContext.Session.SetInt32("AdminId", admin.AdminId);
				HttpContext.Session.SetString("AdminProfilePicture", admin.ProfilePicture);
				return RedirectToAction("Admin_dashboard");
			}
			else
			{
				ViewBag.ErrorMessage = "Invalid email or password.";
				return View();
			}
		}
		public IActionResult Seeusers()
		{
			var users = _context.Students.ToList();
			return View(users);
		}
		public IActionResult SeeAllCourses()
		{
			var courses = _context.Courses.ToList();
			return View(courses);
		}
		public IActionResult SeeAllInstructor()
		{
			var instructors = _context.Teachers.ToList();
			return View(instructors);
		}
		[HttpPost]
		public IActionResult DeleteCourses(int id)
		{
			var course = _context.Courses.Find(id);

			if (course == null)
				return NotFound();

			var sections = _context.Sections
				.Where(s => s.CourseId == id)
				.ToList();

			if (sections.Any())
			{
				var sectionIds = sections.Select(s => s.SectionId).ToList();

				var sectionContents = _context.SectionContents
					.Where(sc => sc.SectionId.HasValue && sectionIds.Contains(sc.SectionId.Value))
					.ToList();

				if (sectionContents.Any())
					_context.SectionContents.RemoveRange(sectionContents);

				_context.Sections.RemoveRange(sections);
			}

			_context.Courses.Remove(course);
			_context.SaveChanges();

			return RedirectToAction("SeeAllCourses");
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("AdminLogin");
		}
		public IActionResult DeleteInstructor(int id)
		{
			var instructor = _context.Teachers.Find(id);
			if (instructor == null)
				return NotFound();
			_context.Teachers.Remove(instructor);
			_context.SaveChanges();
			return RedirectToAction("SeeAllInstructor");
		}
		public IActionResult statusInstructors()
		{
			var instructors = _context.Students.Where(i => i.IsTeaching == "pending").ToList();
			return View(instructors);
		}
		public IActionResult ApproveInstructor(int id)
		{
			var instructor = _context.Students.Find(id);
			if (instructor == null)
				return NotFound();
			instructor.IsTeaching = "yes";
			_context.SaveChanges();
			return RedirectToAction("statusInstructors");
		}
		public IActionResult DeniedInstructor(int id)
		{
			var instructor = _context.Students.Find(id);
			if (instructor == null)
				return NotFound();
			instructor.IsTeaching = "Denied";
			_context.SaveChanges();
			return RedirectToAction("statusInstructors");
		}
		public IActionResult Accept_courses()
		{
			var courses = _context.Courses.Where(c => c.IsApproved == "pending").ToList();
			return View(courses);
		}
		public IActionResult ApproveCourse(int id)
		{
			var course = _context.Courses.Find(id);
			if (course == null)
				return NotFound();
			course.IsApproved = "Approved";
			_context.SaveChanges();
			return RedirectToAction("Accept_courses");
		}

		public IActionResult DeniedCourse(int id)
		{
			var course = _context.Courses.Find(id);
			if (course == null)
				return NotFound();
			course.IsApproved = "Denied";
			_context.SaveChanges();
			return RedirectToAction("Accept_courses");
		}

	}
}
