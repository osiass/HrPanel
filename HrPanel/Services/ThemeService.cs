using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace HrPanel.Services;

public class ThemeService
{
    private readonly ProtectedLocalStorage _localStorage;
    private const string ThemeKey = "app_theme";

    public string CurrentTheme { get; private set; } = "light";

    public event Action? OnThemeChanged;

    public ThemeService(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }
    //eski temayı yükler
    public async Task InitializeAsync()
    {
        try
        {
            //storage'dan tema bilgisini alır
            var result = await _localStorage.GetAsync<string>(ThemeKey);
            if (result.Success && !string.IsNullOrEmpty(result.Value))
            {
                CurrentTheme = result.Value;
            }
        }
        //storageda tema yoksa okunamazsa defaultu light yapar
        catch
        {
            CurrentTheme = "light";
        }
        
        NotifyThemeChanged();
    }

    public async Task ToggleThemeAsync()
    {
        //eğer current theme lightsa dark değilse light yapar
        CurrentTheme = CurrentTheme == "light" ? "dark" : "light";
        //storage'a tema bilgisini kaydeder
        await _localStorage.SetAsync(ThemeKey, CurrentTheme);
        NotifyThemeChanged();
    }
    //uygulama baslangıcinda storage'dan tema bilgisini alır
    //invoke event tetikler 
    private void NotifyThemeChanged() => OnThemeChanged?.Invoke();

  /*  private void NotifyThemeChanged()
{
    if (OnThemeChanged != null)
    {
        OnThemeChanged.Invoke();
    }
}*/
}
