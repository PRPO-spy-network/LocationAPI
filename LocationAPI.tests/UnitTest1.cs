using Locations.Controllers;
using Locations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Moq.EntityFrameworkCore;

namespace LocationAPI.tests
{
	public class UnitTest1
	{
		private readonly Mock<ILogger<LocationController>> _mockLogger;
		private readonly Mock<IDbContextFactory<PostgresContext>> _mockDbContextFactory;
		private readonly LocationController _controller;
		private readonly Mock<PostgresContext> _mockContext;
		private readonly Mock<DbSet<CarGpsData>> _mockCarGpsDataSet;
		public UnitTest1()
		{
			_mockLogger = new Mock<ILogger<LocationController>>();
			_mockDbContextFactory = new Mock<IDbContextFactory<PostgresContext>>();
			_mockContext = new Mock<PostgresContext>(new DbContextOptions<PostgresContext>());
			_mockCarGpsDataSet = new Mock<DbSet<CarGpsData>>();

			_mockDbContextFactory
				.Setup(f => f.CreateDbContext())
				.Returns(_mockContext.Object);

			_mockContext
				.Setup(c => c.CarGpsData)
				.Returns(_mockCarGpsDataSet.Object);

			_controller = new LocationController(_mockLogger.Object, _mockDbContextFactory.Object);
		}

		[Fact]
		public void TestGetAllInTimeframe()
		{
			// Arrange
			var testData = new List<CarGpsData>
			{
				new CarGpsData { CarId = "car1", Time = DateTime.UtcNow.AddDays(-1) },
				new CarGpsData { CarId = "car2", Time = DateTime.UtcNow.AddDays(-2) }
			};

			_mockContext
				.Setup(x => x.CarGpsData)
				.ReturnsDbSet(testData);

			// Act
			var result = _controller.Get(null);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedData = Assert.IsType<List<CarGpsData>>(okResult.Value);
			Assert.Equal(2, returnedData.Count);
		}

		[Fact]
		public void Get_WithCarIdAndTimeframe_ReturnsFilteredData()
		{
			// Arrange
			var carId = "car1";
			var timeframe = 2;
			var testData = new List<CarGpsData>
			{
				new CarGpsData { CarId = "car1", Time = DateTime.UtcNow.AddDays(-1) },
				new CarGpsData { CarId = "car1", Time = DateTime.UtcNow.AddDays(-3) },
				new CarGpsData { CarId = "car2", Time = DateTime.UtcNow.AddDays(-1) }
			};

			_mockContext
				.Setup(x => x.CarGpsData)
				.ReturnsDbSet(testData);

			// Act
			var result = _controller.Get(carId, timeframe);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedData = Assert.IsType<List<CarGpsData>>(okResult.Value);
			Assert.Single(returnedData);
			Assert.Equal("car1", returnedData[0].CarId);
			Assert.True(returnedData[0].Time >= DateTime.UtcNow.AddDays(-timeframe));
		}
	}
}