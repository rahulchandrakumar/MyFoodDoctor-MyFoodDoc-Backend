using DotNetify.Security;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models.VM;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Editor")]
    public class PatientsViewModel : BaseListViewModel<Patient>
    {
        private readonly IPatientService _service;

        public PatientsViewModel(IPatientService patientService)
        {
            this._service = patientService;

            //init props
            Init();
        }
        private Action Init => () =>
        {
            try
            {
                this.Items =_service.GetItems().Result.Select(Patient.FromModel).ToList();
            }
            catch (Exception ex)
            {

            }
        };
    }
}
