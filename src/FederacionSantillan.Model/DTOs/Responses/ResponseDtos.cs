using FederacionSantillan.Model.Entities;

namespace FederacionSantillan.Model.DTOs.Responses;

public record ErrorResponseDto(string Title, string Message, bool IsFatal = false);

public class PolicyResponseDto
{
    public int PolicyId { get; set; }
    public int PolicyNumber { get; set; }
    public int InsuredId { get; set; }
    public string InsuredName { get; set; }
    public string BranchName { get; set; }
    public string Company { get; set; }
    public string InsuredAsset { get; set; }
    public decimal TotalPremium { get; set; }
    public int InstallmentsCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
}

public class AlertsDashboardResponseDto
{
    public List<Installment> Overdue { get; set; } = new();
    public List<Installment> Upcoming { get; set; } = new();
    public decimal OverdueTotal { get; set; }
    public decimal UpcomingTotal { get; set; }
    public Dictionary<int, Installment> InstallmentIndex { get; set; } = new();
}
