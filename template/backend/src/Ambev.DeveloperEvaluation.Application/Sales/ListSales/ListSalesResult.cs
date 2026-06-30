using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Result returned when listing sales
/// </summary>
public class ListSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales
    /// </summary>
    public List<GetSaleResult> Sales { get; set; } = new();

    /// <summary>
    /// Gets or sets the total count of sales
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}
