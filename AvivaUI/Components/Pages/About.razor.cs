using AvivaUI.Services;
using System.Reflection;

namespace AvivaUI.Components.Pages
{
    /// <summary>
    /// About services
    /// </summary>
    /// <param name="services"></param>
    public partial class About(IProductServices services)
    {
        private string version = "1.0.0";
        private string connectivity = string.Empty;
        private readonly IProductServices pservices = services;

        protected override void OnInitialized()
        {
            var ver = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();
            if (!string.IsNullOrEmpty(ver))
            {
                version = ver;
            }
        }

        protected async Task CheckOdooConectivityAsync()
        {
            try
            {
                connectivity = "processing... wait";
                connectivity = await pservices.CheckConnectionAliveAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                connectivity = $"A exception happen: {ex.Message}";
            }
        }
    }

}

