﻿using SWP391_BL3W.Repository.Interface;
using SWP391_BL3W.Repository;
using SWP391_BL3W.Services;
using SWP391_BL3W.Services.Interface;


namespace SWP391_BL3W.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            //Register Repository here
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //Register Service here
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPaymentOnlineService, VnPayService>();
            services.AddScoped<IReviewService, ReviewService>();
            return services;
        }
    }
}
