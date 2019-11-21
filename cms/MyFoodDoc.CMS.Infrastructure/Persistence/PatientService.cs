using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class PatientService : IPatientService
    {
        public async Task<PatientModel> AddItem(PatientModel item, CancellationToken cancellationToken = default)
        {
            item.Id = PatientsMock.Default.Count == 0 ? 0 : (PatientsMock.Default.Max(u => u.Id) + 1);
            PatientsMock.Default.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var user = PatientsMock.Default.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return await Task.FromResult(false);

            PatientsMock.Default.Remove(user);
            return await Task.FromResult(true);
        }

        public async Task<PatientModel> GetItem(int id)
        {
            return await Task.FromResult(PatientsMock.Default.FirstOrDefault(u => u.Id == id));
        }

        public async Task<IList<PatientModel>> GetItems()
        {
            return await Task.FromResult(PatientsMock.Default);
        }

        public async Task<PatientModel> UpdateItem(PatientModel item, CancellationToken cancellationToken = default)
        {
            var user = PatientsMock.Default.FirstOrDefault(u => u.Id == item.Id);

            if (user == null)
                return null;

            PatientsMock.Default.Remove(user);
            PatientsMock.Default.Add(item);
            return await Task.FromResult(item);
        }
    }
}
