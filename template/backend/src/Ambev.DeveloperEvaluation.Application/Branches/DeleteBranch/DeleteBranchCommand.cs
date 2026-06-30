using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

public class DeleteBranchCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}