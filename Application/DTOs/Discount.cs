namespace Application.DTOs;

public record struct Discount(int From, int To, decimal Percent)
{
    public readonly decimal CalculateDiscount(decimal days, decimal rate)
    {
        if (days <= From)
        {
            return 0;
        }

        var discountDays = days - From - To;
        discountDays = discountDays < 0 ? To + discountDays : To;
        var result = rate * discountDays * Percent;

        return result;
    }
}