namespace MyPage.Models.SESY;

public class SeatBase(int row, int column) : IEquatable<SeatBase>, IComparable<SeatBase>
{
    public int Row { get; init; } = row;
    public int Column { get; init; } = column;

    public bool Equals(SeatBase? other)
        => other is not null && other.Row == Row && other.Column == Column;

    public override bool Equals(object? obj) => Equals(obj as SeatBase);

    public override int GetHashCode() => $"{Row}{Column}".GetHashCode();

    public static bool operator ==(SeatBase a, SeatBase b) => (a.Row == b.Row) && (a.Column == b.Column);
    public static bool operator !=(SeatBase a, SeatBase b) => (a.Row != b.Row) || (a.Column != b.Column);

    public override string ToString()
    {
        return $"Seat-{Row}{Column}";
    }

    public int CompareTo(SeatBase? other)
    {
        if (other!.Row > Row)
        {
            return 1;
        }
        else if (other!.Row < Row)
        {
            return -1;
        }
        else
        {
            if (other!.Column > Column) return 1;
            else if (other!.Column < Column) return -1;
            else return 0;
        }
    }
}

public class SelectSeat(int row, int column, bool isForbidden = false) : SeatBase(row, column)
{
    public bool IsForbidden { get; set; } = isForbidden;
}

public class DistributeSeat(int row, int column, Student? student = null, bool isForbidden = false) : SeatBase(row, column)
{
    public Student? Student { get; set; } = student;
    public bool IsForbidden { get; set; } = isForbidden;
}