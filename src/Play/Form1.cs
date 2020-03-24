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

//***************************************************************************************************************
namespace Play
{
	public partial class Form1 : Form
	{
		DbOps m_dbOps;
		Group m_activeGroup;
		Member m_mike;
		Member m_diane;
		Member m_lisa;
		Member m_sara;
		Member m_ian;
		Member m_david;

		public Form1()
		{
			InitializeComponent();
			Test();

		}

		//********************************************************************************
		int updatePeriod = 100;

		private void Test()
		{
			m_dbOps = new DbOps();
			m_dbOps.DbOpen();

			// Load some canned test mesh data via code
			LoadData();

			// Display some selected mesh data
			DisplayMesh(m_activeGroup);

			// Test saving && loading from DB
			SaveGroupToDb(m_activeGroup);
			Group dbGroup = m_dbOps.DbLoadGroup("MikeEicks");
			DisplayMesh(dbGroup);


			// Some runtime tests
			while (true)
			{
				UpdateDisplay(m_activeGroup);
				Thread.Sleep(updatePeriod);
			}
			m_dbOps.DbClose();
		}


		//********************************************************************************
		private void DisplayMesh(Group group)
		{
			Console.WriteLine(string.Format("\n******Group {0} ******", group.Name));
			Console.WriteLine(string.Format("\n* Members"));
			foreach (Member m in group.Members)
			{
				string mName = m.CommonName;
				Detail mBirthday = m.GetDetail(DetailType.LifeSpan);
				Console.WriteLine(string.Format("\nGroup Member: {0}", mName));

				Console.WriteLine(string.Format("  Details"));
				foreach (Detail d in m.Details)
				{
					Console.WriteLine(string.Format("    {0}", d.DetailTypeText));
					if (d.StartTimeKnown)
						Console.WriteLine(string.Format("      {0} {1}", d.StartTimeLabel, d.StartTime.ToString())); ;

					if (d.EndTimeKnown)
						Console.WriteLine(string.Format("      {0} {1}", d.EndTimeLabel, d.EndTime.ToString())); ;

					foreach (string story in d.Stories)
						Console.WriteLine(string.Format("      {0}", story));
				}


				Console.WriteLine(string.Format("\n  Relationships"));
				Member relMember;
				string relKind;
				foreach (Relationship rel in m.Relationships)
				{
					if (m == rel.Member1)
					{
						relMember = rel.Member2;
						relKind = rel.Member1Kind;
					}
					else
					{
						relMember = rel.Member1;
						relKind = rel.Member2Kind;
					}

					string relName = relMember.CommonName;
					Detail relBirthday = relMember.GetDetail(DetailType.LifeSpan);

					if (mBirthday.StartTime <= relBirthday.StartTime)
					{
						string timeYounger = Analytics.GetTimeBetween(mBirthday.StartTime, relBirthday.StartTime, PeriodUnits.YearMonthDay, true);
						Console.WriteLine(string.Format("    {0} is {1} to {2}, who is {3} younger than {4}", m.CommonName, relKind, relName, timeYounger, mName));
					}
					else
					{
						string timeOlder = Analytics.GetTimeBetween(relBirthday.StartTime, mBirthday.StartTime, PeriodUnits.YearMonthDay, true);
						Console.WriteLine(string.Format("    {0} is {1} to {2}, who is {3} older than {4}", m.CommonName, relKind, relName, timeOlder, mName));
					}

					foreach (Detail d in rel.Details)
					{
						Console.WriteLine(string.Format("      {0}", d.DetailTypeText));
						if (d.StartTimeKnown)
							Console.WriteLine(string.Format("        {0} : {1}", d.StartTimeLabel, d.StartTime.ToString())); ;

						if (d.EndTimeKnown)
							Console.WriteLine(string.Format("        {0}", d.EndTimeLabel, d.EndTime.ToString())); ;

						foreach (string story in d.Stories)
							Console.WriteLine(string.Format("        {0}", story));
					}
				}
			}

			Console.WriteLine(string.Format("\nFun Facts!"));
			Relationship rSpouse = m_mike.GetRelationship(m_diane, "Wife");
			Detail wedding = rSpouse.GetDetail(DetailType.Wedding);

			Member lisa = group.GetMember("Lisa");
			Detail lBirthday = lisa.GetDetail(DetailType.LifeSpan);
			string howLong = Analytics.GetTimeBetween(wedding.StartTime, lBirthday.StartTime, PeriodUnits.YearMonthDay, true);
			Console.WriteLine(string.Format("  Lisa was born {0} after her parents were married", howLong));

			Member sara = group.GetMember("Sara");
			Detail sBirthday = sara.GetDetail(DetailType.LifeSpan);
			howLong = Analytics.GetTimeBetween(wedding.StartTime, sBirthday.StartTime, PeriodUnits.YearMonthDay, true);
			Console.WriteLine(string.Format("  Sara was born {0} after her parents were married", howLong));

			howLong = Analytics.GetTimeBetween(lBirthday.StartTime, sBirthday.StartTime, PeriodUnits.YearMonthDay, true);
			Console.WriteLine(string.Format("  Sara was born {0} after Lisa was born", howLong));

		}


		//********************************************************************************
		int periodUnitsIndex = 1;
		PeriodUnits[] puOptions =
		{
			PeriodUnits.None,
			PeriodUnits.Years,
			PeriodUnits.Months,
			PeriodUnits.Weeks,
			PeriodUnits.Days,
			PeriodUnits.YearMonthDay,
			PeriodUnits.AllDateUnits,
			PeriodUnits.Hours,
			PeriodUnits.Minutes,
			PeriodUnits.Seconds,
			PeriodUnits.HourMinuteSecond,
			//PeriodUnits.Milliseconds,
			//PeriodUnits.Ticks,
			//PeriodUnits.Nanoseconds,
			PeriodUnits.AllTimeUnits,
			PeriodUnits.DateAndTime,
			//PeriodUnits.AllUnits
		};

		private void UpdateDisplay(Group group)
		{
			string puText = puOptions[periodUnitsIndex].ToString();
			Console.WriteLine("\n******************");
			foreach (Member m in group.Members)
			{
				string mName = m.CommonName;
				Detail mBirthday = m_mike.GetDetail(DetailType.LifeSpan);
				string mAge = Analytics.GetTimeSinceStart(mBirthday, puOptions[periodUnitsIndex], true);
				Console.WriteLine(string.Format("{0} is {1}  ({2})", mName, mAge, puText));
			}
			if (puOptions[periodUnitsIndex] == PeriodUnits.DateAndTime)
			{
				updatePeriod = 2000;
				periodUnitsIndex = 1;
			}
			else
				++periodUnitsIndex;

		}

		//********************************************************************************
		//********************************************************************************

		private void LoadData()
		{
			m_activeGroup = new Group("MikeEicks");

			// Add members and their details
			// *** Mike
			m_mike = m_activeGroup.AddMember("Michael David Eick");
			m_mike.CommonName = "Mike";
			m_mike.AddBirthday(new DateTime(1956, 12, 1), "Mt. Clemens, MI");

			// *** Diane
			m_diane = m_activeGroup.AddMember("Diane Gail Eick");
			m_diane.CommonName = "Diane";
			m_diane.AddBirthday(new DateTime(1957, 1, 25), "Rochester, MI");

			// *** Lisa
			m_lisa = m_activeGroup.AddMember("Lisa Jean Eick");
			m_lisa.CommonName = "Lisa";
			m_lisa.AddBirthday(new DateTime(1988, 2, 21), "Rochester, MI");

			// *** Sara
			m_sara = m_activeGroup.AddMember("Sara Ann Eick");
			m_sara.CommonName = "Sara";
			m_sara.AddBirthday(new DateTime(1993, 5, 11), "Rochester, MI");

			// *** Ian
			m_ian = m_activeGroup.AddMember("Ian Gilmore Mitchel");
			m_ian.CommonName = "Ian";
			m_ian.AddBirthday(new DateTime(1993, 1, 19), "Reno, NV");

			// *** David
			m_david = m_activeGroup.AddMember("David ? Ulmer");
			m_david.CommonName = "David";
			m_david.AddBirthday(new DateTime(1991, 4, 1), "Grosse Pointe, MI");

			// Add Member Relationships
			Relationship rel;
			m_mike.AddRelationShip("Husband", m_diane, "Wife", out rel);
			Detail d = rel.AddDetail(DetailType.Wedding, "Mike & Diane Married", new DateTime(1986, 6, 14));
			d.Stories.Add("St. Johns Lutheran Church");
			d.Stories.Add("New Baltimore");

			m_mike.AddRelationShip("Father", m_lisa, "Daughter", out rel);
			m_mike.AddRelationShip("Father", m_sara, "Daughter", out rel);

			m_diane.AddRelationShip("Mother", m_lisa, "Daughter", out rel);
			m_diane.AddRelationShip("Mother", m_sara, "Daughter", out rel);

			m_lisa.AddRelationShip("Seestra", m_sara, "Seestra", out rel);

			m_lisa.AddRelationShip("Homie", m_ian, "Possee", out rel);
			m_sara.AddRelationShip("Homie", m_david, "Possee", out rel);

			// Add some details
			d = new Detail(DetailType.Event);
			d.StartTimeKnown = true;
			d.StartTime = new DateTime(2020, 2, 29);
			d.Stories.Add("Went to Steven and Jonathan's Wedding Celebration");
			m_mike.Details.Add(d);
			m_diane.Details.Add(d);
			m_lisa.Details.Add(d);
			m_sara.Details.Add(d);
			m_ian.Details.Add(d);
			m_david.Details.Add(d);
		}

			//********************************************************************************
			private void SaveGroupToDb(Group group)
		{
			m_dbOps.DbClearAllData();
			m_dbOps.DbAddGroup(group);
			m_dbOps.DbAddMember(group, m_mike);
			m_dbOps.DbAddMember(group, m_diane);
			m_dbOps.DbAddMember(group, m_lisa);
			m_dbOps.DbAddMember(group, m_sara);
			m_dbOps.DbAddMember(group, m_ian);
			m_dbOps.DbAddMember(group, m_david);
			m_dbOps.DbAddAllRelationships(group);
		}
	}
}
