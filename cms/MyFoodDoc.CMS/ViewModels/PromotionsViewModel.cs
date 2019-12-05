using DotNetify;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    public class PromotionsViewModel : BaseEditableListViewModel<Promotion, int>
    {
        private IInsuranceService _insuranceService;
        private IPromotionService _service;

        public PromotionsViewModel(IPromotionService promotionService, IInsuranceService insuranceService, IConnectionContext connectionContext): base(connectionContext)
        {
            this._service = promotionService;
            this._insuranceService = insuranceService;
        }


        protected override Func<Task<IList<Promotion>>> GetData => async () =>
        {
            return (await _service.GetItems()).Select(Promotion.FromModel).ToList();
        };

        public override Action<Promotion> Add => async (Promotion item) =>
        {
            try
            {
                var itemMod = Promotion.FromModel(await _service.AddItem(item.ToModel()));

                this.AddList(itemMod);
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        };
        public override Action<Promotion> Update => async (Promotion item) =>
        {
            try
            {
                var itemMod = Promotion.FromModel(await _service.UpdateItem(item.ToModel()));
                if (itemMod != null)
                {
                    this.UpdateList(itemMod);
                }
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        };
        public override Action<int> Remove => async (int Id) =>
        {
            try
            {
                if (await _service.DeleteItem(Id))
                {
                    this.RemoveList(Id);
                }
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        };
    }
}
