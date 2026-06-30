using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        var previousTotal = sale.TotalAmount;

        // Clear existing items and add new ones
        // Note: In a real scenario, you might want to track changes more precisely
        var itemsToRemove = sale.Items.ToList();
        foreach (var item in itemsToRemove)
        {
            sale.RemoveItem(item.Id);
        }

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

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish domain event
        var saleModifiedEvent = new SaleModifiedEvent(
            updatedSale.Id,
            updatedSale.SaleNumber,
            previousTotal,
            updatedSale.TotalAmount,
            "Sale items updated"
        );

        _logger.LogInformation(
            "Sale Modified Event: SaleId={SaleId}, SaleNumber={SaleNumber}, PreviousTotal={PreviousTotal}, NewTotal={NewTotal}, OccurredAt={OccurredAt}",
            saleModifiedEvent.SaleId,
            saleModifiedEvent.SaleNumber,
            saleModifiedEvent.PreviousTotalAmount,
            saleModifiedEvent.NewTotalAmount,
            saleModifiedEvent.OccurredAt
        );

        var result = _mapper.Map<UpdateSaleResult>(updatedSale);
        return result;
    }
}
