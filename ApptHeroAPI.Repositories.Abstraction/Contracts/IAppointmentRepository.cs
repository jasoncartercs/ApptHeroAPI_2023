using ApptHeroAPI.Repositories.Context.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Abstraction.Contracts
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAppointmentsByPersonIds(List<long> personIds);
    }
}
