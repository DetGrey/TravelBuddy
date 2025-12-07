using System.Net.Http.Json;

namespace TravelBuddy.IntegrationTests.Helpers;

public static class AuthHelper
{
    public static async Task AuthenticateAsync(this HttpClient client, string email, string password)
    {
        // 1. Call the real endpoint
        var response = await client.PostAsJsonAsync("/api/users/login", new 
        { 
            Email = email, 
            Password = password
        });

        response.EnsureSuccessStatusCode();

        // 2. Extract the cookie the server gave us
        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            // Replace any existing Cookie header to avoid duplicates across tests
            client.DefaultRequestHeaders.Remove("Cookie");
            // 3. Attach it to the client so ALL future requests in this test use it
            //    (This acts like a browser storing the session)
            client.DefaultRequestHeaders.Add("Cookie", cookies);
        }
        else
        {
            throw new Exception("Login succeeded, but the server didn't send a cookie!");
        }
    }
}