using FederacionSantillan.Model.Interfaces;

namespace FederacionSantillan.Model.Entities;

public class Insured(int id, string taxId, string fullName, string phone, string email = "", string address = "")
    : ICopiable<Insured>, IClonable<Insured>, IEquatableDomain<Insured>, IIdentifiable<int>
{
    private int insuredId = id;
    private string taxId = taxId, fullName = fullName, phone = phone;
    private string email = email ?? string.Empty, address = address ?? string.Empty;
    private bool active = true;

    public Insured(int id) : this(id, "NO_TAX_ID", "NO_NAME", "NO_PHONE") { }

    public int Id => insuredId;
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
        (insuredId, taxId, fullName, phone, email, address, active) =
            (other.insuredId, other.taxId, other.fullName, other.phone, other.email, other.address, other.active);
    }

    public Insured clone() => new(insuredId, taxId, fullName, phone, email, address) { active = active };

    public bool equals(Insured other) =>
        other != null && (insuredId, taxId) == (other.insuredId, other.taxId);

    public override string ToString() => $"[{insuredId}] {fullName} (CUIT/DNI: {taxId}, Tel: {phone})";
}
