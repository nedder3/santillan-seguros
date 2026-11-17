using FederacionSantillan.Model.Abstractions;

namespace FederacionSantillan.Model;

public class CustomDate : ICopiable<CustomDate>, IClonable<CustomDate>, IEquatableDomain<CustomDate>
{
    private int day, month, year;

    public CustomDate(int d, int m, int y)
    {
        day = d;
        month = m;
        year = y;
    }

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
        day = other.day;
        month = other.month;
        year = other.year;
    }

    public CustomDate clone() => new CustomDate(day, month, year);

    public bool equals(CustomDate other) =>
        other != null && day == other.day && month == other.month && year == other.year;

    public bool isValid() =>
        year > 1900 && month >= 1 && month <= 12 && day >= 1 && day <= 31;

    public override string ToString() => $"{day:D2}/{month:D2}/{year}";
}
