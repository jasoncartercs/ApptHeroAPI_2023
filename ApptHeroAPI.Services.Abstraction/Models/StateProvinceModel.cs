namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class StateProvinceModel
    {
        public static string DataValueField = "StateProvinceId";
        public static string DataTextField = "StateName";
        public static int USA = 1;
        public int StateProvinceId { get; set; }
        public int CountryId { get; set; }

        public string StateCode { get; set; }

        public string StateName { get; set; }
    }
}