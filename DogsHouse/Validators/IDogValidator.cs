using Domain.Models;

namespace DogsHouseWeb.Validators
{
    public interface IDogValidator
    {
        Task<ValidationResult> ValidateDogCreation(Dog dog);
    }
}