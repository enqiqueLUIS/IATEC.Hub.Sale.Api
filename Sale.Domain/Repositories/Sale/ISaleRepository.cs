using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Common;

namespace Sale.Domain.Repositories.Sale;

public interface ISaleRepository: IGenericRepository<SaleModel>
{
    Task<List<SaleDto>> GetAll();
}