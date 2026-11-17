using FederacionSantillan.Model.Abstractions;

namespace FederacionSantillan.Model;

public class Insured : ICopiable<Insured>, IClonable<Insured>, IEquatableDomain<Insured>, IIdentifiable<int>
{
    private int insuredId;
    private string taxId, fullName, phone, email, address;
    private bool active;

    public int Id => insuredId;

    public Insured(int id)
        : this(id, "NO_TAX_ID", "NO_NAME", "NO_PHONE", string.Empty, string.Empty) { }

    public Insured(int id, string taxId, string fullName, string phone, string email = "", string address = "")
    {
        insuredId = id;
        this.taxId = taxId;
        this.fullName = fullName;
        this.phone = phone;
        this.email = email ?? string.Empty;
        this.address = address ?? string.Empty;
        active = true;
    }

    public int InsuredId { get => insuredId; set => insuredId = value; }
    public string TaxId { get => taxId; set => taxId = value; }
    public string FullName { get => fullName; set => fullName = value; }
    public string Phone { get => phone; set => phone = value; }
    public string Email { get => email; set => email = value; }
    public string Address { get => address; set => address = value; }
    public bool Active { get => active; set => active = value; }

    public int getInsuredId() => insuredId;
    public void setInsuredId(int id) => insuredId = id;
    public string getTaxId() => taxId;
    public void setTaxId(string t) => taxId = t;
    public string getFullName() => fullName;
    public void setFullName(string n) => fullName = n;
    public string getPhone() => phone;
    public void setPhone(string p) => phone = p;
    public string getEmail() => email;
    public void setEmail(string e) => email = e;
    public string getAddress() => address;
    public void setAddress(string a) => address = a;

    public void copy(Insured other)
    {
        if (other == null) return;
        insuredId = other.insuredId;
        taxId = other.taxId;
        fullName = other.fullName;
        phone = other.phone;
        email = other.email;
        address = other.address;
        active = other.active;
    }

    public Insured clone() => new Insured(insuredId, taxId, fullName, phone, email, address) { active = active };

    public bool equals(Insured other) =>
        other != null && insuredId == other.insuredId && taxId == other.taxId;

    public override string ToString() => $"[{insuredId}] {fullName} (CUIT/DNI: {taxId}, Tel: {phone})";
}
