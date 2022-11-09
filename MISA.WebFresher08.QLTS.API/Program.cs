using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.BL.DepartmentBL;
using MISA.WebFresher08.QLTS.DL;
using MISA.WebFresher08.QLTS.DL.DepartmentDL;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IAssetBL, AssetBL>();
builder.Services.AddScoped<IAssetDL, AssetDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();
builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IVoucherBL, VoucherBL>();
builder.Services.AddScoped<IVoucherDL, VoucherDL>();

DataContext.MySqlConnectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
