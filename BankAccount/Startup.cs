using BankAccount.Features.Accounts.Create;
using BankAccount.Features.ExceptionValidation;
using BankAccount.Features.Middleware;
using BankAccount.Persistence.Db;
using BankAccount.Persistence.InMemory;
using BankAccount.Persistence.Interfaces;
using BankAccount.Services;
using BankAccount.Services.Interfaces;
using BankAccount.UnitTests;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceCollectionExtensions = BankAccount.Services.ServiceCollectionExtensions;

namespace BankAccount
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        [Obsolete("Obsolete")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(ServiceCollectionExtensions.AddCors);

            services.AddControllers(opt =>
            {
                opt.Filters.Add<MbExceptionFilter>();
            });

            services.AddSwaggerGen(o => ServiceCollectionExtensions.AddSwaggerGen(o, Configuration));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => { ServiceCollectionExtensions.AddAuthentication(options, Configuration); });

            services.AddMediatR(typeof(Program));
            services.AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            if (Environment.IsDevelopment())
            {
                var accountRepository = new InMemoryAccountsRepository();
                services.AddSingleton<IAccountRepository>(accountRepository);
            }

            var ownersRepository = new InMemoryOwnersRepository();
            services.AddSingleton<IOwnersRepository>(ownersRepository);

            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<IClientVerificationService>(new ClientVerification(ownersRepository));
            services.AddSingleton<ICurrencyService>(new CurrencyService());

            services.Configure<ApiBehaviorOptions>(ServiceCollectionExtensions.ConfigureApiBehaviorOptions);

            services.AddHangfireServer();
            services.AddScoped<IHangfireJobScheduler, HangfireJobScheduler>();

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<TestService>();

        }

        public void Configure(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!Environment.IsDevelopment())
                {
                    db.Database.Migrate();
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.RoutePrefix = "swagger";

                options.OAuthClientId(Configuration["Jwt:Audience"]);
                options.OAuthUsePkce();
                options.OAuth2RedirectUrl(Configuration["Jwt:RedirectUrl"]);
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var scheduler = scope.ServiceProvider.GetRequiredService<IHangfireJobScheduler>();
                try
                {
                    scheduler.ScheduleJobs();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            app.UseStaticFiles();

            app.UseHangfireDashboard();

            app.UseRouting();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
