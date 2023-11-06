using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class MassageFeaturesModel
    {
        public const string SchedulerFeatureName = "Scheduler";
        public const string TextBackFeatureName = "TextBack";
        public const string BotFeatureName = "Bot";
        public const string CovidFormName = "Covid";
        public const string MultiUser = "Multi-user";
        public const string BasicVersionFeatureName = "BasicApptHero";

        public static int SchedulerFeatureId = 1;
        public static int TextBackFeatureId = 2;
        public static int BptFeatureId = 3;
        public static int CovidFeatureId = 4;
        public static int MultiUserFeatureId = 5;
        public static int BasicVersionFeatureId = 6;
        public int MassageFeatureId { get; set; }
        public string FeatureName { get; set; }
    }
}
