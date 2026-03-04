using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace HrPanel.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        //sistemdeki mevcut kullanıcıyı döndürür
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //herkesi giriş yapmamış kabul et 
            return Task.FromResult(new AuthenticationState(_anonymous));
        }

        //giriş yapıldığında sistemi haberdar etmek için 
        public void NotifyUserLogin(string username)
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "apiauth");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        // Çıkış yapıldığında
        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}