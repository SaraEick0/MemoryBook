namespace MemoryBook.Desktop
{
    using NodaTime;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.Text;
    using Business.Detail.Models;
    using Business.DetailType;
    using Business.Group.Models;
    using Business.Member.Models;
    using Business.Relationship.Models;
    using Business.RelationshipType;
    using Common.Extensions;
    using Repository.Detail.Managers;
    using Repository.Group.Extensions;
    using Repository.Group.Managers;
    using Repository.Member.Extensions;
    using Repository.Member.Managers;
    using Repository.MemoryBookUniverse.Managers;
    using Repository.SeedData;

    public partial class MemoryBook : Form
    {
        private readonly IMemoryBookUniverseManager memoryBookUniverseManager;
        private readonly ISeedDataManager seedDataManager;
        private readonly IMemberManager memberManager;
        private readonly IGroupManager groupManager;
        private readonly IMemberDetailManager memberDetailManager;
        private readonly IRelationshipDetailManager relationshipDetailManager;

        public MemoryBook(
            IMemoryBookUniverseManager memoryBookUniverseManager,
            ISeedDataManager seedDataManager,
            IMemberManager memberManager,
            IGroupManager groupManager,
            IMemberDetailManager memberDetailManager,
            IRelationshipDetailManager relationshipDetailManager)
        {
            Contract.RequiresNotNull(memoryBookUniverseManager, nameof(memoryBookUniverseManager));
            Contract.RequiresNotNull(seedDataManager, nameof(seedDataManager));
            Contract.RequiresNotNull(memberManager, nameof(memberManager));
            Contract.RequiresNotNull(groupManager, nameof(groupManager));
            Contract.RequiresNotNull(memberDetailManager, nameof(memberDetailManager));
            Contract.RequiresNotNull(relationshipDetailManager, nameof(relationshipDetailManager));

            this.memoryBookUniverseManager = memoryBookUniverseManager;
            this.seedDataManager = seedDataManager;
            this.memberManager = memberManager;
            this.groupManager = groupManager;
            this.memberDetailManager = memberDetailManager;
            this.relationshipDetailManager = relationshipDetailManager;

            this.InitializeComponent();
        }

        public delegate Task<string> InvokeDelegate(Label label);

        //********************************************************************************
        private string DisplayMesh(GroupReadModel group)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\n******Group {0} ******", group.Name));
            sb.AppendLine(string.Format("\n* Members"));
            foreach (MemberReadModel m in group.Members)
            {
                string mName = m.CommonName;
                DetailReadModel mBirthday = m.GetBirthday();

                sb.AppendLine(string.Format("\nGroup Member: {0}", mName));

                if (m.Details != null)
                {
                    sb.AppendLine(string.Format("  Details"));
                    foreach (DetailReadModel d in m.Details)
                    {
                        sb.AppendLine(string.Format("    {0}", d.DetailTypeText));
                        if (d.StartTime.HasValue)
                            sb.AppendLine(string.Format("      {0} {1}", d.DetailType.DetailStartText,
                                d.StartTime.ToString()));

                        if (d.EndTime.HasValue)
                            sb.AppendLine(string.Format("      {0} {1}", d.DetailType.DetailEndText,
                                d.EndTime.ToString()));

                        sb.AppendLine(string.Format("        {0}", d.Story));
                    }
                }

                if (m.Relationships != null)
                {
                    sb.AppendLine(string.Format("\n  Relationships"));

                    foreach (RelationshipReadModel rel in m.Relationships)
                    {
                        var selfRelationship = rel.Memberships.First(x => x.MemberId == m.Id);
                        var otherRelationship = rel.Memberships.First(x => x.MemberId != m.Id);

                        var other = group.Members.FirstOrDefault(x => x.Id == otherRelationship.MemberId);

                        string relName = other.CommonName;

                        DetailReadModel relBirthday = other.GetBirthday();

                        if (mBirthday.StartTime <= relBirthday.StartTime)
                        {
                            string timeYounger = Analytics.GetTimeBetween(mBirthday.StartTime.Value,
                                relBirthday.StartTime.Value,
                                PeriodUnits.YearMonthDay, true);
                            sb.AppendLine(string.Format("    {0} is {1} to {2}, who is {3} younger than {4}",
                                m.CommonName, selfRelationship.MemberRelationshipType.Code, relName, timeYounger, mName));
                        }
                        else
                        {
                            string timeOlder = Analytics.GetTimeBetween(relBirthday.StartTime.Value,
                                mBirthday.StartTime.Value,
                                PeriodUnits.YearMonthDay, true);
                            sb.AppendLine(string.Format("    {0} is {1} to {2}, who is {3} older than {4}",
                                m.CommonName, selfRelationship.MemberRelationshipType.Code, relName, timeOlder, mName));
                        }

                        if (rel.Details != null)
                        {
                            foreach (DetailReadModel d in rel.Details)
                            {
                                sb.AppendLine(string.Format("      {0}", d.DetailTypeText));
                                if (d.StartTime.HasValue)
                                {
                                    sb.AppendLine(string.Format("        {0} : {1}", d.DetailType.DetailStartText, d.StartTime.ToString()));
                                }

                                if (d.EndTime.HasValue)
                                {
                                    sb.AppendLine(string.Format("        {0}", d.DetailType.DetailEndText, d.EndTime.ToString()));
                                }

                                sb.AppendLine(string.Format("        {0}", d.Story));
                            }
                        }
                    }
                }
            }

            sb.AppendLine(string.Format("\nFun Facts!"));

            var mike = group.GetMember("Mike");
            RelationshipReadModel rSpouse = mike.GetRelationship(RelationshipTypeEnum.Wife);
            DetailReadModel wedding = rSpouse.GetDetail(DetailTypeEnum.Wedding);

            MemberReadModel lisa = group.GetMember("Lisa");
            DetailReadModel lBirthday = lisa.GetDetail(DetailTypeEnum.LifeSpan);
            string howLong =
                Analytics.GetTimeBetween(wedding.StartTime.Value, lBirthday.StartTime.Value, PeriodUnits.YearMonthDay, true);
            sb.AppendLine(string.Format("  Lisa was born {0} after her parents were married", howLong));

            MemberReadModel sara = group.GetMember("Sara");
            DetailReadModel sBirthday = sara.GetDetail(DetailTypeEnum.LifeSpan);
            howLong = Analytics.GetTimeBetween(wedding.StartTime.Value, sBirthday.StartTime.Value, PeriodUnits.YearMonthDay, true);
            sb.AppendLine(string.Format("  Sara was born {0} after her parents were married", howLong));

            howLong = Analytics.GetTimeBetween(lBirthday.StartTime.Value, sBirthday.StartTime.Value, PeriodUnits.YearMonthDay,
                true);
            sb.AppendLine(string.Format("  Sara was born {0} after Lisa was born", howLong));

            return sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object[] myArray = new object[1];

            myArray[0] = this.TestDataLabel;

            this.TestDataLabel.BeginInvoke(new InvokeDelegate(this.RunTestScenario), myArray);
        }

        private async Task<string> RunTestScenario(Label label)
        {
            const string UniverseName = "TestUniverse";

            await this.LoadSeedData().ConfigureAwait(false);

            var universe = await this.memoryBookUniverseManager.GetUniverse(UniverseName).ConfigureAwait(false);

            if (universe != null)
            {
                await this.DeleteTestData(universe.Id);
            }

            var universeId = await this.memoryBookUniverseManager.CreateUniverse(UniverseName).ConfigureAwait(false);

            // Load some canned test mesh data via code
            GroupReadModel group = await this.LoadData(universeId).ConfigureAwait(false);

            // Display some selected mesh data
            string meshRead = DisplayMesh(group);

            await this.DeleteTestData(universeId);

            label.Text = meshRead;
            return meshRead;
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

            await this.memberDetailManager.CreateBirthday(sara, mike, new DateTime(1956, 12, 1), "Mt. Clemens, MI");
            await this.memberDetailManager.CreateBirthday(sara, diane, new DateTime(1957, 1, 25), "Rochester, MI");
            await this.memberDetailManager.CreateBirthday(sara, lisa, new DateTime(1988, 2, 21), "Rochester, MI");
            await this.memberDetailManager.CreateBirthday(sara, sara, new DateTime(1993, 5, 11), "Rochester, MI");
            await this.memberDetailManager.CreateBirthday(sara, ian, new DateTime(1993, 1, 19), "Reno, NV");
            await this.memberDetailManager.CreateBirthday(sara, david, new DateTime(1991, 5, 25), "Grosse Pointe, MI");

            await this.memberManager.CreateRelationship(universeId, mike, diane, RelationshipTypeEnum.Husband, RelationshipTypeEnum.Wife, new DateTime(1986, 6, 14), null);

            await this.memberManager.CreateRelationship(universeId, mike, lisa, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.memberManager.CreateRelationship(universeId, diane, lisa, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.memberManager.CreateRelationship(universeId, mike, sara, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);
            await this.memberManager.CreateRelationship(universeId, diane, sara, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);

            await this.memberManager.CreateRelationship(universeId, lisa, sara, RelationshipTypeEnum.Sister, RelationshipTypeEnum.Sister, new DateTime(1993, 05, 11), null);
            await this.memberManager.CreateRelationship(universeId, lisa, sara, RelationshipTypeEnum.Sister, RelationshipTypeEnum.Friend, new DateTime(2011, 05, 11), null);

            await this.memberManager.CreateRelationship(universeId, david, sara, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 26), null);
            await this.memberManager.CreateRelationship(universeId, ian, lisa, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 11), null);

            var mikeDianeMarriage = mike.GetRelationship(RelationshipTypeEnum.Husband);

            await this.relationshipDetailManager.CreateWedding(mike, mikeDianeMarriage, new DateTime(1986, 6, 14), "St. Johns Lutheran Church in New Baltimore, MI")
                .ConfigureAwait(false);

            var stevenWeddingDate = new DateTime(2020, 2, 29);

            await this.memberDetailManager.CreateEvent(mike, allMembers.ToList(), stevenWeddingDate, stevenWeddingDate, "Steven and Jonathan's Wedding Celebration")
                .ConfigureAwait(false);

            return group;
        }

        private async Task DeleteTestData(Guid memoryBookUniverseId)
        {
            await this.memoryBookUniverseManager.DeleteUniverse(memoryBookUniverseId);
        }

    }
}