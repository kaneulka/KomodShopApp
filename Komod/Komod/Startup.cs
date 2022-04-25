using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Repo;
using Komod.Repo.ArticleRepo;
using Komod.Repo.BrandRepo;
using Komod.Repo.CartRepo;
using Komod.Repo.CategoryRepo;
using Komod.Repo.ColorRepo;
using Komod.Repo.CountryRepo;
using Komod.Repo.DeliveryMethodRepo;
using Komod.Repo.EventProductRepo;
using Komod.Repo.EventPromotionRepo;
using Komod.Repo.ImageRepo;
using Komod.Repo.OrderRepo;
using Komod.Repo.OrderStatusRepo;
using Komod.Repo.PaymentMethodRepo;
using Komod.Repo.ProductRepo;
using Komod.Repo.ProductSetRepo;
using Komod.Repo.PromocodeArticleRepo;
using Komod.Repo.PromocodeRepo;
using Komod.Repo.PropertyRepo;
using Komod.Repo.PropertyValCatArtRepo;
using Komod.Repo.PropertyValueRepo;
using Komod.Repo.StockStatusRepo;
using Komod.Repo.UserRepo;
using Komod.Repo.WishlistRepo;
using Komod.Ser.ArticleSer;
using Komod.Ser.BrandSer;
using Komod.Ser.CartSer;
using Komod.Ser.CategorySer;
using Komod.Ser.ColorSer;
using Komod.Ser.CountrySer;
using Komod.Ser.DeliveryMethodSer;
using Komod.Ser.EventProductSer;
using Komod.Ser.EventPromotionSer;
using Komod.Ser.ImageSer;
using Komod.Ser.OrderSer;
using Komod.Ser.OrderStatusSer;
using Komod.Ser.PaymentMethodSer;
using Komod.Ser.ProductSer;
using Komod.Ser.ProductSetSer;
using Komod.Ser.PromocodeArticleSer;
using Komod.Ser.PromocodeSer;
using Komod.Ser.PropertySer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.StockStatusSer;
using Komod.Ser.UserSer;
using Komod.Ser.WishlistSer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Komod
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDataProtection().PersistKeysToDbContext<ApplicationContext>();

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 6;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = true;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = true; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = true; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = true; // требуются ли цифры
            }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddSession();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IArticleRepository), typeof(ArticleRepository));
            services.AddScoped(typeof(IBrandRepository), typeof(BrandRepository));
            services.AddScoped(typeof(ICartRepository), typeof(CartRepository));
            services.AddScoped(typeof(ICartItemRepository), typeof(CartItemRepository));
            services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            services.AddScoped(typeof(IColorRepository), typeof(ColorRepository));
            services.AddScoped(typeof(ICountryRepository), typeof(CountryRepository));
            services.AddScoped(typeof(IDeliveryMethodRepository), typeof(DeliveryMethodRepository));
            services.AddScoped(typeof(IEventPromotionRepository), typeof(EventPromotionRepository));
            services.AddScoped(typeof(IEventProductRepository), typeof(EventProductRepository));
            services.AddScoped(typeof(IImageRepository), typeof(ImageRepository));
            services.AddScoped(typeof(IOrderItemRepository), typeof(OrderItemRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddScoped(typeof(IOrderStatusRepository), typeof(OrderStatusRepository));
            services.AddScoped(typeof(IPaymentMethodRepository), typeof(PaymentMethodRepository));
            services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            services.AddScoped(typeof(IPropertyRepository), typeof(PropertyRepository));
            services.AddScoped(typeof(IPropertyValCatArtRepository), typeof(PropertyValCatArtRepository));
            services.AddScoped(typeof(IPromocodeRepository), typeof(PromocodeRepository));
            services.AddScoped(typeof(IPromocodeArticleRepository), typeof(PromocodeArticleRepository));
            services.AddScoped(typeof(IPropertyValueRepository), typeof(PropertyValueRepository));
            services.AddScoped(typeof(IStockStatusRepository), typeof(StockStatusRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IWishlistItemRepository), typeof(WishlistItemRepository));
            services.AddScoped(typeof(IWishlistRepository), typeof(WishlistRepository));
            services.AddScoped(typeof(IProductSetRepository), typeof(ProductSetRepository));


            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ICartItemService, CartItemService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IDeliveryMethodService, DeliveryMethodService>();
            services.AddTransient<IEventProductService, EventProductService>();
            services.AddTransient<IEventPromotionService, EventPromotionService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderStatusService, OrderStatusService>();
            services.AddTransient<IPaymentMethodService, PaymentMethodService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IPromocodeService, PromocodeService>();
            services.AddTransient<IPromocodeArticleService, PromocodeArticleService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IPropertyValCatArtService, PropertyValCatArtService>();
            services.AddTransient<IPropertyValueService, PropertyValueService>();
            services.AddTransient<IStockStatusService, StockStatusService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWishlistItemService, WishlistItemService>();
            services.AddTransient<IWishlistService, WishlistService>();
            services.AddTransient<IProductSetService, ProductSetService>();

            services.AddControllersWithViews();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Identity.Application";
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();

            app.UseBrowserLink();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
