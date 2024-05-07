namespace Infrastructure;
public class CustomConfiguration(bool isIntegrationTesting)
{
    public bool IsIntegrationTesting { get; init; } = isIntegrationTesting;
}
