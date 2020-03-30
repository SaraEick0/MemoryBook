//***************************************************************************************************************

using MemoryBook.Business.Member.Models;
using NodaTime;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

//***************************************************************************************************************
namespace Play
{
    using System.Collections.Generic;
    using MemoryBook.Business.Detail.Models;
    using MemoryBook.Business.DetailType;
    using MemoryBook.Business.Group.Models;
    using MemoryBook.Business.Relationship.Models;
    using MemoryBook.Business.RelationshipType;
    using MemoryBook.Repository;
    using MemoryBook.Repository.Detail.Managers;
    using MemoryBook.Repository.Group.Extensions;
    using MemoryBook.Repository.Group.Managers;
    using MemoryBook.Repository.Member.Extensions;
    using MemoryBook.Repository.Member.Managers;
    using MemoryBook.Repository.MemoryBookUniverse.Managers;
    using MemoryBook.Repository.Relationship.Extensions;
    using MemoryBook.Repository.Relationship.Managers;

    public partial class Form1 : Form
    {
        private readonly IMemoryBookUniverseManager memoryBookUniverseManager;
        private readonly ISeedDataManager seedDataManager;
        private readonly IMemberManager memberManager;
        private readonly IRelationshipManager relationshipManager;
        private readonly IGroupManager groupManager;
        private readonly IDetailManager detailManager;

        public Form1(
            IMemoryBookUniverseManager memoryBookUniverseManager,
            ISeedDataManager seedDataManager,
            IMemberManager memberManager,
            IRelationshipManager relationshipManager,
            IGroupManager groupManager,
            IDetailManager detailManager)
        {
            InitializeComponent();

            this.memoryBookUniverseManager = memoryBookUniverseManager ?? throw new ArgumentNullException(nameof(memoryBookUniverseManager));
            this.seedDataManager = seedDataManager ?? throw new ArgumentNullException(nameof(seedDataManager));
            this.memberManager = memberManager ?? throw new ArgumentNullException(nameof(memberManager));
            this.relationshipManager = relationshipManager ?? throw new ArgumentNullException(nameof(relationshipManager));
            this.groupManager = groupManager ?? throw new ArgumentNullException(nameof(groupManager));
            this.detailManager = detailManager ?? throw new ArgumentNullException(nameof(detailManager));
        }

        //********************************************************************************
        private void DisplayMesh(GroupReadModel group)
        {
            Console.WriteLine(string.Format("\n******Group {0} ******", group.Name));
            Console.WriteLine(string.Format("\n* Members"));
            foreach (MemberReadModel m in group.Members)
            {
                string mName = m.CommonName;
                DetailReadModel mBirthday = m.GetBirthday();

                Console.WriteLine(string.Format("\nGroup Member: {0}", mName));

                Console.WriteLine(string.Format("  Details"));
                foreach (DetailReadModel d in m.Details)
                {
                    Console.WriteLine(string.Format("    {0}", d.DetailTypeText));
                    if (d.StartTime.HasValue)
                        Console.WriteLine(string.Format("      {0} {1}", d.DetailType.DetailStartText, d.StartTime.ToString()));

                    if (d.EndTime.HasValue)
                        Console.WriteLine(string.Format("      {0} {1}", d.DetailType.DetailEndText, d.EndTime.ToString()));

                    Console.WriteLine(string.Format("        {0}", d.Story));
                }


                Console.WriteLine(string.Format("\n  Relationships"));
                foreach (RelationshipReadModel rel in m.Relationships)
                {
                    var self = rel.Memberships.FirstOrDefault(x => x.MemberId == m.Id).Member;
                    var other = rel.Memberships.FirstOrDefault(x => x.MemberId != m.Id);

                    string relName = other.Member.CommonName;
                    DetailReadModel relBirthday = other.Member.GetBirthday();

                    if (mBirthday.StartTime <= relBirthday.StartTime)
                    {
                        string timeYounger = Analytics.GetTimeBetween(mBirthday.StartTime.Value, relBirthday.StartTime.Value,
                            PeriodUnits.YearMonthDay, true);
                        Console.WriteLine(string.Format("    {0} is {1} to {2}, who is {3} younger than {4}",
                            m.CommonName, other.MemberRelationshipType.Code, relName, timeYounger, mName));
                    }
                    else
                    {
                        string timeOlder = Analytics.GetTimeBetween(relBirthday.StartTime.Value, mBirthday.StartTime.Value,
                            PeriodUnits.YearMonthDay, true);
                        Console.WriteLine(string.Format("    {0} is {1} to {2}, who is {3} older than {4}",
                            m.CommonName, other.MemberRelationshipType.Code, relName, timeOlder, mName));
                    }

                    foreach (DetailReadModel d in rel.Details)
                    {
                        Console.WriteLine(string.Format("      {0}", d.DetailTypeText));
                        if (d.StartTime.HasValue)
                            Console.WriteLine(string.Format("        {0} : {1}", d.DetailType.DetailStartText,
                                d.StartTime.ToString()));
                        ;

                        if (d.EndTime.HasValue)
                            Console.WriteLine(string.Format("        {0}", d.DetailType.DetailEndText, d.EndTime.ToString()));
                        ;
                        
                        Console.WriteLine(string.Format("        {0}", d.Story));
                    }
                }
            }

            Console.WriteLine(string.Format("\nFun Facts!"));

            var mike = group.GetMember("Mike");
            RelationshipReadModel rSpouse = mike.GetRelationship(RelationshipTypeEnum.Wife);
            DetailReadModel wedding = rSpouse.GetDetail(DetailTypeEnum.Wedding);

            MemberReadModel lisa = group.GetMember("Lisa");
            DetailReadModel lBirthday = lisa.GetDetail(DetailTypeEnum.LifeSpan);
            string howLong =
                Analytics.GetTimeBetween(wedding.StartTime.Value, lBirthday.StartTime.Value, PeriodUnits.YearMonthDay, true);
            Console.WriteLine(string.Format("  Lisa was born {0} after her parents were married", howLong));

            MemberReadModel sara = group.GetMember("Sara");
            DetailReadModel sBirthday = sara.GetDetail(DetailTypeEnum.LifeSpan);
            howLong = Analytics.GetTimeBetween(wedding.StartTime.Value, sBirthday.StartTime.Value, PeriodUnits.YearMonthDay, true);
            Console.WriteLine(string.Format("  Sara was born {0} after her parents were married", howLong));

            howLong = Analytics.GetTimeBetween(lBirthday.StartTime.Value, sBirthday.StartTime.Value, PeriodUnits.YearMonthDay,
                true);
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

        private void UpdateDisplay(GroupReadModel group)
        {
            string puText = puOptions[periodUnitsIndex].ToString();
            Console.WriteLine("\n******************");
            foreach (MemberReadModel member in group.Members)
            {
                string mName = member.CommonName;
                DetailReadModel mBirthday = member.GetBirthday();
                string mAge = Analytics.GetTimeSinceStart(mBirthday, puOptions[periodUnitsIndex], true);
                Console.WriteLine(string.Format("{0} is {1}  ({2})", mName, mAge, puText));
            }

            if (puOptions[periodUnitsIndex] == PeriodUnits.DateAndTime)
            {
                periodUnitsIndex = 1;
            }
            else
            {
                ++periodUnitsIndex;
            }
        }

        /// <summary>
        /// Need to update this, obviously this is dumb.
        /// </summary>
        /// <returns></returns>
        private async Task LoadSeedData()
        {
            await this.seedDataManager.LoadSeedData();
        }

        private async Task<GroupReadModel> LoadData(Guid universeId)
        {
            var group = await this.groupManager.CreateGroup(universeId, "TheMikeEicks", "The Mike Eicks", "The family of Diane and Michael Eick");

            var mike = await this.memberManager.CreateMember(universeId, "Michael", "David", "Eick", "Mike");
            var diane = await this.memberManager.CreateMember(universeId, "Diane", "Gail", "Eick", "Diane");
            var lisa = await this.memberManager.CreateMember(universeId, "Lisa", "Jean", "Eick", "Lisa");
            var sara = await this.memberManager.CreateMember(universeId, "Sara", "Ann", "Eick", "Sara");
            var ian = await this.memberManager.CreateMember(universeId, "Ian", "Gilmore", "Mitchel", "Ian");
            var david = await this.memberManager.CreateMember(universeId, "David", "Randolph", "Ulmer", "David");

            var allMembers = new List<MemberReadModel>
            {
                mike, diane, lisa, sara, ian, david
            };

            await this.groupManager.AddMembersToGroup(universeId, group, allMembers.ToList())
                .ConfigureAwait(false);

            await this.detailManager.CreateBirthday(sara, mike, new DateTime(1956, 12, 1), "Mt. Clemens, MI");
            await this.detailManager.CreateBirthday(sara, diane, new DateTime(1957, 1, 25), "Rochester, MI");
            await this.detailManager.CreateBirthday(sara, lisa, new DateTime(1988, 2, 21), "Rochester, MI");
            await this.detailManager.CreateBirthday(sara, sara, new DateTime(1993, 5, 11), "Rochester, MI");
            await this.detailManager.CreateBirthday(sara, ian, new DateTime(1993, 1, 19), "Reno, NV");
            await this.detailManager.CreateBirthday(sara, david, new DateTime(1991, 5, 25), "Grosse Pointe, MI");

            await this.relationshipManager.CreateRelationship(mike, diane, RelationshipTypeEnum.Husband, RelationshipTypeEnum.Wife, new DateTime(1986, 6, 14), null);

            await this.relationshipManager.CreateRelationship(mike, lisa, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.relationshipManager.CreateRelationship(diane, lisa, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.relationshipManager.CreateRelationship(mike, sara, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);
            await this.relationshipManager.CreateRelationship(diane, sara, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);

            await this.relationshipManager.CreateRelationship(lisa, sara, RelationshipTypeEnum.Sister, RelationshipTypeEnum.Sister, new DateTime(1993, 05, 11), null);
            await this.relationshipManager.CreateRelationship(lisa, sara, RelationshipTypeEnum.Sister, RelationshipTypeEnum.Friend, new DateTime(2011, 05, 11), null);

            await this.relationshipManager.CreateRelationship(david, sara, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 26), null);
            await this.relationshipManager.CreateRelationship(ian, lisa, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 11), null);

            await this.detailManager.CreateWedding(mike, mike, diane, new DateTime(1986, 6, 14), "St. Johns Lutheran Church in New Baltimore, MI")
                .ConfigureAwait(false);

            var stevenWeddingDate = new DateTime(2020, 2, 29);
            
            await this.detailManager.CreateEvent(mike, allMembers.ToList(), stevenWeddingDate, stevenWeddingDate, "Steven and Jonathan's Wedding Celebration")
                .ConfigureAwait(false);

            return group;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await this.LoadSeedData().ConfigureAwait(false);

            var universeId = await this.memoryBookUniverseManager.GetOrCreateUniverse("TestUniverse").ConfigureAwait(false);

            // Load some canned test mesh data via code
            GroupReadModel group = await this.LoadData(universeId).ConfigureAwait(false);

            // Display some selected mesh data
            DisplayMesh(group);
        }
    }
}