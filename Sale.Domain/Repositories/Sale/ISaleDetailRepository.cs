using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Common;

namespace Sale.Domain.Repositories.Sale;

public interface ISaleDetailRepository: IGenericRepository<SaleDetailModel>
{
    Task<List<SaleDetailDto>> GetAll();
    
}