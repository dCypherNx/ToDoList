using Microsoft.EntityFrameworkCore;
using Worker.Domain.Interfaces.Data;
using Worker.Domain.Interfaces.Services;
using Worker.Data.Contexts;
using Worker.Data.Repositories;
using Worker.Services;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    // Registra o WorkerService como um serviço hospedado
                    services.AddHostedService<WorkerService>();

                    // Registra as dependências necessárias para o RabbitMQService
                    services.AddScoped<IRabbitMQService, RabbitMQService>();
                    services.AddScoped<IToDoRepository, ToDoRepository>();
                });
    }
}
