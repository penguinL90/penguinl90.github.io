using System.Text;
using System.Text.Json.Nodes;

namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        JsonObject jso = new JsonObject()
        {
            ["Fru"] = null,
            ["Row"] = 7,
            ["Col"] = 7,
            ["Fss"] = SeatListGenerator.Summon([new(1, 1), new(2, 2)])
        };
        Console.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes(jso.ToJsonString())));
    }
}

public static class SeatListGenerator
{
    public static string Summon(List<SelectSeat> seatList)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        StringBuilder sb = new();
        writer.Write("SeSy".ToCharArray());
        writer.Write((short)seatList.Count);
        foreach (SelectSeat seat in seatList)
        {
            writer.Write((short)-seat.Row);
            writer.Write((short)-seat.Column);
        }
        writer.Write("sEsY".ToCharArray());
        byte[] ba = stream.ToArray();
        foreach (byte b in ba) sb.Append(b.ToString("X2"));
        return sb.ToString();
    }

    public static List<SeatBase> Parse(string str)
    {
        List<byte> bl = new(str.Length / 2);
        for (int i = 0; i < str.Length; i += 2)
        {
            bl.Add(Convert.ToByte(str[i..(i + 2)], 16));
        }
        MemoryStream stream = new();
        BinaryReader reader = new(stream);
        stream.Write(bl.ToArray());
        stream.Position = 0;
        if (Encoding.UTF8.GetString(reader.ReadBytes(4)) != "SeSy")
        {
            throw new FormatException("Unknown fotmat");
        }
        int count = reader.ReadInt16();
        List<SeatBase> sb = new(count);
        for (int i = 0; i < count; ++i)
        {
            sb.Add(new(-reader.ReadInt16(), -reader.ReadInt16()));
        }
        if (Encoding.UTF8.GetString(reader.ReadBytes(4)) != "sEsY")
        {
            throw new FormatException("Unknown fotmat");
        }
        return sb;
    }
}

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

//public class DistributeSeat(int row, int column, Student? student = null, bool isForbidden = false) : SeatBase(row, column)
//{
//    public Student? Student { get; set; } = student;
//    public bool IsForbidden { get; set; } = isForbidden;
//}