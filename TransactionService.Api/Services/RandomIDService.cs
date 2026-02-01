namespace TransactionService.Api.Services;

public interface IRandomIDService
{
    Guid RandomID { get; }
}
public class RandomIDService : IRandomIDService
{
    public Guid RandomID { get; } = Guid.NewGuid();
}

public class TransientRandomIDService : IRandomIDService
{
    public Guid RandomID { get; } = Guid.NewGuid();
}
