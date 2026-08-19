using System.Text.Json;

namespace GoldSilverApp.Automation.Core;

public static class ConfigManager
{
    private static JsonElement? _config;

    public static void Load(string environment = "qa")
    {
        var path = Path.Combine(AppContext.BaseDirectory, $"appsettings.{environment}.json");
        if (!File.Exists(path))
            throw new FileNotFoundException($"Config file not found for environment '{environment}': {path}");

        var json = File.ReadAllText(path);
        _config = JsonSerializer.Deserialize<JsonElement>(json);
    }

    public static string BaseUrl => GetRequired("BaseUrl");

    //public static string ApiBaseUrl => GetRequired("ApiBaseUrl");

    // public static string PlaidSandboxKey =>
    //     Environment.GetEnvironmentVariable("PLAID_SANDBOX_KEY")
    //         ?? throw new InvalidOperationException("PLAID_SANDBOX_KEY environment variable not set.");

    // public static string BraintreeSandboxKey =>
    //     Environment.GetEnvironmentVariable("BRAINTREE_SANDBOX_KEY")
    //         ?? throw new InvalidOperationException("BRAINTREE_SANDBOX_KEY environment variable not set.");

    private static string GetRequired(string key)
    {
        if (_config is null)
            throw new InvalidOperationException("ConfigManager.Load() must be called before accessing config values.");

        if (!_config.Value.TryGetProperty(key, out var value))
            throw new KeyNotFoundException($"Config key '{key}' not found in loaded config.");

        return value.GetString()
            ?? throw new InvalidOperationException($"Config key '{key}' was present but null.");
    }
}