using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotVVM.DynamicData.Helpers.Services
{
    public interface IDetailPageService<TDetailModel, TKey>
    {

        Task<TDetailModel> LoadItem(TKey id);

        Task<TDetailModel> InitializeItem();

        Task SaveItem(TDetailModel item, TKey id);

        Task DeleteItem(TKey id);

    }
}
