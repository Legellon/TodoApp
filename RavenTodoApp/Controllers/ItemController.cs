using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RavenTodoApp.Persistence;

namespace RavenTodoApp.Controllers;

[Authorize]
[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthorizationService _authService;

    public ItemsController(IItemRepository itemRepository, IAuthorizationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
    }

    [HttpGet("allBy/{userToken}")]
    public async Task<IActionResult> GetAll(string userToken)
    {
        var items = _itemRepository.GetAllRelatedItems(userToken);

        var authorizationResult = await _authService.AuthorizeAsync(
            User, items.FirstOrDefault(), "AccessPolicy");

        if (!authorizationResult.Succeeded)
        {
            return new NotFoundResult();
        }

        return Ok(new {items});
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] Item item)
    {
        var authorizationResult = await _authService.AuthorizeAsync(
            User, item, "AccessPolicy");

        if (!authorizationResult.Succeeded)
        {
            return new ForbidResult();
        }

        item.Owner = User.Identity?.Name;
        _itemRepository.InsertOrUpdate(item);

        return Ok(new {success = true});
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] Item item)
    {
        _itemRepository.InsertOrUpdate(new Item()
        {
            Title = item.Title,
            Owner = User.Identity?.Name
        });

        return Ok(new {success = true});
    }

    [HttpPost("delete/{*itemId}")]
    public async Task<IActionResult> Delete(string itemId)
    {
        var accessedItem = _itemRepository.Get(itemId);

        var authorizationResult = await _authService.AuthorizeAsync(
            User, accessedItem, "AccessPolicy");

        if (!authorizationResult.Succeeded)
        {
            return new ForbidResult();
        }

        _itemRepository.Delete(itemId);
        return Ok(new {success = true});
    }
}