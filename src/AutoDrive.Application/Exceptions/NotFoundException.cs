namespace AutoDrive.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entity)
        : base($"{entity} not found.")
    {
    }
}
