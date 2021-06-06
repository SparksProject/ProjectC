using System.Text;

using AutoMapper;

using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service;
using Chep.Service.Interface;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Chep.WebApi
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
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            services.AddAuthorization();

            services.Configure<SparksConfig>(Configuration.GetSection("SparksXConfig"));
            var configModel = Configuration.GetSection("SparksXConfig").Get<SparksConfig>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configModel.TokenManagement.Secret)),
                    ValidIssuer = configModel.TokenManagement.Issuer,
                    ValidateIssuer = false,
                    ValidAudience = configModel.TokenManagement.Audience,
                    ValidateAudience = false
                };
            });

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IMailReportService, MailReportService>();
            services.AddScoped<IGenericReportService, GenericReportService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDefinitionService, DefinitionService>();
            services.AddScoped<IStokGirisService, StokGirisService>();
            services.AddScoped<IStokCikisService, StokCikisService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "files")),
            //    RequestPath = "/files"
            //});
        }
    }
}