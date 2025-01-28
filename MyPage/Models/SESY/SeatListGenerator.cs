using System.Text;

namespace MyPage.Models.SESY;

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
