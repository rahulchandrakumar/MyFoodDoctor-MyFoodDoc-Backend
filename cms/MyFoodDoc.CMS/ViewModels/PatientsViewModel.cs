using DotNetify;
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

        public Action<Patient> Add => async (Patient patient) =>
        {
            try
            {
                this.AddList(nameof(Items), Patient.FromModel(await _service.AddItem(patient.ToModel())));
                this.PushUpdates();
            }
            catch (Exception ex)
            {

            }
        };
        public Action<Patient> Update => async (Patient user) =>
        {
            try
            {
                if (await _service.UpdateItem(user.ToModel()) != null)
                {
                    this.UpdateList(nameof(Items), user);
                    this.PushUpdates();
                }
            }
            catch (Exception ex)
            {

            }
        };
        public Action<int> Remove => async (int Id) =>
        {
            try
            {
                if (await _service.DeleteItem(Id))
                {
                    this.RemoveList(nameof(Items), Id);
                    this.PushUpdates();
                }
            }
            catch (Exception ex)
            {

            }
        };
    }
}
