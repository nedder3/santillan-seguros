using FederacionSantillan.Model.Abstractions;

namespace FederacionSantillan.Model;

public class Policy : ICopiable<Policy>, IClonable<Policy>, IEquatableDomain<Policy>, IIdentifiable<int>
{
    private int policyId, policyNumber, installmentsCount;
    private string company, insuredAsset, status;
    private decimal totalPremium;
    private CustomDate startDate, endDate;
    private Insured holder;
    private Branch branch;

    public int Id => policyId;

    public Policy(
        int id, int number, Insured holder, Branch branch,
        string company, string asset, decimal premium, int installmentsCount,
        CustomDate startDate, CustomDate endDate, string status = "ACTIVE")
    {
        policyId = id;
        policyNumber = number;
        this.holder = holder;
        this.branch = branch;
        this.company = company;
        insuredAsset = asset;
        totalPremium = premium;
        this.installmentsCount = installmentsCount;
        this.startDate = startDate;
        this.endDate = endDate;
        this.status = status;
    }

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
        policyId = other.policyId;
        policyNumber = other.policyNumber;
        company = other.company;
        insuredAsset = other.insuredAsset;
        totalPremium = other.totalPremium;
        installmentsCount = other.installmentsCount;
        status = other.status;
        if (holder != null && other.holder != null) holder.copy(other.holder);
        if (branch != null && other.branch != null) branch.copy(other.branch);
        if (startDate != null && other.startDate != null) startDate.copy(other.startDate);
        if (endDate != null && other.endDate != null) endDate.copy(other.endDate);
    }

    public Policy clone() =>
        new Policy(
            policyId,
            policyNumber,
            holder != null ? holder.clone() : null,
            branch != null ? branch.clone() : null,
            company,
            insuredAsset,
            totalPremium,
            installmentsCount,
            startDate != null ? startDate.clone() : null,
            endDate != null ? endDate.clone() : null,
            status);

    public bool equals(Policy other) =>
        other != null &&
        policyNumber == other.policyNumber &&
        (holder == null ? other.holder == null : holder.equals(other.holder)) &&
        (branch == null ? other.branch == null : branch.equals(other.branch));

    public override string ToString() => $"Policy #{policyNumber} - {holder?.getFullName()} ({status})";
}
