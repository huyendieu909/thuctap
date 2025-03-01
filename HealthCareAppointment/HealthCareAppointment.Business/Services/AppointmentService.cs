using HealthCareAppointment.Data.UnitOfWork;
using HealthCareAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Business.Services
{
    public class AppointmentService : BaseService<Appointment>
    {
        public AppointmentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
