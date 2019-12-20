using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IPatientService: IServiceBasePaginatedRead<PatientModel, string>
    {
        Task<IList<HistoryModel<int>>> FullUserHistory(CancellationToken cancellationToken = default);
    }
}
