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
}