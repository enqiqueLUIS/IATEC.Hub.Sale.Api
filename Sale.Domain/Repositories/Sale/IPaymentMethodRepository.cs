using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Common;

namespace Sale.Domain.Repositories.Sale;

public interface IPaymentMethodRepository: IGenericRepository<PaymentMethodModel>
{
    Task<List<PaymentMethodDto>> GetAll();
    
}