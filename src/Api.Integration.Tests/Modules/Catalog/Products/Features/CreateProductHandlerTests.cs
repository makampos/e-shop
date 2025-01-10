using Catalog.Contracts.Products.Dtos;
using Catalog.Products.Features.CreateProduct;
using FluentAssertions;

namespace Api.Integration.Tests.Modules.Catalog.Products.Features;

public class CreateProductHandlerTests : BaseIntegrationTest
{
    public CreateProductHandlerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_ShouldCreateProduct()
    {
        var command =
            new CreateProductCommand(
                new ProductDto(
                    Guid.NewGuid(),
                    "Xbox Series X",
                    ["Consoles"],
                    "Some Description",
                    ".png",
                    399));

        var result = await Sender.Send(command);

        result.Should().NotBeNull();

    }
}