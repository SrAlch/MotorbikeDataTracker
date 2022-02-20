using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MotorbikeDataTracker.Api.Controllers;
using MotorbikeDataTracker.Api.Dtos;
using MotorbikeDataTracker.Api.Entities;
using MotorbikeDataTracker.Api.Repositories;
using Xunit;

namespace MotorbikeDataTracker.UnitTest
{
    public class MotorbikesControllerTest
    {
        private readonly Mock<IMotorbikesRepository> repositoryStub = new();
        private readonly Mock<ILogger<MotorbikesController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetMotorbikeAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange

            repositoryStub.Setup(repo => repo.GetMotorbikeAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Motorbike)null);

            var controller = new MotorbikesController(repositoryStub.Object, loggerStub.Object);

            // Act

            var result = await controller.GetMotorbikeAsync(Guid.NewGuid());
            
            // Assert

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetMotorbikeAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange

            var expectedMotorbike = CreateRandomMotorbike();

            repositoryStub.Setup(repo => repo.GetMotorbikeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedMotorbike);

            var controller = new MotorbikesController(repositoryStub.Object, loggerStub.Object);
            
            // Act

            var result = await controller.GetMotorbikeAsync(Guid.NewGuid());
            
            // Assert

            result.Result.Should().BeEquivalentTo(expectedMotorbike);
        }

        [Fact]
        public async Task GetMotorbikesAsync_WithExistingItems_ReturnsAllItems()
        {
            // Arrange

            var expectedMotorbikes = new[]{CreateRandomMotorbike(), CreateRandomMotorbike(), CreateRandomMotorbike()};
            repositoryStub.Setup(repo => repo.GetMotorbikesAsync())
                .ReturnsAsync(expectedMotorbikes);

            var controller = new MotorbikesController(repositoryStub.Object, loggerStub.Object);
            
            // Act

            var actualMotorbikes = await controller.GetMotorbikesAsync();
            
            // Assert

            actualMotorbikes.Should().BeEquivalentTo(expectedMotorbikes);
        }

        [Fact]
        public async Task GetMotorbikesAsync_WithMatchinItems_ReturnsMatchingItems()
        {
            // Arrange

            var allMotorbikes = new[]
            {
                new Motorbike(){Brand = "Ducati", Year = 2020, Model = "Panigale V2"},
                new Motorbike(){Brand = "Yamaha", Year = 2020, Model = "R3"},
                new Motorbike(){Brand = "Yamaha", Year = 2009, Model = "R6"},
                new Motorbike(){Brand = "Honda", Year = 2020, Model = "CBR650R"}
            };
            
            var brandToMatch = "Yamaha";

            repositoryStub.Setup(repo => repo.GetMotorbikesAsync())
                .ReturnsAsync(allMotorbikes);

            var controller = new MotorbikesController(repositoryStub.Object, loggerStub.Object);

            // Act

            IEnumerable<MotorbikeDto> foundMotorbikes = await controller.GetMotorbikesAsync(brandToMatch);
            
            // Assert

            foundMotorbikes.Should().OnlyContain(
                motorbike => motorbike.Brand == allMotorbikes[1].Brand || motorbike.Brand == allMotorbikes[2].Brand
            );
        }

        [Fact]
        public async Task CreateMotorbikeAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange

            var motorbikeToCreate = new CreateMotorbikeDto(
                Guid.NewGuid().ToString(),
                rand.Next(1885, 2022),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            );
            
            var controller = new MotorbikesController(repositoryStub.Object, loggerStub.Object);

            // Act

            var result = await controller.CreateMotorbikeAsync(motorbikeToCreate);
            
            // Assert

            var createdMotorbike = (result.Result as CreatedAtActionResult).Value as MotorbikeDto;
            motorbikeToCreate.Should().BeEquivalentTo(
                createdMotorbike,
                options => options.ComparingByMembers<MotorbikeDto>().ExcludingMissingMembers()
            );
            createdMotorbike.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateMotorbikeAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange

            Motorbike existingMotorbike = CreateRandomMotorbike();

            repositoryStub.Setup(repo => repo.GetMotorbikeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingMotorbike);

            var controller = new MotorbikesController(repositoryStub.Object, loggerStub.Object);
            
            var motorbikeId = existingMotorbike.Id;
            var motorbikeToUpdate = new UpdateMotorbikeDto(
                Guid.NewGuid().ToString(),
                rand.Next(1885, 2022),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            );

            // Act

            var result = await controller.UpdateMotorbikeAsync(motorbikeId, motorbikeToUpdate);
            
            // Assert

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteMotorbikeAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange

            Motorbike existingMotorbike = CreateRandomMotorbike();

            repositoryStub.Setup(repo => repo.GetMotorbikeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingMotorbike);

            var controller = new MotorbikesController(repositoryStub.Object, loggerStub.Object);
            
            // Act

            var result = await controller.DeleteMotorbikeAsync(existingMotorbike.Id);
            
            // Assert

            result.Should().BeOfType<NoContentResult>();
        }

        private Motorbike CreateRandomMotorbike()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Brand = Guid.NewGuid().ToString(),
                // TODO: Add current year as dynamic date
                Year = rand.Next(1885, 2022),
                Model = Guid.NewGuid().ToString(),
                Trim = Guid.NewGuid().ToString()
            };
        }
    }
}
