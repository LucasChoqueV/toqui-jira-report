using Microsoft.AspNetCore.Http;

namespace TJR.Common.Extensions
{
    public static class HttpClientTokenExtension
    {
        public static void AddToken(this HttpClient client, IHttpContextAccessor context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.HttpContext.Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
                }
            }
        }
    }
}
