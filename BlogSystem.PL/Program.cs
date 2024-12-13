using BlogSystem.BLL;
using BlogSystem.BLL.GlobalExceptions;
using BlogSystem.BLL.GlobalExceptions.ModelStateErros;
using BlogSystem.DAL;
using BlogSystem.DAL._Identity;
using BlogSystem.DAL.Entities;
using BlogSystem.PL.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new CustomExcpetionFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<BlogIdentityDbContext>();

builder.Services.DALServices(builder.Configuration);
    builder.Services.BLLServices(builder.Configuration);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {

        var errors = context.ModelState.Where(M => M.Value!.Errors.Count > 0).
                                        ToDictionary(k => k.Key, v => v.Value!.Errors.Select(err => err.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new ModelStateErrorResponse()
        {
            Errors = errors
        });
    };
});

var app = builder.Build();

#region DbIntialize

await app.DataIntialize();

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
