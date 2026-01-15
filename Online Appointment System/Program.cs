

using Online_Appointment_System.DAL;
using OnlineAppointmentSystem.Helpers;
using static Online_Appointment_System.DAL.TimeSlatDAL;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<EmailHelper>();

builder.Services.AddTransient<AdminDAL>();
builder.Services.AddTransient<UserDAL>();
builder.Services.AddTransient<AppointmentDAL>();
builder.Services.AddTransient<TimeSlotDAL>();

var app = builder.Build();
 
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

app.UseAuthorization();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
