using ProductDistribution.Logging;

namespace ProductDistribution.Tests.TestDoubles;

internal sealed class SpyDistributionLogger : IDistributionLogger
{
    private readonly List<string> _messages = [];

    public IReadOnlyCollection<string> Messages => _messages.ToList();

    public void Log(string message)
    {
        _messages.Add(message);
    }
}
