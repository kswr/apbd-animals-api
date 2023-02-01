using Microsoft.AspNetCore.Mvc;

namespace AnimalsApi.Animals;

[ApiController]
[Route("api/animals")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalsRepository _animalsRepository;

    public AnimalsController(IAnimalsRepository repository)
    {
        _animalsRepository = repository;
    }

    [HttpGet]
    public IActionResult GetAnimals([FromQuery(Name = "orderBy")] string? orderBy = "name")
    {
        var animals = _animalsRepository.GetAnimals(orderBy!);
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(Animal animal)
    {
        try
        {
            _animalsRepository.Add(animal);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }

    [HttpDelete]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        try
        {
            _animalsRepository.Delete(idAnimal);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
}