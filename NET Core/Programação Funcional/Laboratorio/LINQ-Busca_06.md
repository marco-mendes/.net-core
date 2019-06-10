## Criando Relações - Parte 5/5

Nesta parte, vamos criar os controladores para cada uma de nossas entidades, expondo uma interface para o mundo externo.

Nesta parte, todas as criações e alterações - provavelmente - serão feitas no projeto `Application`. Caso queira que o comando `dotnet` execute seu projeto novamente sempre que um arquivo for modificado, basta executar o comando:

```bash
dotnet watch run
```

Desta forma, o `dotnet` verifica todos os arquivos do projeto e sempre que há uma alteração, o projeto é parado e executado novamente, evitando a necessidade de finalizar o processo e iniciar com `dotnet run`.



#### Controlador exemplo - Fabricante

Em sua raiz, todos os controladores de API de vamos implementar expõe um CRUD completo, por isso são bem parecidos. O que define a funcionalidade de cada método é implementando no Serviço/Repositório, dependendo do nível de afinidade com as Regras de Negócio.

```c#
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
```

