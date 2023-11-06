using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Models.MailModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IAppointmentEmailService
    {
        bool SendCancellationEmailToClient(Appointment appointment);
        bool SendCancellationEmailToAdmin(Appointment appointment);

        bool SendAcceptPendingAppointmentEmail(Appointment appointment, int companyId);
        bool SendDeclinePendingAppointmentEmail(Appointment appointment, int companyId);

        bool SendConfirmationMailToTeamMember(Appointment appointment);

        bool SendUpdateConfirmationMailToTeamMember(Appointment appointment, string oldDateTime);

        bool SendAppointmentCancelledEmailToTeamMember(Appointment appointment, string oldDateTime);

        string GetEmailHtml(string name);

    }
}
