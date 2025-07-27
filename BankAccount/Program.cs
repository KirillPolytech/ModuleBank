using BankAccount.Features.Accounts.Create;
using BankAccount.Features.ExceptionValidation;
using BankAccount.Features.Middleware;
using BankAccount.Services;
using BankAccount.Services.Interfaces;
using FluentValidation;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddOpenApi();

// Remove in production, use a real database instead
var accountRepository = new AccountRepository();
builder.Services.AddSingleton<IAccountRepository>(accountRepository);

var accountService = new InMemoryAccountService(accountRepository);
builder.Services.AddSingleton<IAccountService>(accountService);

var clientVerificationStub = new ClientVerification(accountRepository);
builder.Services.AddSingleton<IClientVerificationService>(clientVerificationStub);

var currencyService = new CurrencyService();
builder.Services.AddSingleton<ICurrencyService>(currencyService);
//

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();