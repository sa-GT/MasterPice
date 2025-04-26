namespace MasterPice.Models.ViewModels
{
	public class LessonViewModel
	{
		public List<Course> coursess { get; set; }
		public List<Section> sectionss { get; set; }
		public List<SectionContent> sectionContentss { get; set; }

		public SectionContent? SelectedContent { get; set; } // محتوى الدرس المفتوح

	}
}


