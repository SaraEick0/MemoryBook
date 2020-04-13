namespace MemoryBook.Desktop
{
    using NodaTime;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.Text;
    using Business.DataCoordinators.Managers;
    using Business.Detail.Models;
    using Business.Group.Models;
    using Business.Group.Providers;
    using Business.Member.Extensions;
    using Business.Member.Models;
    using Business.Relationship.Extensions;
    using Business.Relationship.Managers;
    using Business.Relationship.Models;
    using Common.Extensions;
    using Repository.DetailType;
    using Repository.Member.Models;
    using Repository.RelationshipType;
    using MemoryBook.Business.Group.Extensions;
    using MemoryBook.Business.Member.Managers;
    using MemoryBook.Business.MemoryBookUniverse.Managers;
    using MemoryBook.Business.SeedData;
    using MemoryBook.Business.Detail.Managers;

    public partial class MemoryBookForm : Form
    {
        private readonly IMemoryBookUniverseManager memoryBookUniverseManager;
        private readonly ISeedDataManager seedDataManager;
        private readonly IMemberManager memberManager;
        private readonly IRelationshipManager relationshipManager;
        private readonly IGroupProvider groupManager;
        private readonly IViewCoordinator groupViewCoordinator;
        private readonly IMemberDetailManager memberDetailManager;
        private readonly IRelationshipDetailManager relationshipDetailManager;

        public MemoryBookForm(
            IMemoryBookUniverseManager memoryBookUniverseManager,
            ISeedDataManager seedDataManager,
            IMemberManager memberManager,
            IRelationshipManager relationshipManager,
            IGroupProvider groupManager,
            IViewCoordinator groupViewCoordinator,
            IMemberDetailManager memberDetailManager,
            IRelationshipDetailManager relationshipDetailManager)
        {
            Contract.RequiresNotNull(memoryBookUniverseManager, nameof(memoryBookUniverseManager));
            Contract.RequiresNotNull(seedDataManager, nameof(seedDataManager));
            Contract.RequiresNotNull(memberManager, nameof(memberManager));
            Contract.RequiresNotNull(relationshipManager, nameof(relationshipManager));
            Contract.RequiresNotNull(groupManager, nameof(groupManager));
            Contract.RequiresNotNull(groupViewCoordinator, nameof(groupViewCoordinator));
            Contract.RequiresNotNull(memberDetailManager, nameof(memberDetailManager));
            Contract.RequiresNotNull(relationshipDetailManager, nameof(relationshipDetailManager));

            this.memoryBookUniverseManager = memoryBookUniverseManager;
            this.seedDataManager = seedDataManager;
            this.memberManager = memberManager;
            this.relationshipManager = relationshipManager;
            this.groupManager = groupManager;
            this.groupViewCoordinator = groupViewCoordinator;
            this.memberDetailManager = memberDetailManager;
            this.relationshipDetailManager = relationshipDetailManager;

            this.InitializeComponent();
        }

        public delegate Task<string> InvokeDelegate(Label label);

        //********************************************************************************
        private string DisplayMesh(GroupViewModel group)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\n******Group {0} ******", group.GroupName));
            sb.AppendLine(string.Format("\n* Members"));
            foreach (var member in group.Members)
            {
                string mName = member.CommonName;
                var memberBirthday = member.GetBirthday();

                sb.AppendLine(string.Format("\nGroup Member: {0}", mName));

                if (member.Details != null)
                {
                    sb.AppendLine(string.Format("  Details"));
                    foreach (DetailViewModel memberDetail in member.Details)
                    {
                        sb.AppendLine(string.Format("    {0}", memberDetail.DetailTypeText));
                        if (memberDetail.StartDate.HasValue)
                            sb.AppendLine(string.Format("      {0} {1}", memberDetail.DetailTypeStartText,
                                memberDetail.StartDate.ToString()));

                        if (memberDetail.EndDate.HasValue)
                            sb.AppendLine(string.Format("      {0} {1}", memberDetail.DetailTypeEndText,
                                memberDetail.EndDate.ToString()));

                        sb.AppendLine(string.Format("        {0}", memberDetail.Story));
                    }
                }

                if (member.Relationships != null)
                {
                    sb.AppendLine("\n  Relationships");

                    foreach (CombinedRelationshipReadModel rel in member.Relationships)
                    {
                        var self = rel.RelationshipMembers.First(x => x.MemberId == member.Id);
                        var selfRelationshipTypeCode = self.MemberRelationshipTypeCode;
                        var other = rel.RelationshipMembers.First(x => x.MemberId != member.Id);
                        var otherMember = group.Members.First(x => x.Id == other.MemberId);

                        string relName = otherMember.CommonName;

                        DetailViewModel relBirthday = otherMember.GetBirthday();

                        if (memberBirthday.StartDate <= relBirthday.StartDate)
                        {
                            string timeYounger = Analytics.GetTimeBetween(memberBirthday.StartDate.Value,
                                relBirthday.StartDate.Value,
                                PeriodUnits.YearMonthDay, true);
                            sb.AppendLine(string.Format("    {0} is {1} to {2}, who is {3} younger than {4}",
                                member.CommonName, selfRelationshipTypeCode, relName, timeYounger, mName));
                        }
                        else
                        {
                            string timeOlder = Analytics.GetTimeBetween(relBirthday.StartDate.Value,
                                memberBirthday.StartDate.Value,
                                PeriodUnits.YearMonthDay, true);
                            sb.AppendLine(string.Format("    {0} is {1} to {2}, who is {3} older than {4}",
                                member.CommonName, selfRelationshipTypeCode, relName, timeOlder, mName));
                        }

                        if (rel.Details != null)
                        {
                            foreach (var relationshipDetail in rel.Details)
                            {
                                sb.AppendLine(string.Format("      {0}", relationshipDetail.DetailTypeText));
                                if (relationshipDetail.StartDate.HasValue)
                                {
                                    sb.AppendLine(string.Format("        {0} : {1}", relationshipDetail.DetailTypeStartText, relationshipDetail.StartDate.ToString()));
                                }

                                if (relationshipDetail.EndDate.HasValue)
                                {
                                    sb.AppendLine(string.Format("        {0}", relationshipDetail.DetailTypeEndText, relationshipDetail.EndDate.ToString()));
                                }

                                sb.AppendLine(string.Format("        {0}", relationshipDetail.Story));
                            }
                        }
                    }
                }
            }

            sb.AppendLine(string.Format("\nFun Facts!"));

            MemberViewModel mike = group.GetMember("Mike");
            CombinedRelationshipReadModel rSpouse = mike.GetRelationship(RelationshipTypeEnum.Wife);
            DetailViewModel wedding = rSpouse.GetDetail(DetailTypeEnum.Wedding);

            MemberViewModel lisa = group.GetMember("Lisa");
            DetailViewModel lBirthday = lisa.GetDetail(DetailTypeEnum.LifeSpan);
            string howLong =
                Analytics.GetTimeBetween(wedding.StartDate.Value, lBirthday.StartDate.Value, PeriodUnits.YearMonthDay, true);
            sb.AppendLine(string.Format("  Lisa was born {0} after her parents were married", howLong));

            MemberViewModel sara = group.GetMember("Sara");
            DetailViewModel sBirthday = sara.GetDetail(DetailTypeEnum.LifeSpan);
            howLong = Analytics.GetTimeBetween(wedding.StartDate.Value, sBirthday.StartDate.Value, PeriodUnits.YearMonthDay, true);
            sb.AppendLine(string.Format("  Sara was born {0} after her parents were married", howLong));

            howLong = Analytics.GetTimeBetween(lBirthday.StartDate.Value, sBirthday.StartDate.Value, PeriodUnits.YearMonthDay,
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

            Repository.MemoryBookUniverse.Models.MemoryBookUniverseReadModel universe = await this.memoryBookUniverseManager.GetUniverse(UniverseName).ConfigureAwait(false);

            if (universe != null)
            {
                await this.DeleteTestData(universe.Id);
            }

            Guid universeId = await this.memoryBookUniverseManager.CreateUniverse(UniverseName).ConfigureAwait(false);

            // Load some canned test mesh data via code
            Guid groupId = await this.LoadData(universeId).ConfigureAwait(false);

            var groupView = await this.groupViewCoordinator.GetGroupViewModel(universeId, groupId).ConfigureAwait(false);

            // Display some selected mesh data
            string meshRead = DisplayMesh(groupView);

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

        private async Task<Guid> LoadData(Guid universeId)
        {
            Guid groupId = await this.groupManager.CreateGroup(universeId, "TheMikeEicks", "The Mike Eicks", "The family of Diane and Michael Eick");

            MemberReadModel mike = await this.memberManager.CreateMember(universeId, "Michael", "David", "Eick", "Mike");
            MemberReadModel diane = await this.memberManager.CreateMember(universeId, "Diane", "Gail", "Eick", "Diane");
            MemberReadModel lisa = await this.memberManager.CreateMember(universeId, "Lisa", "Jean", "Eick", "Lisa");
            MemberReadModel sara = await this.memberManager.CreateMember(universeId, "Sara", "Ann", "Eick", "Sara");
            MemberReadModel ian = await this.memberManager.CreateMember(universeId, "Ian", "Gilmore", "Mitchel", "Ian");
            MemberReadModel david = await this.memberManager.CreateMember(universeId, "David", "Randolph", "Ulmer", "David");

            List<MemberReadModel> allMembers = new List<MemberReadModel>
            {
                mike, diane, lisa, sara, ian, david
            };

            await this.groupManager.AddMembersToGroup(universeId, groupId, allMembers.ToList())
                .ConfigureAwait(false);

            await this.memberDetailManager.CreateBirthday(universeId, sara, mike.Id, new DateTime(1956, 12, 1), "Mt. Clemens, MI");
            await this.memberDetailManager.CreateBirthday(universeId, sara, diane.Id, new DateTime(1957, 1, 25), "Rochester, MI");
            await this.memberDetailManager.CreateBirthday(universeId, sara, lisa.Id, new DateTime(1988, 2, 21), "Rochester, MI");
            await this.memberDetailManager.CreateBirthday(universeId, sara, sara.Id, new DateTime(1993, 5, 11), "Rochester, MI");
            await this.memberDetailManager.CreateBirthday(universeId, sara, ian.Id, new DateTime(1993, 1, 19), "Reno, NV");
            await this.memberDetailManager.CreateBirthday(universeId, sara, david.Id, new DateTime(1991, 5, 25), "Grosse Pointe, MI");

            var mikeDianeRelationshipId = await this.relationshipManager.CreateTwoPersonRelationship(mike, diane, RelationshipTypeEnum.Husband, RelationshipTypeEnum.Wife, new DateTime(1986, 6, 14), null);

            await this.relationshipManager.CreateTwoPersonRelationship( mike, lisa, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.relationshipManager.CreateTwoPersonRelationship( diane, lisa, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.relationshipManager.CreateTwoPersonRelationship( mike, sara, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);
            await this.relationshipManager.CreateTwoPersonRelationship( diane, sara, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);

            await this.relationshipManager.CreateTwoPersonRelationship( lisa, sara, RelationshipTypeEnum.Sister, RelationshipTypeEnum.Sister, new DateTime(1993, 05, 11), null);
            await this.relationshipManager.CreateTwoPersonRelationship( lisa, sara, RelationshipTypeEnum.Sister, RelationshipTypeEnum.Friend, new DateTime(2011, 05, 11), null);

            await this.relationshipManager.CreateTwoPersonRelationship( david, sara, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 26), null);
            await this.relationshipManager.CreateTwoPersonRelationship( ian, lisa, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 11), null);

            await this.relationshipDetailManager.CreateWedding(mike, mikeDianeRelationshipId, new DateTime(1986, 6, 14), "St. Johns Lutheran Church in New Baltimore, MI")
                .ConfigureAwait(false);

            DateTime stevenWeddingDate = new DateTime(2020, 2, 29);

            await this.memberDetailManager.CreateEvent(mike, allMembers.Select(x => x.Id).ToList(), stevenWeddingDate, stevenWeddingDate, "Steven and Jonathan's Wedding Celebration")
                .ConfigureAwait(false);

            return groupId;
        }

        private async Task DeleteTestData(Guid memoryBookUniverseId)
        {
            await this.memoryBookUniverseManager.DeleteUniverse(memoryBookUniverseId);
        }

    }
}