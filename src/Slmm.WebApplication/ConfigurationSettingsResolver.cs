namespace Slmm.WebApplication
{
    using Microsoft.Extensions.Configuration;
    using Slmm.Api;

    public class ConfigurationSettingsResolver : ISettingsResolver
    {
        IConfiguration configuration;

        public ConfigurationSettingsResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public T ReadValue<T>(string sectionName, string key, T defaultValue) where T : struct
        {
            var section = this.configuration.GetSection(sectionName);
            if (section == null)
            {
                return defaultValue;
            }

            return section.GetValue(key, defaultValue);
        }

        public string ReadValue(string sectionName, string key, string defaultValue)
        {
            var section = this.configuration.GetSection(sectionName);
            if (section == null)
            {
                return defaultValue;
            }

            return section.GetValue(key, defaultValue);
        }
    }
}
