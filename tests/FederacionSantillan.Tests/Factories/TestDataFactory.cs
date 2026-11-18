using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.Entities;

namespace FederacionSantillan.Tests.Factories;

public static class TestDataFactory
{
    public static CustomDate CreateDate(int day = 15, int month = 6, int year = 2026) =>
        new(day, month, year);

    public static Insured CreateInsured(int id = 1, string taxId = "20-34567890-1", string name = "Carlos Gomez") =>
        new(id, taxId, name, "291-4123456", "carlos@email.com", "Av. Alem 1240");

    public static Branch CreateBranch(int id = 1, string code = "AUTO", string name = "Automotor") =>
        new(id, code, name, "Vehicle coverage");

    public static Policy CreatePolicy(int id = 1, int number = 900101) =>
        new(
            id, number, CreateInsured(), CreateBranch(),
            "Federacion Patronal Seguros", "Toyota Hilux 2022",
            360000m, 6, CreateDate(1, 1, 2026), CreateDate(1, 1, 2027));

    public static Installment CreateInstallment(int id = 1, int daysOffset = -5, string status = "PENDING") =>
        new(id, 1, 1, 60000m, DateTime.Today.AddDays(daysOffset), status)
        {
            PolicyNumberRef = 900101,
            InsuredNameRef = "Carlos Gomez",
            InsuredPhoneRef = "291-4123456",
            BranchNameRef = "Automotor"
        };

    public static InsuredRequestDto CreateValidInsuredDto() =>
        new()
        {
            TaxId = "20-34567890-1",
            FullName = "Carlos Gomez",
            Phone = "291-4123456",
            Email = "carlos@email.com",
            Address = "Av. Alem 1240"
        };

    public static RegisterUserRequestDto CreateValidRegisterUserDto() =>
        new()
        {
            FullName = "Ariel Santillan",
            Username = "asantillan",
            Password = "securePassword1",
            ConfirmPassword = "securePassword1",
            Role = "PRODUCTOR"
        };

    public static PolicyRequestDto CreateValidPolicyDto() =>
        new()
        {
            PolicyNumber = 900555,
            InsuredId = 1,
            BranchId = 1,
            Company = "Federacion Patronal Seguros",
            InsuredAsset = "Fiat Cronos 2023",
            TotalPremium = 210000m,
            InstallmentsCount = 6,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddYears(1),
            FirstDueDate = DateTime.Today.AddMonths(1)
        };
}
