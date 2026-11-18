using FederacionSantillan.Model.Interfaces;

namespace FederacionSantillan.Model.Entities;

public class Policy(
    int id, int number, Insured holder, Branch branch,
    string company, string asset, decimal premium, int installmentsCount,
    CustomDate startDate, CustomDate endDate, string status = "ACTIVE")
    : ICopiable<Policy>, IClonable<Policy>, IEquatableDomain<Policy>, IIdentifiable<int>
{
    private int policyId = id, policyNumber = number, installmentsCount = installmentsCount;
    private string company = company, insuredAsset = asset, status = status;
    private decimal totalPremium = premium;
    private CustomDate startDate = startDate, endDate = endDate;
    private Insured holder = holder;
    private Branch branch = branch;

    public int Id => policyId;
    public int PolicyId { get => policyId; set => policyId = value; }
    public int PolicyNumber { get => policyNumber; set => policyNumber = value; }
    public Insured Holder { get => holder; set => holder = value; }
    public Branch Branch { get => branch; set => branch = value; }
    public string Company { get => company; set => company = value; }
    public string InsuredAsset { get => insuredAsset; set => insuredAsset = value; }
    public decimal TotalPremium { get => totalPremium; set => totalPremium = value; }
    public int InstallmentsCount { get => installmentsCount; set => installmentsCount = value; }
    public CustomDate StartDate { get => startDate; set => startDate = value; }
    public CustomDate EndDate { get => endDate; set => endDate = value; }
    public string Status { get => status; set => status = value; }

    public int getPolicyNumber() => policyNumber;
    public void setPolicyNumber(int n) => policyNumber = n;
    public Insured getHolder() => holder;
    public void setHolder(Insured h) => holder = h;
    public Branch getBranch() => branch;
    public void setBranch(Branch b) => branch = b;
    public decimal getTotalPremium() => totalPremium;
    public void setTotalPremium(decimal p) => totalPremium = p;
    public CustomDate getStartDate() => startDate;
    public void setStartDate(CustomDate d) => startDate = d;
    public CustomDate getEndDate() => endDate;
    public void setEndDate(CustomDate d) => endDate = d;

    public void copy(Policy other)
    {
        if (other == null) return;
        (policyId, policyNumber, company, insuredAsset, totalPremium, installmentsCount, status) =
            (other.policyId, other.policyNumber, other.company, other.insuredAsset, other.totalPremium, other.installmentsCount, other.status);
        if (holder != null && other.holder != null) holder.copy(other.holder);
        if (branch != null && other.branch != null) branch.copy(other.branch);
        if (startDate != null && other.startDate != null) startDate.copy(other.startDate);
        if (endDate != null && other.endDate != null) endDate.copy(other.endDate);
    }

    public Policy clone() =>
        new(
            policyId,
            policyNumber,
            holder?.clone(),
            branch?.clone(),
            company,
            insuredAsset,
            totalPremium,
            installmentsCount,
            startDate?.clone(),
            endDate?.clone(),
            status);

    public bool equals(Policy other) =>
        other != null &&
        policyNumber == other.policyNumber &&
        (holder == null ? other.holder == null : holder.equals(other.holder)) &&
        (branch == null ? other.branch == null : branch.equals(other.branch));

    public override string ToString() => $"Policy #{policyNumber} - {holder?.getFullName()} ({status})";
}
