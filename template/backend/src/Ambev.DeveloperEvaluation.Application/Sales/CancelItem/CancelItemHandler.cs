using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Handler for processing CancelItemCommand requests
/// </summary>
public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelItemHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelItemHandler
    /// </summary>
    public CancelItemHandler(ISaleRepository saleRepository, ILogger<CancelItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CancelItemCommand request
    /// </summary>
    public async Task<CancelItemResult> Handle(CancelItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelItemValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        var item = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);

        if (item == null)
            throw new KeyNotFoundException($"Item with ID {command.ItemId} not found in sale");

        sale.CancelItem(command.ItemId, command.Reason);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish domain event
        var itemCancelledEvent = new ItemCancelledEvent(
            sale.Id,
            sale.SaleNumber,
            item.Id,
            item.ProductName,
            item.Quantity,
            command.Reason,
            item.TotalAmount
        );

        _logger.LogInformation(
            "Item Cancelled Event: SaleId={SaleId}, SaleNumber={SaleNumber}, ItemId={ItemId}, Product={ProductName}, Quantity={Quantity}, Reason={Reason}, OccurredAt={OccurredAt}",
            itemCancelledEvent.SaleId,
            itemCancelledEvent.SaleNumber,
            itemCancelledEvent.ItemId,
            itemCancelledEvent.ProductName,
            itemCancelledEvent.Quantity,
            itemCancelledEvent.CancellationReason,
            itemCancelledEvent.OccurredAt
        );

        return new CancelItemResult
        {
            SaleId = sale.Id,
            ItemId = item.Id,
            Success = true,
            NewSaleTotalAmount = sale.TotalAmount
        };
    }
}
