using AvivaLibrary.Models;

namespace AvivaUI
{
    public class CConfig(IConfiguration config)
    {
       public string ApiAddress { get; set; } = config["Aviva:ApiAddress"] ?? throw new KeyNotFoundException("Aviva:ProveedoresPago:PagaFacil is missing in configuration");

 
    }
}
