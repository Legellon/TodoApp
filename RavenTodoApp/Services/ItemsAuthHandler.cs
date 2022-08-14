using Microsoft.AspNetCore.Authorization;

namespace RavenTodoApp.Services;

public class SameOwnerRequirement : IAuthorizationRequirement
{
}

public class ItemsAuthHandler : AuthorizationHandler<SameOwnerRequirement, Item>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        SameOwnerRequirement requirement, 
        Item resource)
    {
        if (context.User.Identity?.Name == resource.Owner)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}