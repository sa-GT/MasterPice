using MasterPice.Models;
using MasterPice.Models.ViewModels;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

namespace MasterPice.Controllers
{
	public class InstructorController : Controller
	{
		private readonly MyDbContext _context;
		public InstructorController(MyDbContext context)
		{
			_context = context;
		}
		public IActionResult Instructor_dashboard()
		{
			return View();
		}
		//public IActionResult Create_courses(int? id)
		//{
		//	int idTeacher = id ?? HttpContext.Session.GetInt32("idTeacher") ?? 0;
		//	if (idTeacher == 0)
		//		return NotFound("Teacher ID not provided.");

		//	HttpContext.Session.SetInt32("idTeacher", idTeacher);

		//	var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == idTeacher);
		//	if (teacher == null)
		//		return NotFound("Teacher not found.");

		//	int? sessionCourseId = HttpContext.Session.GetInt32("courseid");

		//	Course course = new Course(); // دايمًا نبدأ بفورم فاضي
		//	List<MasterPice.Models.Section> sections = new List<MasterPice.Models.Section>();
		//	List<SectionContent> contents = new List<SectionContent>();

		//	if (sessionCourseId.HasValue)
		//	{
		//		course = _context.Courses.FirstOrDefault(c => c.CourseId == sessionCourseId.Value);
		//		if (course != null)
		//		{
		//			sections = _context.Sections.Where(s => s.CourseId == course.CourseId).ToList();
		//			var sectionIds = sections.Select(s => s.SectionId).ToList();

		//			if (sectionIds.Any())
		//			{
		//				contents = _context.SectionContents
		//					.Where(sc => sc.SectionId.HasValue && sectionIds.Contains(sc.SectionId.Value))
		//					.ToList();
		//			}
		//		}
		//	}

		//	var model = new CreateCourseViewModel
		//	{
		//		Course = course,
		//		ExistingSections = sections,
		//		ExistingSectionContents = contents,
		//		AllCourses = _context.Courses.Where(c => c.TeacherId == idTeacher).ToList()
		//	};

		//	return View(model);
		//}

		public IActionResult Create_courses(int? id, int? courseId)
		{
			int idTeacher = id ?? HttpContext.Session.GetInt32("idTeacher") ?? 0;
			if (idTeacher == 0)
				return NotFound("Teacher ID not provided.");

			HttpContext.Session.SetInt32("idTeacher", idTeacher);

			var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == idTeacher);
			if (teacher == null)
				return NotFound("Teacher not found.");
			int? activeCourseId = courseId ?? HttpContext.Session.GetInt32("courseid");
			if (activeCourseId.HasValue)
				HttpContext.Session.SetInt32("courseid", activeCourseId.Value);

			var allCourses = _context.Courses.Where(c => c.TeacherId == idTeacher).ToList();

			Course course = new Course();
			List<MasterPice.Models.Section> sections = new();
			List<SectionContent> contents = new();

			if (activeCourseId.HasValue)
			{
				course = _context.Courses.FirstOrDefault(c => c.CourseId == activeCourseId.Value) ?? new Course();

				sections = _context.Sections.Where(s => s.CourseId == course.CourseId).ToList();
				var sectionIds = sections.Select(s => s.SectionId).ToList();

				contents = _context.SectionContents
					.Where(sc => sc.SectionId.HasValue && sectionIds.Contains(sc.SectionId.Value))
					.ToList();
			}

			var model = new CreateCourseViewModel
			{
				Course = course,
				ExistingSections = sections,
				ExistingSectionContents = contents,
				AllCourses = allCourses
			};

			return View(model);
		}





		public IActionResult Instructor_profile()
		{
			return View();
		}
		public IActionResult Instructor_courses()
		{
			return View();
		}
		public IActionResult Instractor_setting()
		{
			return View();
		}
		public IActionResult Instructor_attepmts_quiz()
		{
			return View();
		}
		public IActionResult Instructor_assignments()
		{
			return View();
		}
		//[HttpPost]
		//public IActionResult CreateCourses(CreateCourseViewModel model)
		//{
		//	var course = model.Course;
		//	var section = model.Section;

		//	int? idTeacher = HttpContext.Session.GetInt32("idTeacher");
		//	if (idTeacher == null) return RedirectToAction("Login", "Account");



		//	var existingCourse = _context.Courses
		//		.FirstOrDefault(c => c.TeacherId == idTeacher && c.Title == "New Course");

		//	// إذا الكورس موجود ومجرد placeholder (New Course) → نحدثه
		//	if (existingCourse != null)
		//	{
		//		existingCourse.Title = course.Title;
		//		existingCourse.Description = course.Description;
		//		existingCourse.Level = course.Level;
		//		existingCourse.VideoLink = course.VideoLink;
		//		existingCourse.Category = course.Category;
		//		existingCourse.Price = course.Price ?? 0;
		//		existingCourse.CourseCover = course.CourseCover;

		//		_context.Courses.Update(existingCourse);
		//		_context.SaveChanges();
		//	}
		//	else
		//	{
		//		// الكورس جديد تمامًا
		//		course.TeacherId = idTeacher.Value;
		//		course.Price = course.Price ?? 0;

		//		_context.Courses.Add(course);
		//		_context.SaveChanges();

		//		existingCourse = course;
		//	}
		//	// حفظ رقم الكورس في السيشن
		//	HttpContext.Session.SetInt32("courseid", existingCourse.CourseId);

		//	// إضافة سكشن جديد
		//	section.CourseId = existingCourse.CourseId;
		//	section.Title = section.Title ?? course.Title; // لتفادي null

		//	_context.Sections.Add(section);
		//	_context.SaveChanges();

		//	var sectionId = section.SectionId;
		//	HttpContext.Session.SetInt32("sectionid", sectionId);

		//	return RedirectToAction("Create_courses");
		//}
		[HttpPost]
		public IActionResult CreateCourses(CreateCourseViewModel model)
		{
			var course = model.Course;
			var section = model.Section;

			int? idTeacher = HttpContext.Session.GetInt32("idTeacher");
			if (idTeacher == null) return RedirectToAction("Login", "Account");

			var existingCourse = _context.Courses
				.FirstOrDefault(c => c.TeacherId == idTeacher && c.Title == "New Course");

			if (existingCourse != null)
			{
				existingCourse.Title = course.Title;
				existingCourse.Description = course.Description;
				existingCourse.Level = course.Level;
				existingCourse.VideoLink = course.VideoLink;
				existingCourse.Category = course.Category;
				existingCourse.Price = course.Price ?? 0;
				existingCourse.CourseCover = course.CourseCover;

				_context.Courses.Update(existingCourse);
				_context.SaveChanges();
			}
			else
			{
				course.TeacherId = idTeacher.Value;
				course.Price = course.Price ?? 0;
				course.IsApproved = "Pending"; // تعيين القيمة الافتراضية

				_context.Courses.Add(course);
				_context.SaveChanges();

				existingCourse = course;
			}

			HttpContext.Session.SetInt32("courseid", existingCourse.CourseId);

			section.CourseId = existingCourse.CourseId;
			section.Title = section.Title ?? course.Title;

			_context.Sections.Add(section);
			_context.SaveChanges();

			var sectionId = section.SectionId;
			HttpContext.Session.SetInt32("sectionid", sectionId);

			return RedirectToAction("Create_courses", new { courseId = existingCourse.CourseId });
		}




		[HttpPost]
		public IActionResult AddSectionWithContent(CreateCourseViewModel model,int sectionID)
		{
			int? courseid = HttpContext.Session.GetInt32("courseid");

			var sections = _context.Sections.Where(s => s.CourseId == courseid && s.SectionId == sectionID).Select(s=>s.SectionId).ToList();

			// حفظ المحتوى المرتبط بالقسم
			var content = model.SectionContent;
			content.SectionId = sections.First() ;
			_context.SectionContents.Add(content);
			_context.SaveChanges();

			return RedirectToAction("Create_courses"); // حتى يظهر القسم المضاف
		}

		[HttpPost]
		public IActionResult AddSection(CreateCourseViewModel model, string Sectionsss)
		{
			int? courseId = HttpContext.Session.GetInt32("courseid");
			if (courseId == null)
				return RedirectToAction("Create_courses");

			var newSection = new MasterPice.Models.Section
			{
				CourseId = courseId.Value,
				Title = Sectionsss
			};

			_context.Sections.Add(newSection);
			_context.SaveChanges();

			return RedirectToAction("Create_courses", new { courseId = courseId.Value });
		}
		[HttpPost]
		public IActionResult DeleteSection(int id)
		{
			var section = _context.Sections.FirstOrDefault(s => s.SectionId == id);

			var sectionContent = _context.SectionContents.Where(s => s.SectionId == section.SectionId).ToList();
			if (section != null)
			{
				if(sectionContent != null)
				{
					_context.SectionContents.RemoveRange(sectionContent);
					_context.SaveChanges();
				}
					_context.Sections.Remove(section);
					_context.SaveChanges();
			}

			return RedirectToAction("Create_courses");
		}
		[HttpPost]
		public IActionResult RemoveSectionContent(int id)
		{
			var content = _context.SectionContents.FirstOrDefault(sc => sc.ContentId == id);
			if (content != null)
			{
				_context.SectionContents.Remove(content);
				_context.SaveChanges();
			}
			return RedirectToAction("Create_courses");
		}
	}
}
