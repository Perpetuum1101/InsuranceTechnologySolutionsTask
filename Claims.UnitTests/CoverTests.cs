using Application.DTOs;
using Application.Repository.Contracts;
using Application.Service;
using Application.Service.Contracts;
using AutoMapper;
using Claims.UnitTests.Utility;
using Domain.Entities;
using Domain.Types;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Claims.UnitTests;

public class CoverTests
{
    public const double YACHT_30_DATYS = CoverService.BASELINE_RATE * 30 * 1.1;
    public const double YACHT_25_DATYS = CoverService.BASELINE_RATE * 25 * 1.1;
    public const double TANKER_27_DAYS = CoverService.BASELINE_RATE * 27 * 1.5;
    public const double PASSANGER_SHIP_27_DAYS = CoverService.BASELINE_RATE * 27 * 1.2;
    public const double BULK_CARRIER_27_DAYS = CoverService.BASELINE_RATE * 27 * 1.3;
    public const double CONTAINER_SHIP_27_DAYS = CoverService.BASELINE_RATE * 27 * 1.3;
    public const double YACHT_180_DAYS = (CoverService.BASELINE_RATE * 180 * 1.1) - 
                                         (CoverService.BASELINE_RATE * 150 * 1.1 * 0.05);
    public const double YACHT_160_DAYS = (CoverService.BASELINE_RATE * 160 * 1.1) - 
                                         (CoverService.BASELINE_RATE * 130 * 1.1 * 0.05);
    public const double BULK_CARRIER_120_DAYS = (CoverService.BASELINE_RATE * 120 * 1.3) - 
                                                (CoverService.BASELINE_RATE * 90 * 1.3 * 0.02);
    public const double YACHT_365_DAYS = (CoverService.BASELINE_RATE * 365 * 1.1) -
                                         (CoverService.BASELINE_RATE * 150 * 1.1 * 0.05) -
                                         (CoverService.BASELINE_RATE * 185 * 1.1 * 0.08);
    public const double YACHT_190_DAYS = (CoverService.BASELINE_RATE * 190 * 1.1) -
                                         (CoverService.BASELINE_RATE * 150 * 1.1 * 0.05) -
                                         (CoverService.BASELINE_RATE * 10 * 1.1 * 0.08);
    public const double BULK_CARRIER_250_DAYS = (CoverService.BASELINE_RATE * 250 * 1.3) -
                                                (CoverService.BASELINE_RATE * 150 * 1.3 * 0.02) -
                                                (CoverService.BASELINE_RATE * 70 * 1.3 * 0.03);

    private static readonly DateTime _startDate = new(2024, 1, 1);
    private readonly ICoverService _coverService;

    public CoverTests()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var repository = new Mock<ICoverRepository>();
        var validator = new Mock<IValidator<CoverDTO>>();
        var result = new Mock<ValidationResult>().Setup(x => x.IsValid).Returns(true);
        validator.Setup(x => x.ValidateAsync(
            It.IsAny<CoverDTO>(),
            It.IsAny<CancellationToken>()));
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<CoverDTO, Cover>();
            cfg.CreateMap<Cover, CoverDTO>();
        });
        var mapper = config.CreateMapper();
        _coverService = new CoverService(
                            unitOfWork.Object,
                            repository.Object,
                            validator.Object,
                            mapper);
    }

    [Theory]
    [InlineData(CoverType.Yacht, 1, CoverService.BASELINE_RATE * 1.1)]
    [InlineData(CoverType.Yacht, 30, YACHT_30_DATYS)]
    [InlineData(CoverType.Yacht, 25, YACHT_25_DATYS)]
    [InlineData(CoverType.Tanker, 27, TANKER_27_DAYS)]
    [InlineData(CoverType.PassengerShip, 27, PASSANGER_SHIP_27_DAYS)]
    [InlineData(CoverType.BulkCarrier, 27, BULK_CARRIER_27_DAYS)]
    [InlineData(CoverType.ContainerShip, 27, CONTAINER_SHIP_27_DAYS)]
    [InlineData(CoverType.Yacht, 180, YACHT_180_DAYS)]
    [InlineData(CoverType.Yacht, 160, YACHT_160_DAYS)]
    [InlineData(CoverType.BulkCarrier, 120, BULK_CARRIER_120_DAYS)]
    [InlineData(CoverType.Yacht, 365, YACHT_365_DAYS)]
    [InlineData(CoverType.Yacht, 190, YACHT_190_DAYS )]
    [InlineData(CoverType.BulkCarrier, 250, BULK_CARRIER_250_DAYS)]
    public void ShouldReturnExpectedPremiumForGivenDateRangeAndType(
                CoverType type,
                int days,
                decimal expected)
    {
        // Arrange
        var endDate = _startDate.AddDays(days);
        var dto = new ComputePremiumDTO(_startDate, endDate, type);

        // Act
        var result = _coverService.ComputePremium(dto);

        // Assert
        Assert.Equal(expected, result);
    }
}