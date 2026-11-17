using FederacionSantillan.Model.Abstractions;

namespace FederacionSantillan.Model;

public class Branch : ICopiable<Branch>, IClonable<Branch>, IEquatableDomain<Branch>, IIdentifiable<int>
{
    private int branchId;
    private string code, name, description;

    public int Id => branchId;

    public Branch(int id, string code, string name, string description)
    {
        branchId = id;
        this.code = code;
        this.name = name;
        this.description = description;
    }

    public int BranchId { get => branchId; set => branchId = value; }
    public string Code { get => code; set => code = value; }
    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }

    public int getBranchId() => branchId;
    public void setBranchId(int id) => branchId = id;
    public string getCode() => code;
    public void setCode(string c) => code = c;
    public string getName() => name;
    public void setName(string n) => name = n;
    public string getDescription() => description;
    public void setDescription(string d) => description = d;

    public void copy(Branch other)
    {
        if (other == null) return;
        branchId = other.branchId;
        code = other.code;
        name = other.name;
        description = other.description;
    }

    public Branch clone() => new Branch(branchId, code, name, description);

    public bool equals(Branch other) =>
        other != null && branchId == other.branchId && code == other.code && name == other.name;

    public override string ToString() => $"{name} ({code})";
}
