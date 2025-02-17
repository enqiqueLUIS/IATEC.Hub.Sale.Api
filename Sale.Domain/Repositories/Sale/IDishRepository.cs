using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Common;

namespace Sale.Domain.Repositories.Sale;

public interface IDishRepository: IGenericRepository<DishModel>
{
    Task<List<DishDto>> GetAll();
    
}