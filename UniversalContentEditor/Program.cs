using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using UniversalContentEditor.Components;
using UniversalContentEditor.Components.Account;
using UniversalContentEditor.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Configure MongoDB
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb") ?? throw new InvalidOperationException("Connection string 'MongoDb' not found.");
var mongoClient = new MongoClient(mongoConnectionString);
var databaseName = builder.Configuration["MongoDbSettings:DatabaseName"] ?? "UniversalContentEditor";
var collectionName = builder.Configuration["MongoDbSettings:CollectionName"] ?? "users";
var mongoDatabase = mongoClient.GetDatabase(databaseName);

builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
builder.Services.AddSingleton<ApplicationDbContext>();

// Configure Identity with MongoDB
builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.Tokens.ProviderMap.Clear();
    })
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Register MongoUserStore
builder.Services.AddScoped<IUserStore<ApplicationUser>>(provider => 
    new MongoUserStore(provider.GetRequiredService<IMongoDatabase>(), collectionName));

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
