using FederacionSantillan.Tests.Factories;

namespace FederacionSantillan.Tests;

public class InstallmentBillingTests
{
    [Fact]
    public void Given_PendingInstallmentPastDueDate_When_CheckingOverdue_Then_ReturnsTrueAndPositiveDays()
    {
        var installment = TestDataFactory.CreateInstallment(daysOffset: -10, status: "PENDING");

        Assert.True(installment.isOverdue());
        Assert.Equal(10, installment.getDaysOverdue());
    }

    [Fact]
    public void Given_PendingInstallmentDueInSevenDays_When_CheckingUpcomingWindowOfFifteen_Then_ReturnsTrue()
    {
        var installment = TestDataFactory.CreateInstallment(daysOffset: 7, status: "PENDING");

        Assert.True(installment.isUpcoming(15));
        Assert.False(installment.isOverdue());
    }

    [Fact]
    public void Given_PendingInstallment_When_RegisterPaymentTwice_Then_FirstSucceedsAndSecondReturnsFalse()
    {
        var installment = TestDataFactory.CreateInstallment(daysOffset: 0, status: "PENDING");

        bool firstAttempt = installment.registerPayment("Transferencia");
        bool secondAttempt = installment.registerPayment("Efectivo");

        Assert.True(firstAttempt);
        Assert.Equal("PAID", installment.getStatus());
        Assert.NotNull(installment.PaymentDate);
        Assert.False(secondAttempt);
    }

    [Fact]
    public void Given_OverdueInstallment_When_BuildingWhatsAppReminder_Then_IncludesInsuredPolicyAndAmount()
    {
        var installment = TestDataFactory.CreateInstallment(daysOffset: -3, status: "OVERDUE");

        string text = installment.buildWhatsAppReminderText();

        Assert.Contains("Carlos Gomez", text);
        Assert.Contains("900101", text);
        Assert.Contains("VENCIDA", text);
    }
}
