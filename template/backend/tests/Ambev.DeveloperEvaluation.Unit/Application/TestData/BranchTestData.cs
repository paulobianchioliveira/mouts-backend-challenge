using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides test data generation for Branch-related tests
/// </summary>
public static class BranchTestData
{
    private static readonly Faker<CreateBranchCommand> createBranchFaker = new Faker<CreateBranchCommand>()
        .RuleFor(b => b.Name, f => $"Branch {f.Address.City()}")
        .RuleFor(b => b.Code, f => f.Random.Replace("BR-###"))
        .RuleFor(b => b.Address, f => f.Address.StreetAddress())
        .RuleFor(b => b.City, f => f.Address.City())
        .RuleFor(b => b.State, f => f.Address.StateAbbr())
        .RuleFor(b => b.PostalCode, f => f.Address.ZipCode("#####-###"))
        .RuleFor(b => b.ManagerName, f => f.Person.FullName);

    private static readonly Faker<UpdateBranchCommand> updateBranchFaker = new Faker<UpdateBranchCommand>()
        .RuleFor(b => b.Id, f => Guid.NewGuid())
        .RuleFor(b => b.Name, f => $"Branch {f.Address.City()}")
        .RuleFor(b => b.Code, f => f.Random.Replace("BR-###"))
        .RuleFor(b => b.Address, f => f.Address.StreetAddress())
        .RuleFor(b => b.City, f => f.Address.City())
        .RuleFor(b => b.State, f => f.Address.StateAbbr())
        .RuleFor(b => b.PostalCode, f => f.Address.ZipCode("#####-###"))
        .RuleFor(b => b.ManagerName, f => f.Person.FullName)
        .RuleFor(b => b.IsActive, f => true);

    private static readonly Faker<Branch> branchEntityFaker = new Faker<Branch>()
        .RuleFor(b => b.Id, f => Guid.NewGuid())
        .RuleFor(b => b.Name, f => $"Branch {f.Address.City()}")
        .RuleFor(b => b.Code, f => f.Random.Replace("BR-###"))
        .RuleFor(b => b.Address, f => f.Address.StreetAddress())
        .RuleFor(b => b.City, f => f.Address.City())
        .RuleFor(b => b.State, f => f.Address.StateAbbr())
        .RuleFor(b => b.PostalCode, f => f.Address.ZipCode("#####-###"))
        .RuleFor(b => b.ManagerName, f => f.Person.FullName)
        .RuleFor(b => b.IsActive, f => true);

    public static CreateBranchCommand GenerateValidCreateCommand() => createBranchFaker.Generate();

    public static UpdateBranchCommand GenerateValidUpdateCommand() => updateBranchFaker.Generate();

    public static Branch GenerateValidBranch() => branchEntityFaker.Generate();

    public static List<Branch> GenerateBranchList(int count = 5) => branchEntityFaker.Generate(count);
}