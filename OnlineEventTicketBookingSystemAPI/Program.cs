using OnlineEventTicketBookingSystemAPI.Extenstions;

namespace OnlineEventTicketBookingSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Environment.SetEnvironmentVariable("SECRET", "%esosesos1421349125#@!es156465gsosfasf%");

            // Add services to the container.

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(
                options => options.SuppressModelStateInvalidFilter = true);

            builder.Services.ConfigureAddDbContext(builder.Configuration);
            builder.Services.ConfigureIdentityDbContext();
            builder.Services.ConfigureCorsPolicy();
            builder.Services.ConfigureRepositoryPattern();

            builder.Services.AddJwtConfiguration(builder.Configuration);
            builder.Services.ConfigureJWTAuthentication(builder.Configuration);

            builder.Services.AddAutoMapper(typeof(Program)); // This line requires the AutoMapper.Extensions.Microsoft.DependencyInjection package

           

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // app.ConfigureExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("corsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
