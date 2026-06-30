using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Create sale aggregate
        var sale = Sale.Create(
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName
        );

        // Add items to sale
        foreach (var itemDto in command.Items)
        {
            var item = SaleItem.Create(
                itemDto.ProductId,
                itemDto.ProductName,
                itemDto.Quantity,
                itemDto.UnitPrice
            );

            sale.AddItem(item);
        }

        // Persist sale
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Publish domain event (log only - not using message broker)
        var saleCreatedEvent = new SaleCreatedEvent(
            createdSale.Id,
            createdSale.SaleNumber,
            createdSale.CustomerId,
            createdSale.BranchId,
            createdSale.TotalAmount,
            createdSale.Items.Count
        );

        _logger.LogInformation(
            "Sale Created Event: SaleId={SaleId}, SaleNumber={SaleNumber}, Customer={CustomerId}, Branch={BranchId}, Total={TotalAmount}, Items={ItemsCount}, OccurredAt={OccurredAt}",
            saleCreatedEvent.SaleId,
            saleCreatedEvent.SaleNumber,
            saleCreatedEvent.CustomerId,
            saleCreatedEvent.BranchId,
            saleCreatedEvent.TotalAmount,
            saleCreatedEvent.ItemsCount,
            saleCreatedEvent.OccurredAt
        );

        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
}
