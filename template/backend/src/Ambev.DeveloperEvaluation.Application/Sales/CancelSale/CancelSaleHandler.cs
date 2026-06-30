using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelSaleHandler
    /// </summary>
    public CancelSaleHandler(ISaleRepository saleRepository, ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CancelSaleCommand request
    /// </summary>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        sale.Cancel(command.Reason);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish domain event
        var saleCancelledEvent = new SaleCancelledEvent(
            sale.Id,
            sale.SaleNumber,
            command.Reason,
            sale.TotalAmount
        );

        _logger.LogInformation(
            "Sale Cancelled Event: SaleId={SaleId}, SaleNumber={SaleNumber}, Reason={Reason}, TotalAmount={TotalAmount}, OccurredAt={OccurredAt}",
            saleCancelledEvent.SaleId,
            saleCancelledEvent.SaleNumber,
            saleCancelledEvent.CancellationReason,
            saleCancelledEvent.TotalAmount,
            saleCancelledEvent.OccurredAt
        );

        return new CancelSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            Success = true,
            CancellationReason = sale.CancellationReason,
            CancelledAt = sale.CancelledAt
        };
    }
}
