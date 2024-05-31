using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace JToolkit.Capabilities;

public static class StartupMvc
{
    public static IMvcBuilder ConfigureMvc(this IServiceCollection services)
    {
        return services
            .AddControllers()
            .ConfigureJson();
    }

    private static IMvcBuilder ConfigureJson(this IMvcBuilder builder)
    {
        builder.AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Formatting = Formatting.Indented;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;

            options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                DateParseHandling = DateParseHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        });

        return builder;
    }
}