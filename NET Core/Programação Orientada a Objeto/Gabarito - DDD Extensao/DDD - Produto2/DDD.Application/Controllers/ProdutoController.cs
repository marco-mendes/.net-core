using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDD.Domain.Entities;
using DDD.Service.Services;
using DDD.Service.Validators;

namespace DDD.Application.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
    private ProdutoService<Produto> service = new ProdutoService<Produto>();

    [HttpPost]
    public IActionResult Post([FromBody] Produto item)
    {
        Console.WriteLine("\n\n\n---");
        Console.WriteLine(item == null);
        Console.WriteLine("---\n\n\n");
        try
        {
            service.Post<ProdutoValidator>(item);
            
            return new ObjectResult(item.Id);
        }
        catch(ArgumentNullException ex)
        {
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut]
    public IActionResult Put([FromBody] Produto item)
    {
        try
        {
            service.Put<ProdutoValidator>(item);

            return new ObjectResult(item);
        }
        catch(ArgumentNullException ex)
        {
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    [HttpDelete("{id}")]
     public IActionResult Delete(int id)
    {
        try
        {
            service.Delete(id);

            return new NoContentResult();
        }
        catch(ArgumentException ex)
        {
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return new ObjectResult(service.GetAll());
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{id}", Name = "GetProduto")]
    public IActionResult Get(int id)
    {
        try
        {
            return new ObjectResult(service.Get(id));
        }
        catch(ArgumentException ex)
        {
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    }
}