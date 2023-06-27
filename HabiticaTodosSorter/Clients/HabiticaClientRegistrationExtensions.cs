namespace HabiticaTodosSorter.Clients;

public static class HabiticaClientRegistrationExtensions
{
    public static void AddHabiticaClient(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var habiticaClientConfig = new HabiticaClientConfiguration();
        var habiticaClientConfigurationSection = configuration.GetSection(nameof(HabiticaClient));
        habiticaClientConfigurationSection.Bind(habiticaClientConfig);
        services.AddSingleton(habiticaClientConfig);

        services.AddHttpClient(nameof(HabiticaClient), client => client.BaseAddress = new Uri(habiticaClientConfig.Url));
        services.AddSingleton<IHabiticaClient, HabiticaClient>();
    }
}