using IdentityModel.OidcClient;
using Microsoft.Extensions.DependencyInjection;
using NavbarAnimation.Maui.Auth;
using NavbarAnimation.Maui.DataStores;
using NavbarAnimation.Maui.ViewModels.Pages;
using NavbarAnimation.Maui.Views.Pages;

using Sharpnado;
using Sharpnado.CollectionView;
using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;

namespace NavbarAnimation.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Comfortaa-Regular.ttf", "RegularFont");
                fonts.AddFont("Comfortaa-Bold.ttf", "BoldFont");
                fonts.AddFont("Comfortaa-Medium.ttf", "MediumFont");
                fonts.AddFont("Comfortaa-SemiBold.ttf", "SemiBoldFont");
            })
            .UseSharpnadoCollectionView(loggerEnable: false)
            .UseSimpleToolkit()
            .UseSimpleShell();

        // Sharpnado.TaskLoaderView.Initializer.Initialize(true);

#if ANDROID || IOS
        builder.DisplayContentBehindBars();
#endif

        builder.Services.AddSingleton<RibbonPage>();

        builder.Services.AddSingleton(new OidcClient(new()
        {
            Authority = "https://www.benol.com.mx:5000/auth/dev",

            ClientId = "rym.ti.mobile",
            ClientSecret = "UmV5bWEgVEktYjZmZTYxMjctODRlNC00YjJjLTkyNTctNDkzOWQxN2JhNzYwLTIwMjIwMTA2",
            Scope = string.Join(" ", OidcConstants.Scopes),
            RedirectUri = "rym.ti.mobile://callback",

            Browser = new ReymaMauiAuthenticationBrowser()
        }));

        builder.Services.AddTransient<GrAuthorizationHandler>();
        builder.Services.AddHttpClient("ReymaMobileHttpClient", (sp, httpClient) =>
        {
            var oidcClient = sp.GetRequiredService<OidcClient>();
            //var token = ;

            httpClient.BaseAddress = new Uri("https://www.benol.com.mx:5000/reyma/ti/dev/");
            // httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{await SecureStorage.Default.GetAsync("access_token")}");
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"eyJhbGciOiJSUzI1NiIsImtpZCI6IkQ5QTA4NTM1QzQwODgzOTFGOEIxMTdFOTFGMjdEMTk5IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2ODUzODkxNjIsImV4cCI6MTY4NTM5Mjc2MiwiaXNzIjoiaHR0cHM6Ly93d3cuYmVub2wuY29tLm14OjUwMDAvYXV0aC9kZXYiLCJhdWQiOlsicGVybWlzc2lvbnMuc2VydmljZS5hcGkiLCJyeW0udGkudGlja2V0cy5hcGkiLCJyeW0udGkuZ2F0ZXdheS5hcGkiLCJyeW0udGkucmVtaW5kZXJzLmFwaSJdLCJjbGllbnRfaWQiOiJyeW0udGkubW9iaWxlIiwic3ViIjoiZjQ0YmJmMDktOWQ5OC00OTZiLWI1MmYtMGQyNGIzYWE0MTU4IiwiYXV0aF90aW1lIjoxNjg1Mzg5MTYxLCJpZHAiOiJsb2NhbCIsInByZWZlcnJlZF91c2VybmFtZSI6ImpkZWxnYWRvIiwiZW1haWwiOiJqb3JnZS5kZWxnYWRvQHJleW1hLmNvbS5teCIsInJvbGUiOlsib3JnLmV2ZW50b3MuZ2VyZW5jaWEiLCJyeW0uZGlyZWNjaW9uIiwic2lzdGVtYXMiLCJyeW0uY29tcHJhcy5pbm5vdmFjaW9uIiwiUm9sRGlyZWNjaW9uTWFnbmFjZXJvIiwibWFnLmNvbXByYXMuaW5ub3ZhY2lvbiIsInJleW1hLnRpLnRpY2tldHMuc3VzY3JpcHRvciIsInJ5bS50aS5yZWNvcmRhdG9yaW9zLmlubm92YWNpb24iLCJyeW0udGkucmVjb3JkYXRvcmlvcy5jb25zdWx0YSIsIm1lZ2EuY29tcHJhcy5pbm5vdmFjaW9uIiwicnltLnRpLmF1dG9yaXphY2lvbmVzLmdlc3Rpb24iXSwiaHR0cHM6Ly9zY2hlbWFzLnJleW1hLmNvbS5teC9pZGVudGl0eS9jbGFpbXMvZW1wX2lkIjoiNmRmNWUzZGUtZDY1OC00YTc0LThkMTUtN2YwZDhlMDZhNmYzIiwianRpIjoiNTQ5NjZBREM4OUY3MDYxMTY3Q0Y0MDlGMkM4MzEyMzgiLCJzaWQiOiIzQjgwMzQwMjEzRUE0QTI0NDY3NUVEQ0VEQ0Q2OEFDMSIsImlhdCI6MTY4NTM4OTE2Miwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIm9yZ2FuaXphdGlvbmFsIiwicGVybWlzc2lvbnMuc2VydmljZS5yZWFkIiwicnltLnRpLmdhdGV3YXkud3JpdGUiLCJyeW0udGkuZ2F0ZXdheS5yZWFkIiwicnltLnRpLnRpY2tldHMud3JpdGUiLCJyeW0udGkudGlja2V0cy5yZWFkIiwicnltLnRpLnJlbWluZGVycy5yZWFkIiwicnltLnRpLnJlbWluZGVycy53cml0ZSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJwd2QiXX0.eG5T12yuC1mP4FvdVcImtByYaSG6wKbtf2Ngq7OtO3sE2HFc6CVPFAHtLrsdA6osqkAhIpaDEsm0bcEDINnQB1rho8tDvGIqd3Zfdd9Fi4DnnVKRCFr1Cz5_EpeMp2M91hI4YVy_N8X31UGoicdd1KaAJmLJAiVjMjRFS3JnhhPcP_Ba9BGq0itWafNXogtOmKSY7bWjTundnnQo_aE37OFhSSASISoOe_hnj_BlElYjIVaICmhq957ysbwIYiMdDX-aiJuNlDp-4qSgDIOkPvoPZX090m_xBihH1VWYl9Q0jLyP9duVHUetAsb8QvX309_HNP0muXwF4pue4zDSCA");
        }).AddHttpMessageHandler<GrAuthorizationHandler>();

        // /api/tickets/serviciosti/tickets

        // DataStores
        builder.Services.AddScoped(typeof(TicketDataStore));

        // ViewModels
        builder.Services.AddScoped(typeof(RibbonViewModel));

        return builder.Build();
    }
}

internal class OidcConstants
{
    public static List<string> Scopes => new List<string>
    {
        "openid", "profile", "offline_access", "organizational", "permissions.service.read", "rym.ti.gateway.write", "rym.ti.gateway.read", "rym.ti.tickets.write", "rym.ti.tickets.read", "rym.ti.reminders.read", "rym.ti.reminders.write"
    };
}

/// <summary>
///  
/// </summary>
internal class GrAuthorizationHandler : DelegatingHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{await SecureStorage.Default.GetAsync("access_token")}");

        return await base.SendAsync(request, cancellationToken);
    }
}