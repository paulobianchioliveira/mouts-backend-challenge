using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="ListBranchesHandler"/> class.
/// </summary>
public class ListBranchesHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ListBranchesHandler _handler;

    public ListBranchesHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListBranchesHandler(_branchRepository, _mapper);
    }

    [Fact(DisplayName = "Given branches exist When listing branches Then returns all branches")]
    public async Task Handle_BranchesExist_ReturnsAllBranches()
    {
        // Given
        var branches = BranchTestData.GenerateBranchList(5);
        var query = new ListBranchesQuery();
        var branchDtos = branches.Select(b => new BranchDto
        {
            Id = b.Id,
            Name = b.Name,
            Code = b.Code,
            City = b.City,
            State = b.State,
            IsActive = b.IsActive
        }).ToList();

        _branchRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(branches);
        _mapper.Map<List<BranchDto>>(branches).Returns(branchDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Branches.Should().HaveCount(5);
    }
}