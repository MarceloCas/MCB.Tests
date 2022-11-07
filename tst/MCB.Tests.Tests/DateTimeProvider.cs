using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using System;

namespace MCB.Tests.Tests;

public class DateTimeProvider
    : IDateTimeProvider
{
    // Fields
    private Func<DateTimeOffset> _getDateFunction;

    // Constructors
    public DateTimeProvider()
    {
        _getDateFunction = () => DateTimeOffset.UtcNow;
    }

    // Public Methods
    public void ChangeGetDateCustomFunction(Func<DateTimeOffset> getDateCustomFunction)
    {
        _getDateFunction = getDateCustomFunction;
    }
    public DateTimeOffset GetDate()
    {
        return _getDateFunction();
    }
}
