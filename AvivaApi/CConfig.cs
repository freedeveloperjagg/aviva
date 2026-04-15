using AvivaLibrary.Models;

namespace AvivaApi
{
    public class CConfig(IConfiguration config)
    {
        public List<ProveedorSetting> ProveedoresSettings { get; set; } = config.GetSection("ProveedoresPago").Get<List<ProveedorSetting>>() ?? throw new KeyNotFoundException("Aviva:ProveedoresPago is missing in configuration");

      
    }
}
