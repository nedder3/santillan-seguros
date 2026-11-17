using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Model;
using FederacionSantillan.Model.DTOs;
using FederacionSantillan.Model.Exceptions;
using FederacionSantillan.Model.Validation;

namespace FederacionSantillan.App.Controllers;

public class PortfolioController
{
    private readonly InsuredDao _insuredDao = new InsuredDao();
    private readonly BranchDao _branchDao = new BranchDao();
    private readonly PolicyDao _policyDao = new PolicyDao();
    private readonly InstallmentDao _installmentDao = new InstallmentDao();

    public List<Insured> ListInsureds(string filter = "") => _insuredDao.GetAll(filter);

    public List<Branch> ListBranches() => _branchDao.GetAll();

    public List<PolicySummaryDto> ListPolicies(string filter = "") => _policyDao.GetAll(filter);

    public List<Installment> ListInstallmentsByPolicy(int policyId) => _installmentDao.GetByPolicy(policyId);

    public string SaveInsured(InsuredInputDto dto)
    {
        DtoValidator.Validate(dto);

        string taxId = dto.TaxId.Trim();
        if (_insuredDao.ExistsTaxId(taxId, dto.Id))
            throw new DuplicateEntityException(SystemMessages.Format(SystemMessages.DuplicateTaxId, taxId));

        var insured = new Insured(
            dto.Id,
            taxId,
            dto.FullName.Trim(),
            dto.Phone.Trim(),
            dto.Email?.Trim(),
            dto.Address?.Trim());

        if (dto.Id == 0)
        {
            _insuredDao.Insert(insured);
            return "Asegurado registrado correctamente.";
        }

        _insuredDao.Update(insured);
        return "Datos del asegurado actualizados correctamente.";
    }

    public void DeleteInsured(int id)
    {
        if (id <= 0)
            throw new BusinessRuleException(SystemMessages.SelectionRequired);

        _insuredDao.SoftDelete(id);
    }

    public string IssuePolicy(PolicyInputDto dto)
    {
        DtoValidator.Validate(dto);

        if (dto.EndDate <= dto.StartDate)
            throw new BusinessRuleException(SystemMessages.InvalidPolicyDates);

        if (_policyDao.ExistsPolicyNumber(dto.PolicyNumber))
            throw new DuplicateEntityException(SystemMessages.Format(SystemMessages.DuplicatePolicyNumber, dto.PolicyNumber));

        _policyDao.IssuePolicyWithInstallments(
            dto.PolicyNumber,
            dto.InsuredId,
            dto.BranchId,
            dto.Company.Trim(),
            dto.InsuredAsset.Trim(),
            dto.TotalPremium,
            dto.InstallmentsCount,
            dto.StartDate,
            dto.EndDate,
            dto.FirstDueDate);

        return $"Póliza #{dto.PolicyNumber} emitida con {dto.InstallmentsCount} cuotas generadas.";
    }
}
