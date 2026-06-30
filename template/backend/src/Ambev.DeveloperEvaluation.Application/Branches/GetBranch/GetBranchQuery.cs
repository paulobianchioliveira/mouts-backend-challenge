using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

public class GetBranchQuery : IRequest<GetBranchResult>
{
    public Guid Id { get; set; }
}