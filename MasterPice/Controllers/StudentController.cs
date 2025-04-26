using MasterPice.Models;
using MasterPice.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MasterPice.Controllers
{
	public class StudentController : Controller
	{
		private readonly MyDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public StudentController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Student_dashboard()
		{
			return View();
		}
		public IActionResult Course_detailed(int id)
		{
			// جلب الكورس
			var courses = _context.Courses.Where(s => s.CourseId == id).ToList();
			HttpContext.Session.SetInt32("courseId", id); 

			// تحقق من وجود الكورس
			if (courses == null || !courses.Any())
			{
				return NotFound();
			}

			var course = courses.First();

			// جلب معرف الطالب من الجلسة
			var userId = HttpContext.Session.GetInt32("id");

			// تحقق إن كان الطالب مسجل في الكورس
			var enrolledCourseIds = _context.EnrollmentUsers
				.Where(s => s.StudentId == userId)
				.Select(s => s.CourseId)
				.ToList();

			TempData["enrol"] = enrolledCourseIds.Contains(id) ? "true" : "false";

			// بيانات الكورس
			ViewBag.LeveL = course.Level ?? "Not Available";
			ViewBag.std_count = course.EnrolledStudentsCount;

			// الأقسام التابعة للكورس
			var courseSections = _context.Sections.Where(s => s.CourseId == id).ToList();
			ViewBag.sectionCount = courseSections.Count;

			// المحتوى المرتبط بالأقسام
			var sectionIds = courseSections.Select(s => s.SectionId).ToList();
			var sectionContents = _context.SectionContents
				.Where(sc => sc.SectionId != null && sectionIds.Contains(sc.SectionId.Value))
				.ToList();

			// جلب بيانات المعلم
			var teacher = _context.Teachers.FirstOrDefault(s => s.TeacherId == course.TeacherId);
			ViewBag.TeacherP = teacher?.ProfilePicture;
			ViewBag.Teachername = teacher?.FullName;
			ViewBag.TeacherBio = teacher?.Bio;

			// عدد الكورسات التي يقدمها المعلم
			ViewBag.count_teacher_courses = _context.Courses.Count(s => s.TeacherId == teacher.TeacherId);

			// عدد الطلاب المسجلين في كورسات المعلم
			var teacherCourses = _context.Courses
				.Where(s => s.TeacherId == teacher.TeacherId)
				.Select(c => c.CourseId)
				.ToList();

			ViewBag.count_students = _context.EnrollmentUsers
				.Count(s => s.CourseId != null && teacherCourses.Contains(s.CourseId.Value));
			ViewBag.courseprice = course.Price ?? 0;

			// إنشاء النموذج الثلاثي
			var model = Tuple.Create(courses, courseSections, sectionContents);
			return View(model);
		}


		//public IActionResult is_erolled()
		//{
		//	var data = HttpContext.Session.GetInt32("id");

		//	if (data == null)
		//	{
		//		TempData["login_required"] = "true";
		//		return RedirectToAction("Login", "GuestFlow"); // رجعه لنفس الصفحة مثلاً
		//	}
		//	// هنا تكتب المنطق لو كان المستخدم مسجل دخولًا فعلاً
		//	return View(); 
		//}


		public IActionResult Category_courses()
		{
			return View();
		}
		public IActionResult Lesson_intro(int id)
		{
			Course course = null;
			SectionContent selectedContent = null;

			// أولاً: نحاول نجيب الدرس إذا الإيد هو ContentId
			selectedContent = _context.SectionContents.FirstOrDefault(c => c.ContentId == id);

			if (selectedContent != null)
			{
				// جلب السكشن والدرس التابع له
				var section = _context.Sections.FirstOrDefault(s => s.SectionId == selectedContent.SectionId);
				if (section == null) return NotFound();

				// جلب الكورس المرتبط بالسكشن
				course = _context.Courses.FirstOrDefault(c => c.CourseId == section.CourseId);
				if (course == null) return NotFound();

				// حفظ CourseId في السيشن إن لم يكن محفوظ
				if (!HttpContext.Session.Keys.Contains("lesson_courseId"))
					HttpContext.Session.SetInt32("lesson_courseId", course.CourseId);
			}
			else
			{
				// الحالة الثانية: المستخدم دخل مباشرة باستخدام CourseId
				course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
				if (course == null) return NotFound();

				// حفظ CourseId في السيشن إن لم يكن محفوظ
				if (!HttpContext.Session.Keys.Contains("lesson_courseId"))
					HttpContext.Session.SetInt32("lesson_courseId", course.CourseId);
			}

			// جلب الأقسام والمحتويات الخاصة بهذا الكورس فقط
			var allSections = _context.Sections
				.Where(s => s.CourseId == course.CourseId)
				.ToList();

			var sectionIds = allSections.Select(s => s.SectionId).ToList();

			var allSectionContents = _context.SectionContents
				.Where(sc => sc.SectionId.HasValue && sectionIds.Contains(sc.SectionId.Value))
				.ToList();

			// إذا ما ضغط على محتوى معين، نعرض أول محتوى
			if (selectedContent == null)
			{
				selectedContent = allSectionContents.FirstOrDefault();
			}

			// تعديل رابط يوتيوب إذا كان موجود
			if (selectedContent != null && selectedContent.ContentText?.Contains("watch?v=") == true)
			{
				selectedContent.ContentText = selectedContent.ContentText.Replace("watch?v=", "embed/");
			}

			var model = new LessonViewModel
			{
				coursess = new List<Course> { course },
				sectionss = allSections,
				sectionContentss = allSectionContents,
				SelectedContent = selectedContent
			};

			return View(model);
		}
		public IActionResult Lesson_Quiz()
		{
			return View();
		}
		public IActionResult Lesson_quiz_result()
		{
			return View();
		}
		public IActionResult Lesson_assignments()
		{
			return View();
		}
		public IActionResult Lesson_assignment_submit()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Student_settings()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Student_settings(Student student, IFormFile ProfilePicture)
		{
			try
			{
				var existingStudent = _context.Students.FirstOrDefault(s => s.Email == student.Email);
				if (existingStudent == null)
				{
					return NotFound();
				}

				// تحديث المعلومات الأساسية
				existingStudent.FullName = student.FullName;
				existingStudent.Interests = student.Interests;

				// تحديث الصورة إذا تم رفعها
				if (ProfilePicture != null && ProfilePicture.Length > 0)
				{
					var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
					if (!Directory.Exists(uploadsFolder))
					{
						Directory.CreateDirectory(uploadsFolder);
					}

					var uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePicture.FileName;
					var filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await ProfilePicture.CopyToAsync(fileStream);
					}

					existingStudent.ProfilePicture = "/uploads/" + uniqueFileName;
				}

				_context.Update(existingStudent);
				await _context.SaveChangesAsync();

				// تحديث الجلسة
				HttpContext.Session.SetString("name", existingStudent.FullName);
				HttpContext.Session.SetString("profile", existingStudent.ProfilePicture ?? "");
				HttpContext.Session.SetString("interests", existingStudent.Interests ?? "");

				TempData["SuccessMessage"] = "تم تحديث الملف الشخصي بنجاح!";
				return RedirectToAction("Student_settings");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "حدث خطأ أثناء تحديث الملف الشخصي: " + ex.Message);
				return View(student);
			}
		}
		[HttpPost]
		public IActionResult ResetPassword(Student student, string Current,string rewrite)
		{
			if(student.PasswordHash == rewrite)
			{
				var existingStudent = _context.Students.FirstOrDefault(s => s.PasswordHash == Current);
				if (existingStudent == null)
				{
					return NotFound();
				}
				existingStudent.PasswordHash = student.PasswordHash;
				_context.Update(existingStudent);
				_context.SaveChanges();
				TempData["SuccessMessage"] = "تم تغيير كلمة المرور بنجاح!";
				return RedirectToAction("Student_settings");
			}
			else
			{
				ModelState.AddModelError("", "كلمة المرور الحالية غير صحيحة");
				return View(student);
			}
		}
		public IActionResult Student_profile()
		{
			return View();
		}
		public IActionResult Student_enrolled_courses()
		{
			var studentId = HttpContext.Session.GetInt32("id");
			var courses = _context.EnrollmentUsers
				.Where(e => e.StudentId == studentId)
				.Select(e => e.Course) 
				.ToList();
			
			var isCompleted = _context.EnrollmentUsers
				.Where(e => e.StudentId == studentId)
				.Select(e => e.IsCompleted)
				.ToList();
			HttpContext.Session.SetInt32("isCompleted", isCompleted.Count);
			return View(courses);
		}

		[HttpGet]
		public IActionResult CheckLogin()
		{
			var data = HttpContext.Session.GetInt32("id");
			if(data != null)
			{
				return RedirectToAction("Checkout","GuestFlow");
			}
			else
			{
				TempData["login_required"] = "true";
				return RedirectToAction("Login", "GuestFlow");
			}
		}


		public async Task<IActionResult> EditCover(IFormFile ProfilePicture)
		{
			var studentId = HttpContext.Session.GetInt32("id");
			var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId);

			if (student != null && ProfilePicture != null)
			{
				var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
				var filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await ProfilePicture.CopyToAsync(fileStream);
				}

				student.CoverPicture = "/uploads/" + uniqueFileName;
				_context.Update(student);
				await _context.SaveChangesAsync();
			}

			return View(); 
		}

		public IActionResult Student_wishlist()
		{
			return View();
		}
	}
}
