using DataAccess.Repository;
using Domain.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLogic.Services;

namespace AppLogicTests.Services
{
    [TestFixture]
    public class DogServiceTests
    {
        private Mock<IRepository<Dog>> _mockDogRepo;
        private IDogService _dogService;

        [SetUp]
        public void Setup()
        {
            _mockDogRepo = new Mock<IRepository<Dog>>();
            _dogService = new DogService(_mockDogRepo.Object);
        }

        [Test]
        public async Task GetAllDogsAsync_ReturnsAllDogs()
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new Dog { Id = 1, Name = "Dog1" },
                new Dog { Id = 2, Name = "Dog2" },
                new Dog { Id = 3, Name = "Dog3" }
            };

            _mockDogRepo.Setup(repo => repo.GetAllEntitiesAsync()).ReturnsAsync(dogs);

            // Act
            var result = await _dogService.GetAllDogsAsync();

            // Assert
            Assert.AreEqual(dogs.Count, result.Count());
            Assert.AreEqual(dogs, result);
        }

        [Test]
        public async Task CreateDogAsync_CallsAddEntityAsync()
        {
            // Arrange
            var dog = new Dog { Id = 1, Name = "Dog1" };

            // Act
            await _dogService.CreateDogAsync(dog);

            // Assert
            _mockDogRepo.Verify(repo => repo.AddEntityAsync(dog), Times.Once);
        }

        [Test]
        public async Task GetSortedDogsAsync_SortsDogsByAttribute()
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new Dog { Id = 1, Name = "Dog1", TailLength = 5 },
                new Dog { Id = 2, Name = "Dog2", TailLength = 3 },
                new Dog { Id = 3, Name = "Dog3", TailLength = 7 }
            };

            var attribute = "TailLength";
            var order = "desc";

            _mockDogRepo.Setup(repo => repo.GetAllEntitiesAsync()).ReturnsAsync(dogs);

            // Act
            var result = await _dogService.GetSortedDogsAsync(attribute, order);

            // Assert
            var sortedDogs = dogs.OrderByDescending(d => d.TailLength).ToList();
            Assert.AreEqual(sortedDogs.Count, result.Count());
            Assert.AreEqual(sortedDogs, result);
        }

        [Test]
        public async Task GetSortedDogsAsync_ReturnsPaginatedResults()
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new Dog { Id = 1, Name = "Dog1", TailLength = 5 },
                new Dog { Id = 2, Name = "Dog2", TailLength = 3 },
                new Dog { Id = 3, Name = "Dog3", TailLength = 7 },
                new Dog { Id = 4, Name = "Dog4", TailLength = 2 },
                new Dog { Id = 5, Name = "Dog5", TailLength = 6 }
            };

            var attribute = "TailLength";
            var order = "asc";
            var pageNumber = 2;
            var limit = 2;

            _mockDogRepo.Setup(repo => repo.GetAllEntitiesAsync()).ReturnsAsync(dogs);

            // Act
            var result = await _dogService.GetSortedDogsAsync(attribute, order, pageNumber, limit);

            // Assert
            var sortedDogs = dogs.OrderBy(d => d.TailLength).Skip((pageNumber - 1) * limit).Take(limit).ToList();
            Assert.AreEqual(sortedDogs.Count, result.Count());
            Assert.AreEqual(sortedDogs, result);
        }
    }
}

