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
    [Authorize("Editor")]
    public class PatientsViewModel: MulticastVM
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
                    Weight = model.Weight?.Select(HistoryBaseModel<decimal>.FromModel).ToList()
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

        public string Items_itemKey => nameof(Patient.Id);
        public IList<Patient> Items = new List<Patient>();

        private readonly IPatientService _patientService;

        public PatientsViewModel(IPatientService patientService)
        {
            this._patientService = patientService;
            //init props
            Observable.FromAsync(async () => (await _patientService.GetItems()).Select(Patient.FromModel).ToList())
                      .Subscribe(x =>
                      {
                          Items = x;
                          PushUpdates();
                      });
        }

        public Action<Patient> Add => async (Patient user) =>
        {
            var userModel = user.ToModel();
            userModel = await _patientService.AddItem(userModel);

            var intUser = Patient.FromModel(userModel);

            Items.Add(intUser);
            this.AddList(nameof(Items), intUser);
        };
        public Action<Patient> Update => async (Patient user) =>
        {
            if (await _patientService.UpdateItem(user.ToModel()) != null)
            {
                Items.Remove(Items.First(i => i.Id == user.Id));
                Items.Add(user);

                this.UpdateList(nameof(Items), user);
            }
        };
        public Action<int> Remove => async (int Id) =>
        {
            if (await _patientService.DeleteItem(Id))
            {
                Items.Remove(Items.First(i => i.Id == Id));
                this.RemoveList(nameof(Items), Id);
            }
        };
    }
}
