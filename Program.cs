
using GPStracker.Model;
using GPStracker.Repository;

namespace GPStracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IRepository, PackageRepository>();

            builder.Services.Configure<MongoDBRestSettings>(builder.Configuration.GetSection(nameof(MongoDBRestSettings)));



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.MapPost("/package", (IRepository sr, Package package) =>
            {
                sr.Add(package);

            });

            app.MapDelete("/package/{id}", (Guid id, IRepository sr) =>
            {
                sr.Delete(id);
            });


            app.MapGet("/package/{id}", (Guid id, IRepository sr) =>
            {
                return sr.Get(id);
            });

            app.MapGet("/packages", (IRepository sr) =>
            {
                return sr.GetAll();
            });


            app.Run();
        }
    }
}