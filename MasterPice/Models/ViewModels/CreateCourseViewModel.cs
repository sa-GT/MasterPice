namespace MasterPice.Models.ViewModels
{
	public class CreateCourseViewModel
	{
		public Course Course { get; set; } = new Course();
		public Section Section { get; set; } = new Section();
		public SectionContent SectionContent { get; set; } = new SectionContent();
		public List<Section>? ExistingSections { get; set; }
		public List<SectionContent>? ExistingSectionContents { get; set; }
		public List<Course>? AllCourses { get; set; }
		public List<Course> ExistingCourses { get; set; } = new();

	}
}