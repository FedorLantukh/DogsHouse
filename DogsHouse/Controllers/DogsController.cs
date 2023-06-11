using AppLogic.Services;
using DogsHouseWeb.Validators;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DogsHouseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        private readonly IDogService _dogService;
        private readonly IDogValidator _dogValidator;

        public DogsController(IDogService dogService, IDogValidator dogValidator)
        {
            _dogService = dogService;
            _dogValidator = dogValidator;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogs house service. Version 1.0.1");
        }

        /// <summary>
        /// Gets an unsorted list of dogs
        /// </summary>
        [HttpGet("getAllDogs")]
        public async Task<ActionResult<IEnumerable<Dog>>> GetAllDogs()
        {
            return Ok(await _dogService.GetAllDogsAsync());
        }

        /// <summary>
        /// Gets a sorted list of dogs
        /// </summary>
        /// <remarks>
        /// Provide the dog details to sort them by attribute
        /// </remarks>
        /// <param name="order">The sorting order for the dogs (asc, desc).</param>
        [HttpGet("getSortedDogs")]
        public async Task<ActionResult<IEnumerable<Dog>>> GetSortedDogs(string attribute, [FromQuery(Name = "order")] string order)
        {
            
            return Ok(await _dogService.GetSortedDogsAsync(attribute, order));
        }


        /// <summary>
        /// Creates a new dog.
        /// </summary>
        /// <remarks>
        /// Provide the dog details to create a new dog
        /// </remarks>
        [HttpPost("createDog")]
        public async Task<ActionResult<Dog>> CreateDog(Dog dog)
        {
            var validationResult =  await _dogValidator.ValidateDogCreation(dog);

            if (validationResult.HasError)
            {
                return BadRequest(validationResult.Message);
            }
            await _dogService.CreateDogAsync(dog);

            return Ok();
        }
    }
}
