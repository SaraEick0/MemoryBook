//***************************************************************************************************************

using System;
using System.Collections.Generic;

namespace Play
{
	//***************************************************************************************************************
	// A Group is the primary container of information of interest.
	// Has a unique Name
	// It represents a scope of interest
	// There can be multiple Groups
	// Contains 0 or more Members

	public class Group
	{
		// *************** Constructors
		public Group(string name)
		{
			Name = name;
			Members = new List<Member>();
			//Relationships = new List<Relationship>();
			Details = new List<Detail>();
		}

		// *************** Variables
		public int GroupIdx { get; set; }               // DB index, unique
		public string Name { get; set; }                // "Mike Eick Family" - Probably don't need list
		public List<Member> Members { get; set; }       // All family members
		public List<Detail> Details { get; set; }
		//public List<Relationship> Relationships { get; set; }

		// *************** Methods
		// *** Adding
		// Members
		public Member AddMember(string memberName)
		{
			Member m = new Member(memberName);
			Members.Add(m);
			return m;
		}

		//// Relationships
		//public Relationship AddRelationShip(Member member1, string member1Kind, Member member2, string member2Kind)
		//{
		//	Relationship r = new Relationship();

		//	r.Member1 = member1;
		//	r.Member1Kind = member1Kind;
		//	r.Member2 = member2;
		//	r.Member2Kind = member2Kind;

		//	this.Relationships.Add(r);
		//	member1.Relationships.Add(r);
		//	member2.Relationships.Add(r);
		//	return r;
		//}

		// Details
		// When Formed, story, history, group pics, etc..
		public Detail AddDetail(DetailType detailType)
		{
			Detail d = new Detail(detailType);
			return d;
		}

		// *** Getting 
		// Members
		public Member GetMember(string commonName)
		{
			foreach (Member m in Members)
			{
				if (m.CommonName == commonName)
					return m;
			}
			return null;
		}

		public Member GetMember(int memberIdx)
		{
			foreach (Member m in Members)
			{
				if (m.MemberIdx == memberIdx)
					return m;
			}
			return null;
		}

		public Relationship GetRelationship(int relationshpipIdx)
		{
			foreach (Member m in Members)
			{
				foreach (Relationship r in m.Relationships)
				{
					if (r.RelationshipIdx == relationshpipIdx)
						return r;
				}
			}
			return null;
		}

	}

	//***************************************************************************************************************
	// A Member represents a person or other entity
	// Has a Name and CommonName
	// Can have Relationships with other Members
	//ME:? Can belong to one or more Groups

	public class Member
	{
		// *************** Constructors
		public Member(string memberName)
		{
			Name = memberName;
			Relationships = new List<Relationship>();
			Details = new List<Detail>();
			OtherGroups = new List<Group>();
		}

		// *************** Variables
		public int MemberIdx { get; set; }
		public string Name { get; set; }						// Members are named
		public string CommonName { get; set; }					// Nickname

		public List<Relationship> Relationships { get; set; }   // Relationships with other Members
		public List<Detail> Details { get; set; }               // Birthday, Deathday, Hired, Left, Bought, Sold ... 
		public List<Group> OtherGroups { get; set; }            // ME: Future, other Groups this member is part of (weak reference)

		// *************** Methods
		// *** Adding
		public bool AddRelationShip(string member1Kind, Member member2, string member2Kind, out Relationship relOut)
		{
			relOut = new Relationship();
			relOut.Member1 = this;
			relOut.Member1Kind = member1Kind;
			relOut.Member2 = member2;
			relOut.Member2Kind = member2Kind;
			this.Relationships.Add(relOut);
			member2.Relationships.Add(relOut);

			return true;
		}

		// Various ways to add details
		public Detail AddDetail(DetailType detailType)
		{
			Detail d = new Detail(detailType);
			Details.Add(d);
			return d;
		}

		public Detail AddDetail_ST(DetailType detailType, string startTimeLabel, DateTime startTime)
		{
			return AddDetail_STET(detailType, startTimeLabel, startTime, null, DateTime.MinValue);
		}

		public Detail AddDetail_ET(DetailType detailType, string endTimeLabel, DateTime endTime)
		{
			return AddDetail_STET(detailType, null, DateTime.MinValue, endTimeLabel, endTime );
		}

		public Detail AddDetail_STET(DetailType detailType, string startTimeLabel, DateTime startTime, string endTimeLabel, DateTime endTime)
		{
			Detail d = AddDetail (detailType);

			if (string.IsNullOrEmpty(startTimeLabel) == false)
			{
				d.StartTimeLabel = startTimeLabel;
				d.StartTimeKnown = true;
				d.StartTime = startTime;
			}

			if (string.IsNullOrEmpty(endTimeLabel) == false)
			{
				d.EndTimeLabel = endTimeLabel;
				d.EndTimeKnown = true;
				d.EndTime = endTime;
			}
			return d;
		}

		// Common details, with an optional story ...
		public Detail AddBirthday(DateTime bDay, string where = null)
		{
			Detail d = AddDetail_ST(DetailType.LifeSpan, "Born", bDay);
			if (string.IsNullOrEmpty(where) == false)
				d.Stories.Add(string.Format("Birthplace is {0}", where));
			return d;
		}

		// *** Getting 
		public Detail GetDetail(DetailType detailType)
		{
			foreach (Detail d in Details)
			{
				if (d.DType == detailType)
					return d;
			}
			return null;
		}

		public Relationship GetRelationship(Member peerMember, string kind)
		{
			foreach (Relationship r in Relationships)
			{
				if ((r.Member2 == peerMember) && (r.Member2Kind == kind))
					return r;
			}
			return null;
		}
	}

	//***************************************************************************************************************
	//***************************************************************************************************************
	// A Relationship  represents a  ... relationship between members
	

	public class Relationship
	{
		// *************** Constructors
		public Relationship()
		{
			StartTimeKnown = false;
			EndTimeKnown = false;
			Details = new List<Detail>();
		}

		// *************** Variables
		public int RelationshipIdx { get; set; }

		public Member Member1 { get; set; }
		public string Member1Kind { get; set; }

		public Member Member2 { get; set; }
		public string Member2Kind { get; set; }

		public bool StartTimeKnown { get; set; }
		public DateTime StartTime { get; set; }
		public string StartTimeLabel { get; set; }

		public bool EndTimeKnown { get; set; }
		public DateTime EndTime { get; set; }
		public string EndTimeLabel { get; set; }

		public List<Detail> Details { get; set; }

		// *************** Methods
		// *** Adding
		public Detail AddDetail(DetailType detailType, string startTimeLabel, DateTime startTime)
		{
			return AddDetail(detailType, startTimeLabel, startTime, null, DateTime.MinValue);
		}

		public Detail AddDetail(DetailType detailType, string startTimeLabel, DateTime startTime, string endTimeLabel, DateTime endTime)
		{
			Detail d = new Detail(detailType);
			Details.Add(d);
			if (string.IsNullOrEmpty(startTimeLabel) == false)
			{
				d.StartTimeLabel = startTimeLabel;
				d.StartTimeKnown = true;
				d.StartTime = startTime;
			}

			if (string.IsNullOrEmpty(endTimeLabel) == false)
			{
				d.EndTimeLabel = endTimeLabel;
				d.EndTimeKnown = true;
				d.EndTime = endTime;
			}
			return d;
		}

		// *** Getting 
		public Detail GetDetail(DetailType detailType)
		{
			foreach (Detail d in Details)
			{
				if (d.DType == detailType)
					return d;
			}
			return null;
		}
	}



	//***************************************************************************************************************
	// A Detail provides additional information about Groups, Members, or Relationships
	public enum DetailType
	{
		None,
		LifeSpan,
		//		Born,Died,
		Event,
		Wedding,
		Divorce,
		Graduated,
		StartJob,
		EndJob,
		Buy,
		Sell,
		Discard,
		Give,
		Other,
		Custom
	}

	public class Detail
	{
		public Detail()
		{
			DType = DetailType.None;
			Pictures = new List<string>();
			Stories = new List<string>();
			Editors = new List<Member>();
			StartTimeKnown = false;
			EndTimeKnown = false;
			CustomDetailText = string.Empty;
		}
		public Detail(DetailType detailType)
		{
			DType = detailType;
			Pictures = new List<string>();
			Stories = new List<string>();
			Editors = new List<Member>();
			StartTimeKnown = false;
			EndTimeKnown = false;
			CustomDetailText = string.Empty;
		}

		public int DetailIdx { get; set; }
		public List<Member> Members { get; set; }

		public DetailType DType { get; set; }
		public string CustomDetailText { get; set; }
		public Member Creator { get; set; }
		public List<Member> Editors { get; set; }

		public bool StartTimeKnown { get; set; }
		public DateTime StartTime { get; set; }
		public string StartTimeLabel { get; set; }

		public bool EndTimeKnown { get; set; }
		public DateTime EndTime { get; set; }
		public string EndTimeLabel { get; set; }

		public List<string> Stories { get; set; }
		public List<string> Pictures { get; set; }


		public string DetailTypeText
		{
			get
			{
				if (DType == DetailType.Custom)
					return CustomDetailText;

				return DType.ToString();
			}
		}
	}
}
