using System.Security.Claims;
using HrPanel.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage; // Bu namespace şart

namespace HrPanel.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthStateProvider(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        //sistemdeki mevcut kullanıcıyı döndürür
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Memory boşsa depodan (Local Storage) çekmeye çalış
            if (_currentUser.Identity?.IsAuthenticated != true)
            {
                try
                {
                    var result = await _localStorage.GetAsync<UserSession>("UserSession");
                    if (result.Success && result.Value != null)
                    {
                        var session = result.Value;
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, session.Username),
                            new Claim(ClaimTypes.Role, session.Role),
                            new Claim(ClaimTypes.NameIdentifier, session.EmployeeId)
                        };
                        var identity = new ClaimsIdentity(claims, "apiauth");
                        _currentUser = new ClaimsPrincipal(identity);
                    }
                }
                catch { /* Prerendering sırasında storage erişilemez */ }
            }

            //herkesi giriş yapmamış kabul et 
            return new AuthenticationState(_currentUser);
        }

        //giriş yapıldığında sistemi haberdar etmek için 
        public async Task NotifyUserLogin(string username, string role, string userId)
        {
            // Bilgileri tarayıcı hafızasına (şifreli) yazıyoruz
            var session = new UserSession { Username = username, Role = role, EmployeeId = userId };
            await _localStorage.SetAsync("UserSession", session);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(claims, "apiauth");

            // Hafızadaki kullanıcıyı güncelliyoruz ki sayfa değiştirince bizi atmasın
            _currentUser = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        // Çıkış yapıldığında
        public async Task NotifyUserLogout()
        {
            // Tarayıcı hafızasını temizliyoruz
            await _localStorage.DeleteAsync("UserSession");

            // Hafızayı sıfırlıyoruz
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }

}