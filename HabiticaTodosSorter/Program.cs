using HabiticaTodosSorter.Clients;
using HabiticaTodosSorter.Mappings;
using HabiticaTodosSorter.RequestHandlers.Tags;
using HabiticaTodosSorter.RequestHandlers.Todos;
using HabiticaTodosSorter.Services;
using Serilog;

namespace HabiticaTodosSorter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Handlers
            builder.Services.AddSingleton<ITodosRequestHandler, TodosRequestHandler>();
            builder.Services.AddSingleton<ITagsRequestHandler, TagsRequestHandler>();
            
            // Services:
            builder.Services.AddSingleton<ITagService, TagService>();
            builder.Services.AddSingleton<ISorterService, SorterService>();
            builder.Services.AddSingleton<ITodoService, TodoService>();
            
            // Clients:
            builder.Services.AddHabiticaClient(builder.Configuration);
            
            // Infrastructure
            builder.Services.AddAutoMapper(typeof(TagProfile));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // Serilog & Seq
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
            builder.Host.UseSerilog();

            // CORS
            builder.Services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(p =>
                {
                    p.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}