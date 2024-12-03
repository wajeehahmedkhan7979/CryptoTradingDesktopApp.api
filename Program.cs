using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Google.Cloud.Firestore;
using CryptoTradingDesktopApp.Api.Services;
using CryptoTradingDesktopApp.Api.Data;
using CryptoTradingDesktopApp.Api;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Initialize Firebase
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("Keys/serviceAccountKey.json")
        });

        // Add Firestore as a service
        builder.Services.AddSingleton(provider =>
        {
            return FirestoreDb.Create("cryptotradingdesktopapp-a06be"); // Replace with your Firebase project ID
        });

        // Add services to the container
        builder.Services.AddControllers();

        // JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = builder.Configuration["Jwt:Key"]
                    ?? throw new ArgumentNullException("Jwt:Key");
                var keyBytes = Encoding.UTF8.GetBytes(key);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

        // Add DbContext
        builder.Services.AddDbContext<CryptoDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MySqlConnection")));

        // Register services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IWalletService, WalletService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IOrderBookService, OrderBookService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITradeService, TradeService>();
        builder.Services.AddScoped<UnitOfWork>();
        builder.Services.AddScoped<FirestoreService>();

        var app = builder.Build();

        // Firestore initialization
        var firestoreDb = app.Services.GetRequiredService<FirestoreDb>();
        await FirestoreInitializer.InitializeCollections(firestoreDb);

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
