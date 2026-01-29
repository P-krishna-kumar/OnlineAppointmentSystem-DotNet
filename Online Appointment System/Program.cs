using Online_Appointment_System.DAL;
using OnlineAppointmentSystem.Helpers;
using static Online_Appointment_System.DAL.TimeSlatDAL;

var builder = WebApplication.CreateBuilder(args);

// ✅ Session services
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EmailHelper>();


builder.Services.AddTransient<AdminDAL>();
builder.Services.AddTransient<UserDAL>();
builder.Services.AddTransient<AppointmentDAL>();
builder.Services.AddTransient<TimeSlotDAL>();
builder.Services.AddTransient<ServiceDAL>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔥 THIS WAS MISSING
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
