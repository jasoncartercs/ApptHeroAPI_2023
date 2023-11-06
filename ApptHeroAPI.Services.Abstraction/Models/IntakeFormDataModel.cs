namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class IntakeFormDataModel
    {
        public long IntakeFormId { get; set; }
        public int IntakeFormCategoryId { get; set; }

        public int? ParentId { get; set; }

        public string FormData { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }
    }
}