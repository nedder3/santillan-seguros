using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.DTOs.Responses;
using FederacionSantillan.Model.Entities;

namespace FederacionSantillan.App.Controllers;

public class PortfolioController(
    InsuredDao insuredDao = null,
    BranchDao branchDao = null,
    PolicyDao policyDao = null,
    InstallmentDao installmentDao = null)
{
    private readonly InsuredDao _insuredDao = insuredDao ?? new();
    private readonly BranchDao _branchDao = branchDao ?? new();
    private readonly PolicyDao _policyDao = policyDao ?? new();
    private readonly InstallmentDao _installmentDao = installmentDao ?? new();

    public List<Insured> ListInsureds(string filter = "") => _insuredDao.GetAll(filter);

    public List<Branch> ListBranches() => _branchDao.GetAll();

    public List<PolicyResponseDto> ListPolicies(string filter = "") => _policyDao.GetAll(filter);

    public List<Installment> ListInstallmentsByPolicy(int policyId) => _installmentDao.GetByPolicy(policyId);

    public string SaveInsured(InsuredRequestDto dto) => _insuredDao.Save(dto);

    public void DeleteInsured(int id) => _insuredDao.SoftDelete(id);

    public string IssuePolicy(PolicyRequestDto dto) => _policyDao.IssuePolicyWithInstallments(dto);
}
