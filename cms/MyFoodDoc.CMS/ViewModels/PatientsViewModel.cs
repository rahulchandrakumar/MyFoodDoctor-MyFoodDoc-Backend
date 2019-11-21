using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{
    public class Patient : ColabDataTableBaseModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Insurance { get; set; }
        public byte Sex { get; set; }
        public int Height { get; set; }
        public DateTime Birth { get; set; }
        public IList<HistoryBaseModel<decimal>> Weight { get; set; }
        public IList<HistoryBaseModel<int>> BloodSugar { get; set; }
        public IList<HistoryBaseModel<decimal>> AbdominalGirth { get; set; }
        public IList<string> Motivation { get; set; }

        public static Patient FromModel(PatientModel model)
        {
            return new Patient()
            {
                Id = model.Id,
                FullName = model.FullName,
                Birth = model.Birth,
                Email = model.Email,
                Insurance = model.Insurance,
                Height = model.Height,
                Sex = (byte)model.Sex,
                AbdominalGirth = model.AbdominalGirth?.Select(HistoryBaseModel<decimal>.FromModel).ToList(),
                BloodSugar = model.BloodSugar?.Select(HistoryBaseModel<int>.FromModel).ToList(),
                Weight = model.Weight?.Select(HistoryBaseModel<decimal>.FromModel).ToList(),
                Motivation = model.Motivation?.ToList()
            };
        }

        public PatientModel ToModel()
        {
            return new PatientModel()
            {
                Id = Id,
                FullName = FullName,
                Birth = Birth,
                Email = Email,
                Insurance = Insurance,
                Height = Height,
                Sex = (SexEnum)Sex,
                AbdominalGirth = AbdominalGirth?.Select(x => x.ToModel()).ToList(),
                BloodSugar = BloodSugar?.Select(x => x.ToModel()).ToList(),
                Weight = Weight?.Select(x => x.ToModel()).ToList()
            };
        }
    }

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
