namespace DogsHouseWeb.Validators
{
    public abstract class BaseValidator
    {
        protected ValidationResult Error(string message) 
        {
            return new ValidationResult() { Message = message, HasError = true};
        }
        protected ValidationResult Success()
        {
            return new ValidationResult() { HasError = false };
        }
    }
}
