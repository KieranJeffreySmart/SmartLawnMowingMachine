namespace Slmm.Api.Infrastructure
{
    public interface ISettingsResolver
    {
        string ReadValue(string sectionName, string key, string defaultValue);
        T ReadValue<T>(string sectionName, string key, T defaultValue) where T : struct;
    }
}
