namespace Kernel.Data.Seed;

public interface IDataSeeder
{
    Task SeedAllAsync();
}