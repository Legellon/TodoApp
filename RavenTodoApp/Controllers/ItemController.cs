using System.Text;
using Microsoft.AspNetCore.Mvc;
using RavenTodoApp.Persistence;

namespace RavenTodoApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemRepository _repository;

    public ItemsController(IItemRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public ActionResult GetAll()
    {
        var userToken = HttpContext.Request.Cookies["user-token"];

        var rnd = new Random();
        
        var token = userToken ?? 
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            rnd.Next(100).ToString()));
        
        return RedirectPermanent($"items/{token}");
    } 
    
    [HttpGet("{userToken}")]
    public ActionResult GetAll(string userToken)
    {
        var items = _repository.GetAllRelated(userToken);
        return Ok(new { userToken, items });
    }

    [HttpPost("insert/{item}")]
    public ActionResult InsertOrUpdate(Item item)
    {
        Console.WriteLine(item);
        _repository.InsertOrUpdate(item);
        return Ok();
    }

    [HttpPost("delete/{itemId}")]
    public ActionResult Delete(string itemId)
    {
        _repository.Delete(itemId);
        return Ok();
    }
}