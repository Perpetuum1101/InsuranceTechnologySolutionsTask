using Application.DTOs;
using Application.Validation;
using Domain.Types;

namespace Claims.UnitTests.ValidationTests;

public class CoverValidationTests
{
    private CoverValidator _coverValidator;
    private DateTime _now = DateTime.Now;

    public CoverValidationTests()
    {
        _coverValidator = new CoverValidator();
    }

    [Fact]
    public async Task ShouldNotBeValidWhenStartDateInThePast()
    {
        // Arrange
        var dto = new CoverDTO
        {
            StartDate = _now.AddDays(-1),
            EndDate = _now.AddDays(10),
            Type = CoverType.BulkCarrier,
            Premium = 100
        };

        // Act
        var result = await _coverValidator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(
               ErrorMessages.START_DATE_MUST_BE_IN_THE_FUTURE,
               result.Errors.Select(x => x.ErrorMessage).First());

    }

    [Fact]
    public async Task ShouldBeValidWhenStartDateIsNotInThePast()
    {
        // Arrange
        var dto = new CoverDTO
        {
            StartDate = _now.AddDays(1),
            EndDate = _now.AddDays(10),
            Type = CoverType.BulkCarrier,
            Premium = 100
        };

        // Act
        var result = await _coverValidator.ValidateAsync(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldNotBeValidWhenPeriodExceedsOneYear()
    {
        // Arrange
        var dto = new CoverDTO
        {
            StartDate = _now.AddDays(1),
            EndDate = _now.AddDays(367),
            Type = CoverType.BulkCarrier,
            Premium = 100
        };

        // Act
        var result = await _coverValidator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(
               ErrorMessages.DATE_RANGE_CANNOT_BE_LONGER_THAN_YEAR,
               result.Errors.Select(x => x.ErrorMessage).First());
    }

    [Fact]
    public async Task ShouldBeValidWhenPeriodDosNotExceedsOneYear()
    {
        // Arrange
        var dto = new CoverDTO
        {
            StartDate = _now.AddDays(1),
            EndDate = _now.AddDays(10),
            Type = CoverType.BulkCarrier,
            Premium = 100
        };

        // Act
        var result = await _coverValidator.ValidateAsync(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldNotBeValidWhenStartDateIsLaterThanEndDate()
    {
        // Arrange
        var dto = new CoverDTO
        {
            StartDate = _now.AddDays(10),
            EndDate = _now.AddDays(5),
            Type = CoverType.BulkCarrier,
            Premium = 100
        };

        // Act
        var result = await _coverValidator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(
               ErrorMessages.START_DATE_CANNOT_BE_LATER_THAN_END_DATE,
               result.Errors.Select(x => x.ErrorMessage).First());
    }

    [Fact]
    public async Task ShouldNotBeValidWhenStartDateIsEarlierThanEndDate()
    {
        // Arrange
        var dto = new CoverDTO
        {
            StartDate = _now.AddDays(1),
            EndDate = _now.AddDays(10),
            Type = CoverType.BulkCarrier,
            Premium = 100
        };

        // Act
        var result = await _coverValidator.ValidateAsync(dto);

        // Assert
        Assert.True(result.IsValid);
    }
}
