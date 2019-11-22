using DotNetify.Security;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Editor")]
    public class PatientsViewModel : BaseListViewModel<Patient, string>
    {
        private readonly IPatientService _service;

        public PatientsViewModel(IPatientService patientService)
        {
            this._service = patientService;
        }

        protected override Func<Task<IList<Patient>>> GetData => async () =>
        {
            try
            {
                return (await _service.GetItems()).Select(Patient.FromModel).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        };
    }
}
