
namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            //
            Dictionary<int, List<BasketItem>> baskets = new();

            app.MapGet("/api/reservations/basket/{userId}", (int userId) =>
            {
                if (!baskets.ContainsKey(userId)) return Results.Ok(new List<BasketItem>());
                return Results.Ok(baskets[userId]);
            });

            app.MapPost("/api/reservations/add/{userId}", (int userId, BasketItem package) =>
            {
                if (string.IsNullOrWhiteSpace(package.Name) || package.Price <= 0)
                {
                    return Results.BadRequest("Invalid travel package.");
                }

                if (!baskets.ContainsKey(userId))
                {
                    baskets[userId] = new List<BasketItem>();
                }

                baskets[userId].Add(package);
                return Results.Ok(new { message = "Package added!", basket = baskets[userId] });
            });

            app.MapDelete("/api/reservations/remove/{userId}/{packageId}", (int userId, int packageId) =>
            {
                if (!baskets.ContainsKey(userId) || !baskets[userId].Any(p => p.Id == packageId))
                {
                    return Results.NotFound("Package not found.");
                }

                baskets[userId].RemoveAll(p => p.Id == packageId);
                return Results.Ok(new { message = "Package removed!", basket = baskets[userId] });
            });

            app.MapPost("/api/reservations/checkout/{userId}", (int userId) =>
            {
                if (!baskets.ContainsKey(userId) || !baskets[userId].Any())
                {
                    return Results.BadRequest("Basket is empty!");
                }

                baskets[userId].Clear();
                return Results.Ok("Booking Confirmed!");
            });


            //
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();


          
        }
    }
}
