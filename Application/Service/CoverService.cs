using Application.DTOs;
using Application.Repository.Contracts;
using Application.Service.Abstraction;
using Application.Service.Contracts;
using AutoMapper;
using Domain.Entities;
using Domain.Types;
using FluentValidation;

namespace Application.Service;

public class CoverService(
             IUnitOfWork unitOfWork,
             ICoverRepository repository,
             IValidator<CoverDTO> validator,
             IMapper mapper) :
             CrudService<Cover, CoverDTO>(unitOfWork, repository, validator, mapper), ICoverService
{
    public const int BASELINE_RATE = 1250;

    private static readonly Dictionary<CoverType, decimal> _multiplierByType = new()
    {
        { CoverType.Yacht, 1.1m },
        { CoverType.PassengerShip, 1.2m },
        { CoverType.Tanker, 1.5m },
        { CoverType.BulkCarrier, 1.3m },
        { CoverType.ContainerShip, 1.3m },
    };

    private static readonly Dictionary<CoverType, List<Discount>> _discountsByCoverType = new()
    {
        { CoverType.Yacht, new List<Discount>
        {
            new(30, 150, 0.05m),
            new(180, 365, 0.08m)
        } },
        { CoverType.PassengerShip, new List<Discount>
        {
            new(30, 150, 0.02m),
            new(180, 365, 0.03m) 
        } },
        { CoverType.Tanker, new List<Discount>
        {
            new(30, 150, 0.02m),
            new(180, 365, 0.03m)
        } },
        { CoverType.BulkCarrier, new List<Discount>
        {
            new(30, 150, 0.02m),
            new(180, 365, 0.03m) 
        } },
        { CoverType.ContainerShip, new List<Discount>
        {
            new(30, 150, 0.02m),
            new(180, 365, 0.03m) 
        } },
    };

    public decimal ComputePremium(ComputePremiumDTO computeDTO)
    {
        var multiplier = GetMultiplierByType(computeDTO.CoverType);
        var premiumPerDay = BASELINE_RATE * multiplier;
        var insuranceLength = (decimal)(computeDTO.EndDate - computeDTO.StartDate).TotalDays;
        var totalPremium = insuranceLength * premiumPerDay;
        var discounts = GetDiscountsByType(computeDTO.CoverType);

        foreach (var discount in discounts)
        {
            totalPremium -= discount.CalculateDiscount(insuranceLength, premiumPerDay);
        }

        return totalPremium;
    }

    protected override void AfterValidation(CoverDTO dto)
    {
        var computePremiumDTO = new ComputePremiumDTO(dto.StartDate, dto.EndDate, dto.Type);
        dto.Premium = ComputePremium(computePremiumDTO);
    }

    private static decimal GetMultiplierByType(CoverType type)
    {
        if (_multiplierByType.TryGetValue(type, out decimal value))
        {
            return value;
        }

        throw new Exception($"{type} has no multiplier value set");
    }

    private static List<Discount> GetDiscountsByType(CoverType type)
    {
        if (_discountsByCoverType.TryGetValue(type, out List<Discount>? value))
        {
            return value;
        }

        throw new Exception($"{type} has no discounts set");
    }
}
