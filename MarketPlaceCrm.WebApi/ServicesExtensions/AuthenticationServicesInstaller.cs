using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MarketPlaceCrm.WebApi.ServicesExtensions
{
    public class AuthenticationServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    var jwtConfig = configuration.GetSection("JwtConfig");
                    var appSecret = jwtConfig["Secret"];
                    var secretBytes = Encoding.UTF8.GetBytes(appSecret);
                 
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        
                        IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                        ValidIssuers = new [] { "http://localhost:5000", "https:localhost:5001" },
                        ValidAudiences = new [] {"http://localhost:3000", "http://localhost:5000", "https:localhost:5001" },
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived =  ctx =>
                        {
                            if (ctx.Request.Query.ContainsKey("t"))
                            {
                                ctx.Token = ctx.Request.Query["t"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy(Constants.AdminPolicy, policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Role, Constants.AdminRole)
                        .Build();
                });
                o.AddPolicy(Constants.ModeratorPolicy, policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Role, Constants.ModeratorClaim)
                        .Build();
                });
            });
        }
    }
}