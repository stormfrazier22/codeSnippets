public static class AppBuilder
{
    // Hard coded connection string, can be moved to configuration as well.
    public const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=University; Integrated Security=True;";

    public static WebApplication GetApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog((hostContext, services, configuration) =>
        {
            configuration
                .WriteTo.File("serilog-file.txt")
                .WriteTo.Console();
        });

        // Setup database connection
        builder
            .Services
            .AddDbContext<UniversityDbContext>(opt => opt.UseSqlServer(ConnectionString));

        // DI - Register repository 
        builder.Services.AddTransient<IStudentRepository, StudentRepository>();

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        return app;
    }
}