using BankAccount.Features.Accounts.Create;
using BankAccount.Features.ExceptionValidation;
using BankAccount.Features.Middleware;
using BankAccount.Services;
using BankAccount.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using ServiceCollectionExtensions = BankAccount.Services.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(ServiceCollectionExtensions.AddCors);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<MbExceptionFilter>();
});

builder.Services.AddSwaggerGen(o => ServiceCollectionExtensions.AddSwaggerGen(o, builder));

// JWT Bearer Auth via Keycloak.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { ServiceCollectionExtensions.AddAuthentication(options, builder);  });

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

var accountRepository = new AccountRepository();
builder.Services.AddSingleton<IAccountRepository>(accountRepository);
builder.Services.AddSingleton<IAccountService>(new InMemoryAccountService(accountRepository));
builder.Services.AddSingleton<IClientVerificationService>(new ClientVerification(accountRepository));
builder.Services.AddSingleton<ICurrencyService>(new CurrencyService());

builder.Services.Configure<ApiBehaviorOptions>(ServiceCollectionExtensions.ConfigureApiBehaviorOptions);

var app = builder.Build();

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