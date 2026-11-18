using FederacionSantillan.Model.Interfaces;

namespace FederacionSantillan.Model.Entities;

public class CustomDate(int d, int m, int y) : ICopiable<CustomDate>, IClonable<CustomDate>, IEquatableDomain<CustomDate>
{
    private int day = d, month = m, year = y;

    public int Day { get => day; set => day = value; }
    public int Month { get => month; set => month = value; }
    public int Year { get => year; set => year = value; }

    public int getDay() => day;
    public void setDay(int d) => day = d;
    public int getMonth() => month;
    public void setMonth(int m) => month = m;
    public int getYear() => year;
    public void setYear(int y) => year = y;

    public void copy(CustomDate other)
    {
        if (other == null) return;
        (day, month, year) = (other.day, other.month, other.year);
    }

    public CustomDate clone() => new(day, month, year);

    public bool equals(CustomDate other) =>
        other != null && (day, month, year) == (other.day, other.month, other.year);

    public bool isValid() =>
        year > 1900 && month is >= 1 and <= 12 && day is >= 1 and <= 31;

    public override string ToString() => $"{day:D2}/{month:D2}/{year}";
}
