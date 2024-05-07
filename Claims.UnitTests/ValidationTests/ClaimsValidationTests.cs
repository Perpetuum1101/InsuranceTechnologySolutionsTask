using Application.DTOs;
using Application.Repository.Contracts;
using Application.Validation;
using Domain.Entities;
using Domain.Types;
using Moq;

namespace Claims.UnitTests.ValidationTests;

public class ClaimsValidationTests
{
    private ClaimsValidator _claimsValidator;
    private Mock<ICoverRepository> _repo;
    private DateTime _now = new DateTime(2024, 1, 1);

    public ClaimsValidationTests()
    {
        _repo = new Mock<ICoverRepository>();
        _claimsValidator = new ClaimsValidator(_repo.Object);
    }

    [Fact]
    public async Task ShouldNotBeValidWhenDamageCostExceeds100000()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _repo.Setup(x => x.Get(guid)).ReturnsAsync(new Cover
        {
            EndDate = _now.AddDays(1),
            StartDate = _now.AddDays(-1)
        });
        var dto = new ClaimDTO(null, guid, _now, "Test", ClaimType.Fire, 120000);

        // Act
        var result = await  _claimsValidator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(
               ErrorMessages.DAMAGE_COST_EXCEEDS_100000,
               result.Errors.Select(x => x.ErrorMessage).First());
    }

    [Fact]
    public async Task ShouldBeValidWhenDamageCostLessThan100000()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _repo.Setup(x => x.Get(guid)).ReturnsAsync(new Cover
        {
            EndDate = _now.AddDays(1),
            StartDate = _now.AddDays(-1)
        });
        var dto = new ClaimDTO(null, guid, _now, "Test", ClaimType.Fire, 99999);

        // Act
        var result = await _claimsValidator.ValidateAsync(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ShouldBeNotValidWhenCreatedDateNotInCoverDateRange()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _repo.Setup(x => x.Get(guid)).ReturnsAsync(new Cover
        {
            EndDate = _now.AddDays(1),
            StartDate = _now.AddDays(-1)
        });
        var dto = new ClaimDTO(null, guid, _now.AddDays(-3), "Test", ClaimType.Fire, 75);

        // Act
        var result = await _claimsValidator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(
               ErrorMessages.CREATE_DATE_NOT_IN_COVER_DATE_RANGR,
               result.Errors.Select(x => x.ErrorMessage).First());
    }

    [Fact]
    public async Task ShouldBeValidWhenCreatedDateInCoverDateRange()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _repo.Setup(x => x.Get(guid)).ReturnsAsync(new Cover
        {
            EndDate = _now.AddDays(1),
            StartDate = _now.AddDays(-1)
        });
        var dto = new ClaimDTO(null, guid, _now, "Test", ClaimType.Fire, 75);

        // Act
        var result = await _claimsValidator.ValidateAsync(dto);

        // Assert
        Assert.True(result.IsValid);
    }
}
