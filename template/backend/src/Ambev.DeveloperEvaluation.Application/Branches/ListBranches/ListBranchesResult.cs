namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public class ListBranchesResult
{
    public List<BranchDto> Branches { get; set; } = new();
}

public class BranchDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}