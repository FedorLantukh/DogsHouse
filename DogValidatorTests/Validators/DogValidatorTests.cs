using AppLogic.Services;
using DogsHouseWeb.Validators;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogValidatorTests.Validators
{
    [TestFixture]
    public class DogValidatorTests
    {
        private Mock<IDogService> _mockDogService;
        private IDogValidator _dogValidator;

        [SetUp]
        public void Setup()
        {
            _mockDogService = new Mock<IDogService>();
            _dogValidator = new DogValidator(_mockDogService.Object);
        }

        [Test]
        public async Task ValidateDogCreation_ValidDog_ReturnsSuccess()
        {
            // Arrange
            var dog = new Dog
            {
                Name = "Dog1",
                TailLength = 5,
                Weight = 10
            };

            var dogs = new List<Dog>
            {
                new Dog { Name = "Dog2" },
                new Dog { Name = "Dog3" }
            };

            _mockDogService.Setup(service => service.GetAllDogsAsync()).ReturnsAsync(dogs);

            // Act
            var result = await _dogValidator.ValidateDogCreation(dog);

            // Assert
            Assert.IsFalse(result.HasError);
        }

        [Test]
        public async Task ValidateDogCreation_NullDog_ReturnsError()
        {
            // Arrange
            Dog dog = null;

            // Act
            var result = await _dogValidator.ValidateDogCreation(dog);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Dog is null here: ", result.Message);
        }

        [Test]
        public async Task ValidateDogCreation_NegativeTailLength_ReturnsError()
        {
            // Arrange
            var dog = new Dog
            {
                Name = "Dog1",
                TailLength = -5,
                Weight = 10
            };

            // Act
            var result = await _dogValidator.ValidateDogCreation(dog);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Tail length and weight must be positive numbers.", result.Message);
        }

        [Test]
        public async Task ValidateDogCreation_NegativeWeight_ReturnsError()
        {
            // Arrange
            var dog = new Dog
            {
                Name = "Dog1",
                TailLength = 5,
                Weight = -10
            };

            // Act
            var result = await _dogValidator.ValidateDogCreation(dog);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Tail length and weight must be positive numbers.", result.Message);
        }

        [Test]
        public async Task ValidateDogCreation_DuplicateName_ReturnsError()
        {
            // Arrange
            var dog = new Dog
            {
                Name = "Dog1",
                TailLength = 5,
                Weight = 10
            };

            var dogs = new List<Dog>
            {
                new Dog { Name = "Dog1" },
                new Dog { Name = "Dog2" }
            };

            _mockDogService.Setup(service => service.GetAllDogsAsync()).ReturnsAsync(dogs);

            // Act
            var result = await _dogValidator.ValidateDogCreation(dog);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("A dog with the name 'Dog1' already exists.", result.Message);
        }
    }
}
