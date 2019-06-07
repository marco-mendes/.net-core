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
    public class FabricanteController : ControllerBase
    {
         private FabricanteService<Fabricante> service = new FabricanteService<Fabricante>();

    [HttpPost]
    public IActionResult Post([FromBody] Fabricante item)
    {
        try
        {
            service.Post<FabricanteValidator>(item);
            
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
    public IActionResult Put([FromBody] Fabricante item)
    {
        try
        {
            service.Put<FabricanteValidator>(item);

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
            return new ObjectResult(service.Get());
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{id}", Name = "GetFabricante")]
    public IActionResult Get(int id)
    {
        try
        {
            return new ObjectResult(service.GetWithProdutcs(id));
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