using System.Text;

namespace MyPage.Models.SESY;

public static class SeatListGenerator
{
	public static string Summon(List<SelectSeat> seatList)
	{
		MemoryStream stream = new();
		BinaryWriter writer = new(stream);
		StringBuilder sb = new();
		writer.Write("sy".ToCharArray());
		writer.Write((short)seatList.Count);
		foreach (SelectSeat seat in seatList)
		{
			if (seat.IsForbidden) continue;
			writer.Write((sbyte)-seat.Row);
			writer.Write((sbyte)-seat.Column);
		}
		writer.Write("sY".ToCharArray());
		byte[] ba = stream.ToArray();
		foreach (byte b in ba) sb.Append(b.ToString("X2"));
		return sb.ToString();
	}

    public static string Summon(List<SeatBase> seatList)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        StringBuilder sb = new();
        writer.Write("SeSy".ToCharArray());
        writer.Write((short)seatList.Count);
        foreach (SeatBase seat in seatList)
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
        return str[0..2] switch
        {
            "53" => ParseV1(str),
            "73" => ParseV2(str),
            _ => throw new FormatException("Unknown format"),
        };
    }

	private static List<SeatBase> ParseV1(string str)
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
            throw new FormatException("Unknown format");
        }
        int count = reader.ReadInt16();
        List<SeatBase> sb = new(count);
        for (int i = 0; i < count; ++i)
        {
            sb.Add(new(-reader.ReadInt16(), -reader.ReadInt16()));
        }
        if (Encoding.UTF8.GetString(reader.ReadBytes(4)) != "sEsY")
        {
            throw new FormatException("Unknown format");
        }
        return sb;
    }

    private static List<SeatBase> ParseV2(string str)
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
        if (Encoding.UTF8.GetString(reader.ReadBytes(2)) != "sy")
        {
            throw new FormatException("Unknown format");
        }
        int count = reader.ReadInt16();
        List<SeatBase> sb = new(count);
        for (int i = 0; i < count; ++i)
        {
            sb.Add(new(-reader.ReadSByte(), -reader.ReadSByte()));
        }
        if (Encoding.UTF8.GetString(reader.ReadBytes(2)) != "sY")
        {
            throw new FormatException("Unknown format");
        }
        return sb;
    }
}
