using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MasterPice.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<EnrollmentUser> EnrollmentUsers { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventFeedback> EventFeedbacks { get; set; }

    public virtual DbSet<EventRegistration> EventRegistrations { get; set; }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<SectionContent> SectionContents { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-6263BV65;Database=MasterPice;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4887469410E");

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D1053426BEF2D2").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.ProfilePicture).HasMaxLength(255);
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__Assignme__32499E5724A5BF27");

            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Course).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Assignmen__Cours__7E37BEF6");
        });

        modelBuilder.Entity<AssignmentSubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__Assignme__449EE10535344D42");

            entity.ToTable("Assignment_Submissions");

            entity.Property(e => e.SubmissionId).HasColumnName("SubmissionID");
            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.StudnetsId).HasColumnName("StudnetsID");
            entity.Property(e => e.SubmissionFile).HasMaxLength(255);
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentSubmissions)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("FK__Assignmen__Assig__01142BA1");

            entity.HasOne(d => d.Studnets).WithMany(p => p.AssignmentSubmissions)
                .HasForeignKey(d => d.StudnetsId)
                .HasConstraintName("FK__Assignmen__Studn__02084FDA");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797CD509C0C");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.Carts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Student");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B2A50A34AFF");

            entity.ToTable("CartItem");

            entity.HasIndex(e => new { e.CartId, e.CourseId }, "UQ_CartItem_Cart_Course").IsUnique();

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.Course).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Course");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B5FC06997");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0B4D9BAE6").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("text");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__Chat__A9FBE626E49F9B5E");

            entity.ToTable("Chat");

            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.ReceiverTid).HasColumnName("Receiver_TID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SentAt).HasColumnType("datetime");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAA2BDB7EBC");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentText).HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71875F1A1C70");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CourseCover).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EnrolledStudentsCount).HasDefaultValue(0);
            entity.Property(e => e.IsApproved)
                .IsUnicode(false)
                .HasColumnName("is_approved");
            entity.Property(e => e.LessonCount).HasDefaultValue(0);
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.Price)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.VideoLink).IsUnicode(false);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Courses__Teacher__4316F928");
        });

        modelBuilder.Entity<EnrollmentUser>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F6877FB8CDE5427");

            entity.ToTable("Enrollment_user");

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.EnrolledAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.EnrollmentUsers)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Enrollmen__Cours__49C3F6B7");

            entity.HasOne(d => d.Student).WithMany(p => p.EnrollmentUsers)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Enrollmen__Stude__48CFD27E");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C870C7B9E9FC");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.CreatedBy).IsUnicode(false);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.EventCover)
                .IsUnicode(false)
                .HasColumnName("Event_cover");
            entity.Property(e => e.EventType).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<EventFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Event_Fe__6A4BEDF621E84579");

            entity.ToTable("Event_Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventFeedbacks)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Event_Fee__Event__1AD3FDA4");

            entity.HasOne(d => d.Student).WithMany(p => p.EventFeedbacks)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Event_Fee__Stude__1BC821DD");
        });

        modelBuilder.Entity<EventRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Event_Re__6EF588300501B8D8");

            entity.ToTable("Event_Registrations");

            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.RegisteredAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Event_Reg__Event__160F4887");

            entity.HasOne(d => d.Student).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Event_Reg__Stude__17036CC0");
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK_feedback");

            entity.ToTable("feedBack");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.FeedbackContent)
                .IsUnicode(false)
                .HasColumnName("feedback_content");
            entity.Property(e => e.Username)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Games__2AB897DDC29430DB");

            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.GameType).HasMaxLength(100);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32C7CCCD14");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF78B6CF00");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

            entity.HasOne(d => d.Cart).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__CartID__690797E6");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__PaymentI__681373AD");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30CFCD4C3B4");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Cours__6DCC4D03");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__6BE40491");

            entity.HasOne(d => d.Student).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Stude__6CD828CA");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58F52A0E18");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsPayed).HasDefaultValue(false);
            entity.Property(e => e.PaidAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(30);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cart).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__CartID__65370702");

            entity.HasOne(d => d.Student).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Studen__6442E2C9");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Posts__AA126038BE3D5EBC");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8C65FA0A5D");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.CoursesId).HasColumnName("CoursesID");
            entity.Property(e => e.QuestionText).HasColumnType("text");
            entity.Property(e => e.QuestionType).HasMaxLength(50);
            entity.Property(e => e.QuizId).HasColumnName("QuizID");

            entity.HasOne(d => d.Courses).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CoursesId)
                .HasConstraintName("FK_Questions_Courses");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK__Sections__80EF08925C93CACB");

            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Course).WithMany(p => p.Sections)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sections_Courses");
        });

        modelBuilder.Entity<SectionContent>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__Section___2907A87E03F12C9D");

            entity.ToTable("Section_Content");

            entity.Property(e => e.ContentId).HasColumnName("ContentID");
            entity.Property(e => e.ContentType).HasMaxLength(50);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Section).WithMany(p => p.SectionContents)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_Section_Content_Section");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A793962A294");

            entity.HasIndex(e => e.Email, "UQ__Students__A9D1053414CC82C1").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.CoverPicture)
                .IsUnicode(false)
                .HasColumnName("coverPicture");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Interests).HasColumnType("text");
            entity.Property(e => e.IsTeaching)
                .IsUnicode(false)
                .HasColumnName("is_Teaching");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.ProfilePicture).IsUnicode(false);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF259445789BF82");

            entity.HasIndex(e => e.Email, "UQ__Teachers__A9D105348F880805").IsUnique();

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.Bio).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
