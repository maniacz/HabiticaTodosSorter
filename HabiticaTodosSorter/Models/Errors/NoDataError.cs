using AutoMapper.Configuration;
using FluentResults;

namespace HabiticaTodosSorter.Models.Errors;

public class NoDataError : Error
{
    public NoDataError() : base("No data found")
    {
    }

    public NoDataError(string message) : base(message)
    {
    }

    public NoDataError(string message, IError causedBy) : base(message, causedBy)
    {
    }
}