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

    [Fact(DisplayName = "Create Product Command with Invalid Price Should Throw Validation Exception")]
    public async Task CreateProductCommand_InvalidPrice_ThrowsValidationException()
    {
        var command =
            new CreateProductCommand(
                new ProductDto(
                    Guid.Empty,
                    "Playstation 5",
                    ["Consoles"],
                    "Some Description",
                    ".png",
                    0));

        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await Sender.Send(command));

        exception.Errors.Should().HaveCount(1);
        exception.Errors.First().ErrorMessage.Should().Be("Price must be greater than 0");
    }

    [Fact(DisplayName = "Create Product Command with Invalid Category Should Throw Validation Exception")]
    public async Task CreateProductCommand_InvalidCategory_ThrowsValidationException()
    {
        var command =
            new CreateProductCommand(
                new ProductDto(
                    Guid.Empty,
                    "Playstation 5",
                    [],
                    "Some Description",
                    ".png",
                    399));

        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await Sender.Send(command));

        exception.Errors.Should().HaveCount(1);
        exception.Errors.First().ErrorMessage.Should().Be("Category is required");
    }

    [Fact(DisplayName = "Create Product Command with Invalid Image File Should Throw Validation Exception")]
    public async Task CreateProductCommand_InvalidImageFile_ThrowsValidationException()
    {
        var command =
            new CreateProductCommand(
                new ProductDto(
                    Guid.Empty,
                    "Playstation 5",
                    ["Consoles"],
                    "Some Description",
                    string.Empty,
                    399));

        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await Sender.Send(command));

        exception.Errors.Should().HaveCount(1);
        exception.Errors.First().ErrorMessage.Should().Be("ImageFile is required");
    }
}