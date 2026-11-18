using FederacionSantillan.Model.Entities;
using FederacionSantillan.Tests.Factories;

namespace FederacionSantillan.Tests;

public class DomainCloningEqualityTests
{
    [Fact]
    public void Given_PolicyWithDeepAssociations_When_CloneIsCalled_Then_CreatesIndependentCopyInDepth()
    {
        var original = TestDataFactory.CreatePolicy();

        var cloned = original.clone();
        cloned.getHolder().setFullName("Modified Name");

        Assert.NotSame(original, cloned);
        Assert.NotSame(original.getHolder(), cloned.getHolder());
        Assert.Equal("Carlos Gomez", original.getHolder().getFullName());
        Assert.False(original.equals(cloned));
    }

    [Fact]
    public void Given_TwoPoliciesWithDifferentState_When_CopyIsCalled_Then_TargetMatchesSourceWithoutChangingReference()
    {
        var target = TestDataFactory.CreatePolicy(1, 100);
        var source = TestDataFactory.CreatePolicy(2, 200);
        source.getHolder().setFullName("Laura Fernandez");

        target.copy(source);

        Assert.NotSame(target, source);
        Assert.NotSame(target.getHolder(), source.getHolder());
        Assert.True(target.equals(source));
    }

    [Fact]
    public void Given_NotificationWithInstallment_When_CloneIsCalled_Then_ClonesChildInstallmentInDepth()
    {
        var installment = TestDataFactory.CreateInstallment();
        var notification = new Notification(1, installment, "OVERDUE", "Reminder text");

        var cloned = notification.clone();

        Assert.NotSame(notification, cloned);
        Assert.NotSame(notification.Installment, cloned.Installment);
        Assert.True(notification.equals(cloned));
    }
}
