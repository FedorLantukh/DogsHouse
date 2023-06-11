using AppLogic.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseWeb.Validators
{
    public class DogValidator : BaseValidator, IDogValidator
    {
        private readonly IDogService _dogService;

        public DogValidator(IDogService dogService)
        {
            _dogService = dogService;
        }

        public async Task<ValidationResult> ValidateDogCreation(Dog dog)
        {
            if (dog == null)
            {
                return Error($"Dog is null here: {dog}");
            }

            if (dog.TailLength <= 0 || dog.Weight <= 0)
            {
                return Error("Tail length and weight must be positive numbers.");
            }

            var dogs = await _dogService.GetAllDogsAsync();
            var dogExists = dogs.Any(x => x.Name.Equals(dog.Name, StringComparison.OrdinalIgnoreCase));

            if (dogExists)
            {
                return Error($"A dog with the name '{dog.Name}' already exists.");
            }

            return Success();
        }

    }
}
