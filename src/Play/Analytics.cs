//***************************************************************************************************************

using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Play
{
	//***************************************************************************************************************
	//***************************************************************************************************************
	static public class Analytics
	{

		public static string GetTimeSinceStart(Detail d, PeriodUnits pu, bool label)
		{
			return GetTimeBetween(d.StartTime, DateTime.Now, pu, label);
		}


		public static string GetTimeBetween(DateTime startTime, DateTime endTime, PeriodUnits pu, bool label)
		{
			// https://nodatime.org/2.4.x/userguide/rationale

			LocalDateTime start = new LocalDateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);
			LocalDateTime end = new LocalDateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, endTime.Second);
			Period period = Period.Between(start, end, pu);

			string s = string.Empty;
			if ((pu & PeriodUnits.Years) != 0)
				s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Years, label ? " Years" : "");

			if ((pu & PeriodUnits.Months) != 0)
				s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Months, label ? " Months" : "");

			if ((pu & PeriodUnits.Weeks) != 0)
				s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Weeks, label ? " Weeks" : "");

			if ((pu & PeriodUnits.Days) != 0)
				s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Days, label ? " Days" : "");

			if ((pu & PeriodUnits.Hours) != 0)
				s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Hours, label ? " Hours" : "");

			if ((pu & PeriodUnits.Minutes) != 0)
				s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Minutes, label ? " Minutes" : "");

			if ((pu & PeriodUnits.Seconds) != 0)
				s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Seconds, label ? " Seconds" : "");

			//if ((pu & PeriodUnits.Milliseconds) != 0)
			//	s = s + string.Format("{0}{1}{2}", string.IsNullOrEmpty(s) ? "" : " ", period.Milliseconds, label ? " Milliseconds" : "");

			return s;
		}
	}
}
