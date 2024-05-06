using Domain.Types;

namespace Application.DTOs;

public record ComputePremiumDTO(DateTime StartDate, DateTime EndDate, CoverType CoverType);
