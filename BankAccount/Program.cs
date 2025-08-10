using BankAccount.Features.Accounts.Create;
using BankAccount.Features.ExceptionValidation;
using BankAccount.Features.Middleware;
using BankAccount.Persistence.Db;
using BankAccount.Persistence.InMemory;
using BankAccount.Persistence.Interfaces;
using BankAccount.Services;
using BankAccount.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceCollectionExtensions = BankAccount.Services.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsHistoryTable(
                "__EFMigrationsHistory",
                "public"
            );
        })
);

builder.Services.AddCors(ServiceCollectionExtensions.AddCors);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<MbExceptionFilter>();
});

builder.Services.AddSwaggerGen(o => ServiceCollectionExtensions.AddSwaggerGen(o, builder));

// JWT Bearer Auth via Keycloak.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { ServiceCollectionExtensions.AddAuthentication(options, builder); });

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

var accountRepository = new InMemoryAccountsRepository();
var ownersRepository = new InMemoryOwnersRepository();
builder.Services.AddSingleton<IAccountRepository>(accountRepository);
builder.Services.AddSingleton<IOwnersRepository>(ownersRepository);

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSingleton<IClientVerificationService>(new ClientVerification(ownersRepository));
builder.Services.AddSingleton<ICurrencyService>(new CurrencyService());

builder.Services.Configure<ApiBehaviorOptions>(ServiceCollectionExtensions.ConfigureApiBehaviorOptions);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = "swagger";

    // Keycloak OAuth2 config.
    options.OAuthClientId(builder.Configuration["Jwt:Audience"]);
    options.OAuthUsePkce();
    options.OAuth2RedirectUrl(builder.Configuration["Jwt:RedirectUrl"]);
});

app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();