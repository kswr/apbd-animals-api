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
   public IActionResult GetAnimals()
   {
      var animals = _animalsRepository.GetAnimals();
      return Ok(animals);
   }
}