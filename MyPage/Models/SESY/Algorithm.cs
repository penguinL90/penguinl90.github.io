namespace MyPage.Models.SESY;

public static class Algorithm
{
	public static async Task Model1(List<List<DistributeSeat>>	mapSpace,
									List<Student>				studentList,
									int							maxWishCount,
									Action						stateChanged,
									Action<string>				logWrite,
									Action<string>				scrollToRect,
									Action<string>				popUpRect)
	{
		logWrite("演算法使用: Model1");
		for (int w = 0; w <= maxWishCount; ++w)
		{
			foreach (List<DistributeSeat> seatCol in mapSpace)
			{
				foreach (DistributeSeat distributeSeat in seatCol)
				{
					if (distributeSeat.IsForbidden) continue;
					if (w == maxWishCount)
					{
						if (distributeSeat.Student == null && studentList.Count > 0)
						{
                            distributeSeat.Student = studentList[0];
							scrollToRect(distributeSeat.ToString());
							await Task.Delay(250);
							popUpRect(distributeSeat.ToString());
							logWrite($"{studentList[0].ID} 未選上，被自動分配至 ({distributeSeat.Row} {distributeSeat.Column})");
							studentList.RemoveAt(0);
							stateChanged();
							await Task.Delay(250);
						}
						continue;
					}
					if (distributeSeat.Student is not null) continue;
					var foundStudent = (from student in studentList
										where student.WishList.Count - 1 >= w && student.WishList[w] == distributeSeat
                                        orderby student.Weight descending
										select student).ToList();
					if (foundStudent.Count > 0)
					{
                        distributeSeat.Student = foundStudent[0];
						scrollToRect(distributeSeat.ToString());
						await Task.Delay(250);
						popUpRect(distributeSeat.ToString());
						logWrite($"{foundStudent[0].ID}\t成功分配至\t({distributeSeat.Row} {distributeSeat.Column})\t為第{w + 1}志願");
						studentList.Remove(foundStudent[0]);
						stateChanged();
						await Task.Delay(250);
					}
				}
			}
		}
		scrollToRect(mapSpace[0][0].ToString());
		if (studentList.Count > 0) 
		{
			logWrite($"未排到座位之人數{studentList.Count}");
		}
	}
}
