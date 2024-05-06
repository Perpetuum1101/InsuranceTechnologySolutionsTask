namespace Application.Validation;

public static class ErrorMessages
{
    public const string DAMAGE_COST_EXCEEDS_100000 = "Damage cost exceeds 100000";
    public const string CREATE_DATE_NOT_IN_COVER_DATE_RANGR = "Create date not in cover date range";
    public const string START_DATE_MUST_BE_IN_THE_FUTURE = "Start date must be in the future";
    public const string START_DATE_CANNOT_BE_LATER_THAN_END_DATE = "Start date cannot be later than end date";
    public const string DATE_RANGE_CANNOT_BE_LONGER_THAN_YEAR = "Date range cannot be longer than a year";
}
