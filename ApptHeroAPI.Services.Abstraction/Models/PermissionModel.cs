using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class PermissionModel
    {
        public const string FullAccess = "Full Access";
        public const string AccessAllSchedules = "Access to all schedules";
        public const string AccessAllSchedulesNoChanges = "Access to all schedules, cannot make changes";
        public const string AccessOwnSchedule = "Access to own schedule";
        public const string AccessOwnScheduleNoChanges = "Access to own schedule, cannot make changes";
        public const string NoAccess = "No access (cannot log in)";


        public const string DataTextField = "PermissionName";
        public const string DataValueField = "PermissionId";
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
}
