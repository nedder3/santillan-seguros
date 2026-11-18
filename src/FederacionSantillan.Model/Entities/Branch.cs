using FederacionSantillan.Model.Interfaces;

namespace FederacionSantillan.Model.Entities;

public class Branch(int id, string code, string name, string description)
    : ICopiable<Branch>, IClonable<Branch>, IEquatableDomain<Branch>, IIdentifiable<int>
{
    private int branchId = id;
    private string code = code, name = name, description = description;

    public int Id => branchId;
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
        (branchId, code, name, description) = (other.branchId, other.code, other.name, other.description);
    }

    public Branch clone() => new(branchId, code, name, description);

    public bool equals(Branch other) =>
        other != null && (branchId, code, name) == (other.branchId, other.code, other.name);

    public override string ToString() => $"{name} ({code})";
}
