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

	//***************************************************************************************************************
	//***************************************************************************************************************
	public class DbOps
	{
		SQLiteConnection m_dbConn;
//		string m_dbFile = @"URI=file:C:\Sourcetree\x\MyAge\Play\PlaySqlLite.db";
		string m_dbFile = @"URI=file:..\..\..\PlaySqlLite.db";
		DateTime m_noDateTime;

		public DbOps()
		{
			m_noDateTime = new DateTime(0);
		}

		public bool DbOpen()
		{
			try
			{
				m_dbConn = new SQLiteConnection(m_dbFile);
				m_dbConn.Open();
				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}

		public bool DbClose()
		{
			try
			{
				m_dbConn.Close();
				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}

		//***************************************************************************************************************
		public bool DbClearAllData()
		{
			string[] tableArr = { "Groups", "Members", "Relationships", "Details", "Stories", "GroupMember", "DetailLinks" };

			try
			{
				foreach (string s in tableArr)
				{
					SQLiteCommand cmd = m_dbConn.CreateCommand();
					cmd.CommandText = string.Format("DELETE from {0}", s);
					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}


		//***************************************************************************************************************
		public bool DbAddGroup(Group group)
		{
			int groupIdx = -1;

			try
			{
				SQLiteCommand cmd;
				cmd = m_dbConn.CreateCommand();
				cmd.CommandText = string.Format("INSERT INTO Groups (Name) VALUES('{0}')", group.Name);
				cmd.ExecuteNonQuery();

				long rowId = m_dbConn.LastInsertRowId;
				string stm = string.Format("SELECT GroupIdx FROM Groups Where ROWID='{0}'", rowId);
				cmd = new SQLiteCommand(stm, m_dbConn);
				SQLiteDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					groupIdx = rdr.GetInt32(0);
				}
				rdr.Close();
				group.GroupIdx = groupIdx;

				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}

		public Group DbLoadGroup(string groupName)
		{
			try
			{
				string stm = "SELECT * FROM Groups";
				var cmd = new SQLiteCommand(stm, m_dbConn);
				SQLiteDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					string gName = rdr.GetString(1);
					if (gName == groupName)
					{
						int groupIdx = rdr.GetInt32(0);
						Group g = new Group(groupName);
						g.GroupIdx = groupIdx;
						DbLoadMembers(g);
						return g;
					}
				}
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return null;
		}


		//***************************************************************************************************************
		public bool DbAddMember(Group group, Member member)
		{
			try
			{
				int memberIdx = -1;

				string stm = string.Format("INSERT INTO Members (Name,CommonName) VALUES ('{0}' , '{1}')", member.Name, member.CommonName);
				SQLiteCommand cmd = new SQLiteCommand(stm, m_dbConn);
				cmd.ExecuteNonQuery();

				long rowId = m_dbConn.LastInsertRowId;
				stm = string.Format("SELECT MemberIdx FROM Members Where ROWID='{0}'", rowId);
				cmd = new SQLiteCommand(stm, m_dbConn);

				SQLiteDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					memberIdx = rdr.GetInt32(0);
				}
				rdr.Close();
				member.MemberIdx = memberIdx;

				stm = string.Format("INSERT INTO GroupMember (GroupIdx, MemberIdx) VALUES ({0} , {1})", group.GroupIdx, member.MemberIdx);
				cmd = new SQLiteCommand(stm, m_dbConn);
				cmd.ExecuteNonQuery();

				// Now add details for this member
				foreach (Detail d in member.Details)
					DbAddDetail(group, member, d);
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}


		public bool DbLoadMembers(Group group)
		{
			try
			{
				// Get MemberIdx for all that belong to this grsoup
				string stm = string.Format("SELECT * FROM GroupMember Where GroupIdx = {0}", group.GroupIdx);
				var cmd = new SQLiteCommand(stm, m_dbConn);
				SQLiteDataReader rdr = cmd.ExecuteReader();

				List<int> memberIdxList = new List<int>();
				while (rdr.Read())
				{
					int memberIdx = rdr.GetInt32(1);
					memberIdxList.Add(memberIdx);
				}

				// Now load the members
				foreach (int idx in memberIdxList)
				{
					stm = string.Format("SELECT * FROM Members Where MemberIdx = {0}", idx);
					cmd = new SQLiteCommand(stm, m_dbConn);
					rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{
						Member m = new Member(rdr.GetString(1));
						m.MemberIdx = rdr.GetInt32(0);
						m.CommonName = rdr.GetString(2);
						group.Members.Add(m);
					}

				}

				// Now load Details foreach member!
				foreach (Member m in group.Members)
				{
					DbLoadRelationships(group, m);
				}
				DbLoadDetails(group);


				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}


		//***************************************************************************************************************
		public bool DbAddDetail(Group group, Detail detail)
		{
			bool rc;
			rc = DbAddDetail(group.GroupIdx, -1, -1, detail);
			rc = DbAddDetailLink(group, detail, -1, -1);
			return rc;
		}

		public bool DbAddDetail(Group group, Member member, Detail detail)
		{
			bool rc;
			rc = DbAddDetail(group.GroupIdx, member.MemberIdx, -1, detail);
			rc = DbAddDetailLink(group, detail, member.MemberIdx, -1);
			return rc;
		}

		public bool DbAddDetail(Group group, Relationship rel, Detail detail)
		{
			bool rc;
			rc = DbAddDetail(group.GroupIdx, -1, rel.RelationshipIdx, detail);
			rc = DbAddDetailLink(group, detail, -1, rel.RelationshipIdx);
			return rc;
		}

		//ME: remove indexes ...
		public bool DbAddDetail(int groupIdx, int memberIdx, int relationshipIdx, Detail detail)
		{
			try
			{
				int detailIdx = -1;

				string detailType = detail.DType == DetailType.Custom ? detail.CustomDetailText : detail.DType.ToString();
				string stm = string.Format("INSERT INTO Details (GroupIdx, StartTime, EndTime, DetailType) VALUES ('{0}', '{1}', '{2}', '{3}')",
					groupIdx,
					detail.StartTimeKnown ? detail.StartTime.ToString() : string.Empty,
					detail.EndTimeKnown ? detail.EndTime.ToString() : string.Empty,
					detailType
					);
				SQLiteCommand cmd = new SQLiteCommand(stm, m_dbConn);
				cmd.ExecuteNonQuery();

				long rowId = m_dbConn.LastInsertRowId;
				stm = string.Format("SELECT DetailIdx FROM Details Where ROWID='{0}'", rowId);
				cmd = new SQLiteCommand(stm, m_dbConn);
				SQLiteDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					detailIdx = rdr.GetInt32(0);
				}
				rdr.Close();
				detail.DetailIdx = detailIdx;


				foreach (string s in detail.Stories)
				{
					string s2 = s.Replace("'", "''");
					stm = string.Format("INSERT INTO Stories (DetailIdx, Story) VALUES ('{0}', '{1}')",
						detail.DetailIdx, s2);

					cmd = new SQLiteCommand(stm, m_dbConn);
					cmd.ExecuteNonQuery();

					rowId = m_dbConn.LastInsertRowId;
					stm = string.Format("SELECT StoryIdx FROM Stories Where ROWID='{0}'", rowId);
					cmd = new SQLiteCommand(stm, m_dbConn);
					rdr = cmd.ExecuteReader();

					while (rdr.Read())
					{
						int storyIdx = rdr.GetInt32(0); //ME: May need this!
					}
					rdr.Close();
				}
				return true;

			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}

			return false;
		}


		public bool DbAddDetailLink(Group group, Detail detail, int memberIdx, int relationshipIdx)
		{
			try
			{
				string detailType = detail.DType == DetailType.Custom ? detail.CustomDetailText : detail.DType.ToString();
				string stm = string.Format("INSERT INTO DetailLinks (GroupIdx, DetailIdx, MemberIdx, RelationshipIdx) VALUES ('{0}', '{1}', '{2}', '{3}')",
					group.GroupIdx,
					detail.DetailIdx,
					memberIdx,
					relationshipIdx
					);
				SQLiteCommand cmd = new SQLiteCommand(stm, m_dbConn);
				cmd.ExecuteNonQuery();
				return true;

			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}

			return false;
		}

		//***************************************************************************************************************
		public bool DbLoadDetails(Group group)
		{
			List<Detail> loadedDetails = new List<Detail>();
			try
			{
				string stm = string.Format("SELECT * FROM Details Where GroupIdx = {0}", group.GroupIdx);
				var cmd = new SQLiteCommand(stm, m_dbConn);
				SQLiteDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					Detail d = new Detail();
					d.DetailIdx = rdr.GetInt32(0);

					string detailType = rdr.GetString(4);
					DetailType dett;
					if (Enum.TryParse(detailType, true, out dett))
						d.DType = dett;
					else
					{
						d.DType = DetailType.Custom;
						d.CustomDetailText = detailType;
					}

					string startTimeText = rdr.GetString(2);
					string endTimeText = rdr.GetString(3);

					DateTime dt;
					d.StartTimeKnown = DateTime.TryParse(startTimeText, out dt);
					if (d.StartTimeKnown)
						d.StartTime = dt;

					d.EndTimeKnown = DateTime.TryParse(endTimeText, out dt);
					if (d.EndTimeKnown)
						d.EndTime = dt;

					// Load Stories
					string sStm = string.Format("SELECT * FROM Stories Where DetailIdx = {0}", d.DetailIdx);
					SQLiteCommand sCmd = new SQLiteCommand(sStm, m_dbConn);
					SQLiteDataReader sRdr = sCmd.ExecuteReader();
					while (sRdr.Read())
					{
						d.Stories.Add(sRdr.GetString(2));
					}

					loadedDetails.Add(d);
				}

				//OK now put the details where they belong
				rdr.Close();
				foreach (Detail d in loadedDetails)
				{
					stm = string.Format("SELECT * FROM DetailLinks Where DetailIdx = {0}", d.DetailIdx);
					cmd = new SQLiteCommand(stm, m_dbConn);
					rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{
						int memberIdx = rdr.GetInt32(2);
						int relationshpipIdx = rdr.GetInt32(3);
						if (memberIdx != -1)
						{
							Member m = group.GetMember(memberIdx);
							if (m != null)
								m.Details.Add(d);
						}
						else if (relationshpipIdx != -1)
						{
							Relationship r = group.GetRelationship(relationshpipIdx);
							if (r != null)
								r.Details.Add(d);
						}
						else
						{
							group.Details.Add(d);
						}
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}


			return false;
		}


		//***************************************************************************************************************
		//***************************************************************************************************************
		public void DbAddAllRelationships(Group group)
		{
			foreach (Member m in group.Members)
			{
				foreach (Relationship r in m.Relationships)
				{
					if (r.Member1 == m)	// We only add from the Member1
						DbAddRelationship(group, m, r);
				}
			}
		}


		public bool DbAddRelationship(Group group, Member member, Relationship r)
		{
			try
			{
				int member1Idx = member.MemberIdx;
				int member2Idx = r.Member2.MemberIdx;

				string stm = string.Format("INSERT INTO Relationships (GroupIdx, MemberIdx, Kind, RelatedMemberIdx, RelatedMemberKind, StartTime, EndTime) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
					group.GroupIdx,
					member1Idx, r.Member1Kind,
					member2Idx, r.Member2Kind,
					r.StartTimeKnown ? r.StartTime.ToString() : string.Empty,
					r.EndTimeKnown ? r.EndTime.ToString() : string.Empty
					);
				SQLiteCommand cmd = new SQLiteCommand(stm, m_dbConn);
				cmd.ExecuteNonQuery();

				long rowId = m_dbConn.LastInsertRowId;
				stm = string.Format("SELECT RelationshipIdx FROM Relationships Where ROWID='{0}'", rowId);
				cmd = new SQLiteCommand(stm, m_dbConn);
				SQLiteDataReader rdr = cmd.ExecuteReader();

				int relationshipIdx = -1;
				while (rdr.Read())
				{
					relationshipIdx = rdr.GetInt32(0);
				}
				rdr.Close();
				r.RelationshipIdx = relationshipIdx;

				//ME: Need to save relationship detail
				foreach (Detail d in r.Details)
				{
					DbAddDetail(group, r, d);
				}
				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}


		public bool DbLoadRelationships(Group g, Member m)
		{
			try
			{
				// Get MemberIdx for all that belong to this grsoup
				string stm = string.Format("SELECT * FROM Relationships Where GroupIdx = {0} AND MemberIdx = {1}", g.GroupIdx, m.MemberIdx);
				var cmd = new SQLiteCommand(stm, m_dbConn);
				SQLiteDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					Relationship r = new Relationship();
					int member1Idx = rdr.GetInt32(2);
					r.Member1 = g.GetMember(member1Idx);
					r.Member1Kind = rdr.GetString(3);

					int member2Idx = rdr.GetInt32(4);
					r.Member2 = g.GetMember(member2Idx);
					r.Member2Kind = rdr.GetString(5);

					string startTimeText = rdr.GetString(6);
					string endTimeText = rdr.GetString(7);

					DateTime dt;
					r.StartTimeKnown = DateTime.TryParse(startTimeText, out dt);
					if (r.StartTimeKnown)
						r.StartTime = dt;

					r.EndTimeKnown = DateTime.TryParse(endTimeText, out dt);
					if (r.EndTimeKnown)
						r.EndTime = dt;

					m.Relationships.Add(r);
					r.Member2.Relationships.Add(r);
				}
				return true;
			}
			catch (Exception ex)
			{
				DebugMe(ex);
			}
			return false;
		}


		private void DebugMe(Exception ex)
		{
			Console.WriteLine("Gak: " + ex.Message);
		}

	}
}
