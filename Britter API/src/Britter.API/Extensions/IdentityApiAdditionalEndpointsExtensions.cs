namespace Britter.API.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// MapIdentityEndpoints extensions.
    /// </summary>
    public static class IdentityApiAdditionalEndpointsExtensions
    {
        /// <summary>
        /// Adds logout method endpoint.
        /// </summary>
        /// <typeparam name="TUser">The type of user to use.</typeparam>
        /// <param name="endpoints">The endpoint builder.</param>
        /// <returns>Updated endpoint builder.</returns>
        [ExcludeFromCodeCoverage(Justification = "Test code.")]
        public static IEndpointRouteBuilder MapIdentityApiAdditionalEndpoints<TUser>(this IEndpointRouteBuilder endpoints)
                where TUser : class, new()
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var routeGroup = endpoints.MapGroup("");

            var accountGroup = routeGroup.MapGroup("/account").RequireAuthorization();

            accountGroup.MapPost("/logout", async (SignInManager<TUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            });

            return endpoints;
        }
    }
}
