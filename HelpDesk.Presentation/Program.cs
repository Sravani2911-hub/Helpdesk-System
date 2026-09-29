using HelpDesk.DAL.Context;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using HelpDesk.BAL.Interfaces;
using HelpDesk.BAL.Services;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Repositories;

using HelpDesk.Presentation.Models;
using HelpDesk.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new EmailSettings
{
    From = builder.Configuration["EmailSettings:From"]!,
    SmtpServer = builder.Configuration["EmailSettings:SmtpServer"]!,
    Port = int.Parse(builder.Configuration["EmailSettings:Port"]!),
    Username = builder.Configuration["EmailSettings:Username"]!,
    Password = builder.Configuration["EmailSettings:Password"]!
});


// Add services to the container.
builder.Services.AddControllersWithViews();

//Department
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); //When IDepartmentRepository is requested,its create DepartmentRepository(The same instance is reused during that HTTP request.)
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

//Category
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

//Priority
builder.Services.AddScoped<IPriorityRepository, PriorityRepository>();
builder.Services.AddScoped<IPriorityService, PriorityService>();

// Ticket
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddScoped<ITicketCommentRepository, TicketCommentRepository>();
builder.Services.AddScoped<ITicketCommentService, TicketCommentService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ITicketHistoryRepository, TicketHistoryRepository>();
builder.Services.AddScoped<ITicketHistoryService, TicketHistoryService>();

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();


builder.Services.AddDbContext<HelpDeskDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HelpDeskConnection"))); // register the DbContext

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HelpDeskDbContext>()
    .AddDefaultTokenProviders(); //register Identity

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())  //we create roles in program.cs so they are created automatically when the application starts.
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Employee", "Support Engineer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
