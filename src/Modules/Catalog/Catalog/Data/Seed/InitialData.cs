namespace Catalog.Data.Seed;

public static class InitialData
{
    public static IEnumerable<Product> Products => new List<Product>()
    {
        Product.Create(
            id: Guid.NewGuid(),
            name: "Xbox Series X",
            category: ["Consoles"],
            description: "Power Your Dreams",
            imageFile: ".png",
            price: 499),
        Product.Create(
            id: Guid.NewGuid(),
            name: "Playstation 5",
            category: ["Consoles"],
            description: "Play Has No Limits",
            imageFile: ".png",
            price: 599)
    };
}