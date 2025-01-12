using Catalog.Contracts.Products.Dtos;
using Catalog.Products.Features.CreateProduct;
using FluentAssertions;
using FluentValidation;

namespace Api.Integration.Tests.Modules.Catalog.Products.Features;

public class CreateProductHandlerTests : BaseIntegrationTest
{
    public CreateProductHandlerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Create Product Command with Valid Input Should Return Created Product")]
    public async Task CreateProductCommand_ValidInput_ReturnsCreatedProduct()
    {
        var command =
            new CreateProductCommand(
                new ProductDto(
                    Guid.Empty,
                    "Xbox Series X",
                    ["Consoles"],
                    "Some Description",
                    ".png",
                    399));

        var result = await Sender.Send(command);

        result.Should().NotBeNull();
    }

    [Fact(DisplayName = "Create Product Command with Invalid Name Should Throw Validation Exception")]
    public async Task CreateProductCommand_InvalidName_ThrowsValidationException()
    {
        var command =
            new CreateProductCommand(
                new ProductDto(
                    Guid.Empty,
                    string.Empty,
                    ["Consoles"],
                    "Some Description",
                    ".png",
                    399));

        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await Sender.Send(command));

        exception.Errors.Should().HaveCount(1);
        exception.Errors.First().ErrorMessage.Should().Be("Name is required");
    }
}