using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PublicUtilities.Data;
using PublicUtilities.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PublicUtilities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PublicUtilitiesDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            WebApplication app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapPost("api/login", ([FromBody] User user) =>
            {
                string login = builder.Configuration.GetValue<string>("AdminLogin");
                string password = builder.Configuration.GetValue<string>("AdminPassword");
                if (user.Login != login || user.Password != password)
                    return Results.Unauthorized();

                List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), };
                JwtSecurityToken jwt = new JwtSecurityToken(
                        issuer: AuthOptions.Issuer,
                        audience: AuthOptions.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1000)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return Results.Ok(encodedJwt);
            });

            app.MapGet("api/approvedApplications", async (PublicUtilitiesDbContext context) =>
            {
                var query = from application in context.Applications
                            let workers = (from appWorker in context.ApplicationWorkers
                                           join worker in context.Workers
                                           on appWorker.WorkerId equals worker.Id
                                           where appWorker.ApplicationId == application.Id
                                           select worker.FirstName + " " + worker.LastName).ToList()
                            where application.Approved
                            select new
                            {
                                application.Id,
                                application.DateOfWork,
                                application.Address,
                                application.ApplicantName,
                                application.ScaleOfWork,
                                application.TypeOfWork,
                                Workers = workers,
                            };

                var result = await query.ToListAsync();

                return Results.Ok(result.Select(application => new
                {
                    application.Id,
                    application.DateOfWork,
                    application.Address,
                    application.ApplicantName,
                    application.ScaleOfWork,
                    application.TypeOfWork,
                    Workers = string.Join(", ", application.Workers)
                }));
            });

            app.MapPost("api/createApplication", async ([FromBody] Application application, PublicUtilitiesDbContext context) =>
            {
                if (application.DateOfWork == null
                 || application.DateOfWork.Value < DateTime.Now
                 || string.IsNullOrWhiteSpace(application.Name)
                 || string.IsNullOrWhiteSpace(application.Address))
                    return Results.BadRequest();

                context.Applications.Add(new Data.Entities.Application
                {
                    Address = application.Address,
                    ApplicantName = application.Name,
                    Approved = false,
                    CreatedDate = DateTime.Now,
                    DateOfWork = application.DateOfWork.Value,
                    ScaleOfWork = application.ScaleOfWork,
                    TypeOfWork = application.TypeOfWork,
                });

                await context.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapPost("api/approveApplication", async ([FromBody] Approve approve, PublicUtilitiesDbContext context) =>
            {
                if (approve.ApplicationId == default || !approve.WorkerIds.Any())
                    return Results.BadRequest();

                Data.Entities.Application? application = context.Applications
                    .FirstOrDefault(currentApplication => currentApplication.Id == approve.ApplicationId);

                if (application == null)
                    return Results.BadRequest();

                application.Approved = true;
                application.ProcessedDate = DateTime.Now;

                foreach (int workerId in approve.WorkerIds)
                {
                    context.ApplicationWorkers.Add(new Data.Entities.ApplicationWorker
                    {
                        WorkerId = workerId,
                        ApplicationId = application.Id,
                    });
                }

                await context.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapGet("api/pendingApplications", [Authorize] async (PublicUtilitiesDbContext context) =>
            {
                return Results.Ok(await context.Applications.Where(application => !application.Approved).ToListAsync());
            });

            app.MapGet("api/workers", [Authorize] async (PublicUtilitiesDbContext context) =>
            {
                return Results.Ok(await context.Workers.ToListAsync());
            });

            app.Run();
        }
    }
}