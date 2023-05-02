namespace Microsoft.AspNetCore.Authorization
{
    public class BanRequirement : IAuthorizationRequirement
    {

    }

    public class BanAuthorizationHandler : AuthorizationHandler<BanRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BanRequirement requirement)
        {
            var banClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "Role" && claim.Value == "Ban");

            if (banClaim is not null)
            {
                context.Fail();
            } 
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
