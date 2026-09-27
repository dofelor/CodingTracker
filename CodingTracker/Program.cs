using CodingTracker.Controllers;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        string connectionString = config.GetConnectionString("DefaultConnection");

        DbContext context = new(connectionString);
        context.InitialTable();

        CodingSessionsController controller = new(context);

        UserInterface main = new UserInterface(controller);
        main.MainMenu();
    }
}
