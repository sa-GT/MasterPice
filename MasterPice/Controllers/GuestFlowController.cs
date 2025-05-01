using MasterPice.Models;
using MasterPice.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
//using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text;
using ContentDisposition = MimeKit.ContentDisposition;

namespace MasterPice.Controllers
{
	public class GuestFlowController : Controller
	{
		private readonly MyDbContext _context;

		public GuestFlowController(MyDbContext context)
		{
			_context = context;
		}
		public IActionResult About()
		{
			var feedbacks = _context.FeedBacks.ToList();
			return View(feedbacks);
		}
		public IActionResult Contact()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Contact(FeedBack feedBack)
		{
			if (feedBack != null)
			{
				_context.FeedBacks.Add(feedBack);
				_context.SaveChanges();
				return View();
			}
			return View();
		}
		public IActionResult Courses(string category)
		{
			var courses = _context.Courses.ToList();
			ViewBag.coursesCount = courses.Count;
			ViewBag.countweb = courses.Count(s => s.Category == "Web Development");
			ViewBag.countapp = courses.Count(s => s.Category == "App Development");
			ViewBag.countCyber = courses.Count(s => s.Category == "Cybersecurity");
			ViewBag.is_approved = courses.First().IsApproved;
			ViewBag.SelectedCategory = string.IsNullOrEmpty(category) ? "All" : category;

			var categories = _context.Categories.ToList();
			var model = Tuple.Create(courses, categories);

			return View(model);
		}

		public IActionResult Event()
		{
			var events = _context.Events.ToList();
			ViewBag.countevent = _context.Events.Count();
			return View(events);
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Register(Student student,string register_con)
		{
			if (student != null)
			{
				if(register_con == student.PasswordHash)
				{
					student.ProfilePicture = "data:image/jpeg;base64,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";
					student.CoverPicture = "https://images.unsplash.com/photo-1546410531-bb4caa6b424d?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8N3x8ZWR1Y2F0aW9ufGVufDB8fDB8fHww";
					_context.Students.Add(student);
					_context.SaveChanges();
					return RedirectToAction("Login");
				}
				// add student to database
			}
			return View();
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(string email, string password)
		{
			var student = _context.Students.FirstOrDefault(s => s.Email == email && s.PasswordHash == password);
			if (student != null)
			{
				HttpContext.Session.SetString("email", student.Email ?? "");
				HttpContext.Session.SetString("name", student.FullName ?? "");
				HttpContext.Session.SetInt32("id", student.StudentId);
				HttpContext.Session.SetString("profile", student.ProfilePicture ?? "");
				HttpContext.Session.SetString("interests", student.Interests ?? "");

				HttpContext.Session.SetString("date", student.CreatedAt != null ? student.CreatedAt.ToString() : "");
				HttpContext.Session.SetString("cover", student.CoverPicture ?? "");
				HttpContext.Session.SetString("isTeacher", student.IsTeaching ?? "false");

				MigrateSessionCartToDatabase(student.StudentId);

				var emailStudent = HttpContext.Session.GetString("email");

				var teacher = _context.Teachers.FirstOrDefault(t => t.Email == emailStudent);
				if (teacher != null)
				{
					HttpContext.Session.SetInt32("teacherid", teacher.TeacherId);
				}
				else
				{
					HttpContext.Session.Remove("teacherid");
				}

				return RedirectToAction("Home");
			}

			// ممكن تعرض رسالة فشل تسجيل الدخول هنا
			ViewBag.LoginError = "Invalid email or password";
			return View();
		}

		private void MigrateSessionCartToDatabase(int studentId)
		{
			string cart = HttpContext.Session.GetString("cart");

			if (!string.IsNullOrEmpty(cart))
			{
				var courseIds = cart.Split(',').Select(int.Parse).ToList();

				// تحقق إذا عنده سلة، إذا لا، أنشئ واحدة
				var cartData = _context.Carts.FirstOrDefault(c => c.StudentId == studentId);
				if (cartData == null)
				{
					cartData = new Cart { StudentId = studentId };
					_context.Carts.Add(cartData);
					_context.SaveChanges();
				}

				// أضف الكورسات إذا مش موجودة
				foreach (var courseId in courseIds)
				{
					bool exists = _context.CartItems.Any(c => c.CartId == cartData.CartId && c.CourseId == courseId);
					if (!exists)
					{
						_context.CartItems.Add(new CartItem
						{
							CartId = cartData.CartId,
							CourseId = courseId
						});
					}
				}

				_context.SaveChanges();

				// نظف الـ Session
				HttpContext.Session.Remove("cart");
				HttpContext.Session.SetInt32("CartCount", 0);
			}
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Home");
		}
		public IActionResult Become_Teacher()
		{
			int? getint = HttpContext.Session.GetInt32("id");
			if (getint != null)
			{
				var student = _context.Students.Find(getint);
				if (student != null)
				{
					ViewBag.Email = student.Email;
					ViewBag.Name = student.FullName;
				}
			}
			return View();
		}
		[HttpPost]
		public IActionResult Become_Teacher(Teacher teacher, string con_passwordcon)
		{
			int? getint = HttpContext.Session.GetInt32("id");
			if (getint == null)
			{
				if(teacher.PasswordHash == con_passwordcon)
				{
					if(HttpContext.Session.GetInt32("id") == null)
					{
						return RedirectToAction("Register");
					}
					else
					{
						_context.Teachers.Add(teacher);
						_context.SaveChanges();
						TempData["Success"] = "Success Application Added";
					}
				}
				else
				{
					TempData["Error"] = "Password Not Match";
				}
			}
			else
			{
				var student = _context.Students.Find(getint);
				if (student != null)
				{
					student.IsTeaching = "pending";
					ViewBag.Email = student.Email;
					ViewBag.Name = student.FullName;
					_context.SaveChanges();
					teacher.ProfilePicture = student.ProfilePicture;
					teacher.PasswordHash = student.PasswordHash;
					_context.Teachers.Add(teacher);
					_context.SaveChanges();
					TempData["Success"] = "Success Application Added";
				}
			}
			return View();
		}
		public IActionResult Home()
		{
			var courses = _context.Courses.Take(3).ToList();
			var cat = _context.Categories.ToList();
			var studentcount = _context.Students.Count();
			ViewBag.StudentCount = studentcount;
			var coursesCount = _context.Courses.Count();
			ViewBag.CoursesCount = coursesCount;
			var eventsCount = _context.Events.Count();
			ViewBag.EventsCount = eventsCount;
			var studentsEnrolled = _context.EnrollmentUsers.Count();
			ViewBag.StudentsEnrolled = studentsEnrolled;
			var events = _context.Events.ToList();
			var feedbacks = _context.FeedBacks.ToList();
			var currentdate = DateTime.Now.Month;
			var upcomoingevents = _context.Events.Where(e => e.StartDate.Month > currentdate).ToList();
			var model = Tuple.Create(courses, cat,feedbacks, upcomoingevents);

			var countsecurity = _context.Courses.Count(s => s.Category == "Cybersecurity");
			var countweb = _context.Courses.Count(s => s.Category == "Web Development");
			var countapp = _context.Courses.Count(s => s.Category == "App Development");
			ViewBag.countweb = countweb;
			ViewBag.countapp = countapp;
			ViewBag.countsecurity = countsecurity;
			return View(model);
		}
		public IActionResult Cart()
		{
			int? userId = HttpContext.Session.GetInt32("id");

			List<Course> courses = new List<Course>();

			if (userId != null)
			{
				// ✅ إذا المستخدم مسجل دخول
				var student = _context.Students.Find(userId);
				var cart = _context.Carts.FirstOrDefault(c => c.StudentId == student.StudentId);

				if (cart != null)
				{
					var cartItems = _context.CartItems
						.Where(c => c.CartId == cart.CartId)
						.ToList();

					var courseIds = cartItems.Select(c => c.CourseId).ToList();

					courses = _context.Courses
						.Where(c => courseIds.Contains(c.CourseId))
						.ToList();
				}
			}
			else
			{
				// ✅ إذا الزائر (مش مسجل دخول)
				string sessionCart = HttpContext.Session.GetString("cart");

				if (!string.IsNullOrEmpty(sessionCart))
				{
					var courseIds = sessionCart.Split(',').Select(int.Parse).ToList();

					courses = _context.Courses
						.Where(c => courseIds.Contains(c.CourseId))
						.ToList();
				}
			}

			// ✅ مجموع الأسعار
			ViewBag.coursecountprice = courses.Sum(c => c.Price);

			// ✅ إرسال الكورسات للسلة
			return View(courses);
		}


		public IActionResult Checkout(int? id)
		{
			if(id == null)
			{
				var userId = HttpContext.Session.GetInt32("id");
				var courseid = HttpContext.Session.GetInt32("courseId");

				var cart = _context.Carts.FirstOrDefault(c => c.StudentId == userId);

				var cartitems = _context.CartItems
					.Where(c => c.CartId == cart.CartId).Select(s => s.CourseId);

				var courses = _context.Courses.Where(c => cartitems.Contains(c.CourseId))
					.ToList();
				var model = new CheckoutViewModel
				{
					Courses = courses,
					Payment = new Payment()
				};
				ViewBag.coursecountprice = courses.Sum(c => c.Price);
				return View(model);
			}
			else
			{
				var userId = HttpContext.Session.GetInt32("id");
				var courseid = HttpContext.Session.GetInt32("courseId");

				//var cart = _context.Carts.FirstOrDefault(c => c.StudentId == userId);


				//var cartitems = _context.CartItems
				//	.Where(c => c.CartId == cart.CartId).Select(s => s.CourseId);

				var courses = _context.Courses.Where(c => c.CourseId == id)
					.ToList();
				var model = new CheckoutViewModel
				{
					Courses = courses,
					Payment = new Payment()
				};
				ViewBag.coursecountprice = courses.Sum(c => c.Price);
				return View(model);
			}
		}
		[HttpPost]
		public IActionResult Checkout(Payment payment)
		{
			var userId = HttpContext.Session.GetInt32("id");

			var cart = _context.Carts.FirstOrDefault(c => c.StudentId == userId);
			var courseIds = _context.CartItems
				.Where(c => c.CartId == cart.CartId)
				.Select(c => c.CourseId)
				.ToList();
			var courses = _context.Courses
				.Where(c => courseIds.Contains(c.CourseId))
				.ToList();
			if (courses.Count == 0)
				return BadRequest("Cart is empty");
			payment.CartId = cart.CartId;
			payment.StudentId = userId.Value;
			payment.IsPayed = true;
			payment.TotalCost = courses.Sum(c => c.Price);
			_context.Payments.Add(payment);
			_context.SaveChanges();

			var order = new Order
			{
				CartId = cart.CartId,
				PaymentId = payment.PaymentId
			};
			_context.Orders.Add(order);
			_context.SaveChanges();

			foreach (var course in courses)
			{
				bool alreadyPaid = _context.OrderDetails
					.Any(od => od.StudentId == userId.Value && od.CourseId == course.CourseId);

				if (alreadyPaid)
					continue;

				var orderDetail = new OrderDetail
				{
					OrderId = order.OrderId,
					CourseId = course.CourseId,
					StudentId = userId.Value
				};

				_context.OrderDetails.Add(orderDetail);
				course.EnrolledStudentsCount++;
				_context.Courses.Update(course);
				_context.SaveChanges();
				var itemsToRemove = _context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();

				_context.CartItems.RemoveRange(itemsToRemove);

				var enrolled = new EnrollmentUser
				{
					StudentId = userId,
					CourseId = course.CourseId
				};
				_context.EnrollmentUsers.Add(enrolled);
				_context.SaveChanges();
			}

			_context.SaveChanges();

			TempData["suuceesPay"] = "You Are Payed Successfully";
			return RedirectToAction("PaymentResult");
		}


		public IActionResult event_detailed()
		{
			return View();
		}
		public IActionResult AddToCart(int courseId, string returnUrl)
		{
			// 1. قراءة السلة من الـ Session
			string cart = HttpContext.Session.GetString("cart");
			int? userid = HttpContext.Session.GetInt32("id");
			var student = userid != null ? _context.Students.Find(userid) : null;
			if (userid != null)
			{
				var cartdata = _context.Carts.FirstOrDefault(c => c.StudentId == student.StudentId);
				if (cartdata != null) {
					if (cart == null)
					{

						var cartitem = _context.CartItems.Where(c => c.CartId == cartdata.CartId);
						var MatchCourse = cartitem.FirstOrDefault(c => c.CourseId == courseId);
						if (MatchCourse == null)
						{
							_context.CartItems.Add(new CartItem
							{
								CartId = cartdata.CartId,
								CourseId = courseId
							});
							_context.SaveChanges();
						}
						else
						{
							// الكورس موجود بالفعل في السلة
							TempData["Errors"] = "Course already in cart";
						}
					}
				}
			}
			else
			{
				// 2. تحويل السلسلة إلى List
				List<string> cartItems = string.IsNullOrEmpty(cart)
					? new List<string>()
					: cart.Split(',').ToList();

				// 3. إضافة الكورس إذا مش موجود
				if (!cartItems.Contains(courseId.ToString()))
				{
					cartItems.Add(courseId.ToString());
					HttpContext.Session.SetString("cart", string.Join(",", cartItems));
				}

				// ✅ 4. حفظ عدد الكورسات في Session (تستخدمه بالـ Layout)
				HttpContext.Session.SetInt32("CartCount", cartItems.Count);
			}

			// 5. الرجوع لنفس الصفحة
			return Redirect(returnUrl ?? "/");
		}



		public IActionResult RemoveFromCart(int courseId)
		{
			string cart = HttpContext.Session.GetString("cart");

			if (!string.IsNullOrEmpty(cart))
			{
				List<string> cartItems = cart.Split(',').ToList();
				cartItems.Remove(courseId.ToString());

				HttpContext.Session.SetString("cart", string.Join(",", cartItems));
			}

			return RedirectToAction("Cart");
		}

		public IActionResult DeleteCartItem(int courseId)
		{
			var cartItem = _context.CartItems.FirstOrDefault(s=>s.CourseId==courseId);
			if (cartItem != null)
			{
				_context.CartItems.Remove(cartItem);
				_context.SaveChanges();
			}
			return RedirectToAction("Cart");
		}

		public IActionResult PaymentResult()
		{
			return View();
		}

		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ForgetPassword(string Email)
		{
			try
			{
				var EmailFrom = "sajatest255@gmail.com";
				var EmailPassword = "ywas oije plbx yikk"; // App Password

				// جلب الطالب حسب الإيميل
				var student = _context.Students.FirstOrDefault(s => s.Email == Email);

				if (student == null)
				{
					ModelState.AddModelError("Email", "This email is not registered.");
					return View();
				}

				// توليد كلمة مرور جديدة (4 أرقام)
				Random random = new Random();
				string newPassword = random.Next(1000, 9999).ToString();

				// تحديث كلمة السر مباشرة (بدون تشفير)
				student.PasswordHash = newPassword;
				_context.SaveChanges();

				// إعداد الإيميل
				var email = new MimeMessage();
				email.From.Add(MailboxAddress.Parse(EmailFrom));
				email.To.Add(MailboxAddress.Parse(student.Email));
				email.Subject = "Your New Password";

				email.Body = new TextPart("plain")
				{
					Text = $"Your new temporary password is: {newPassword}\n\nPlease log in and change it as soon as possible."
				};

				// إرسال الإيميل
				using var smtp = new SmtpClient();

				// تجاوز مشكلة الشهادة أثناء التطوير فقط
				smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

				smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
				smtp.Authenticate(EmailFrom, EmailPassword);
				smtp.Send(email);
				smtp.Disconnect(true);

				TempData["Success"] = "A new password has been sent to your email.";
				return RedirectToAction("Login", "GuestFlow");
			}
			catch (Exception ex)
			{
				Console.WriteLine("EMAIL ERROR: " + ex.ToString());
				TempData["Error"] = "Something went wrong, please try again.";
				return View();
			}
		}



	}
}
