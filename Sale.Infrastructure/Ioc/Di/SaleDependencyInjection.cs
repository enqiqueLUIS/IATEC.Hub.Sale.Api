using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sale.Application.Services.Sale;
using Sale.Application.Validators.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Common;
using Sale.Domain.Repositories.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Context.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Repositories.Common;
using Sale.Infrastructure.DataBase.EntityFramework.Repositories.Sale;

namespace Sale.Infrastructure.Ioc.Di;

public static class SaleDependencyInjection
{
    public static IServiceCollection RegisterDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<SaleDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddTransient<DishService>();
        collection.AddTransient<PaymentMethodService>();
        collection.AddTransient<SaleDetailService>();
        collection.AddTransient<SaleService>();
        
        // Registro del repositorio genérico
        collection.AddScoped<IGenericRepository<DishEntity>, GenericRepository<DishEntity>>();
        collection.AddScoped<IGenericRepository<PaymentMethodEntity>, GenericRepository<PaymentMethodEntity>>();
        collection.AddScoped<IGenericRepository<SaleDetailEntity>, GenericRepository<SaleDetailEntity>>();
        collection.AddScoped<IGenericRepository<SaleEntity>, GenericRepository<SaleEntity>>();
        
        // Registro de validadores de FluentValidation
        collection.AddTransient<IValidator<DishModel>, DishValidation>();
        collection.AddTransient<IValidator<PaymentMethodModel>, PaymentMethodValidation>();
        collection.AddTransient<IValidator<SaleDetailModel>, SaleDetailValidation>();
        collection.AddTransient<IValidator<SaleModel>, SaleValidation>();
        return collection;
    }
    
    public static IServiceCollection RegisterRepositories(this IServiceCollection collection)
    {
        collection.AddTransient<IDishRepository, DishRepository>();
        collection.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
        collection.AddTransient<ISaleDetailRepository, SaleDetailRepository>();
        collection.AddTransient<ISaleRepository, SaleRepository>();
        return collection;
    }
    
    public static IServiceCollection RegisterProviders(this IServiceCollection collection)
    {
        return collection;
    }
}