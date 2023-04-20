using System;

namespace Application._Common.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}