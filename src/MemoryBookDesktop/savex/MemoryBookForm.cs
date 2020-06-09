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
    using Syncfusion.Windows.Forms.Diagram;
    using SfDiagramPlay;

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

        SfDiagram m_sfDiagramForm;
        GroupViewModel m_group;
        Guid m_universeId;

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

        public delegate Task<string> InvokeDelegate(System.Windows.Forms.Label label);

        //********************************************************************************
        private string DisplayMesh(GroupViewModel group)
        {
            bool enableDiag = true;
            bool enableString = false;
            if (enableDiag == true)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    // Running on the UI thread
                    try
                    {
                        m_sfDiagramForm = new SfDiagram();
                        m_group = group;
                        m_sfDiagramForm.AddModelDelegate = AddModelToGraph;
                        m_sfDiagramForm.AddMemberToModelDelegate = AddMemberToModel;
                        m_sfDiagramForm.GetRelationshipsDelegate = GetRelationships;
                        m_sfDiagramForm.AddRelationshipToModelDelegate = AddRelationshipToModelDelegate;

                        m_sfDiagramForm.Show();
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }

            if (enableString == false)
                return "";

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


        //********************************************
        class MemberToNode
        {
            public MemberViewModel member;
            public Node diagramNode;
        };

        //***************************************************************
        void AddModelToGraph()
        {
            List<MemberToNode> memberToNodeList = new List<MemberToNode>();

            List<Node> sfDiagramMembers = new List<Node>();
            List<CombinedRelationshipReadModel> rels = new List<CombinedRelationshipReadModel>();

            // First add members to diagram
            foreach (MemberViewModel m in m_group.Members)
            {
                string mName = m.CommonName;
                Node diagramMember = m_sfDiagramForm.AddNode(mName);
                memberToNodeList.Add(new MemberToNode { member = m, diagramNode = diagramMember });
                m_sfDiagramForm.AddNodeProperty(diagramMember, "MbNode", m);

                sfDiagramMembers.Add(diagramMember);
            }

            // Got members on diagram, now add relationships
            List<CombinedRelationshipReadModel> relList = new List<CombinedRelationshipReadModel>();
            foreach (MemberToNode mton in memberToNodeList)
            {
                MemberViewModel member = mton.member;
                if (member.Relationships != null)
                {
                    foreach (CombinedRelationshipReadModel rel in member.Relationships)
                    {
                        if (relList.Contains(rel))
                            continue;

                        CombinedRelationshipMemberReadModel selfRelationship = rel.RelationshipMembers.First(x => x.MemberId == member.Id);
                        string selfRelationshipTypeCode = selfRelationship.MemberRelationshipTypeCode;
                        CombinedRelationshipMemberReadModel otherRelationship = rel.RelationshipMembers.First(x => x.MemberId != member.Id);
                        string otherRelationshipTypeCode = otherRelationship.MemberRelationshipTypeCode;
                        MemberViewModel otherMember = m_group.Members.First(x => x.Id == otherRelationship.MemberId);

                        MemberToNode otherMton = null;
                        foreach (MemberToNode mton2 in memberToNodeList)
                        {
                            if (mton2.member.Id == otherMember.Id)
                            {
                                otherMton = mton2;
                                break;
                            }
                        }
                        if (otherMton != null)
                        {
                            ConnectorBase lc = m_sfDiagramForm.ConnectMembers(mton.diagramNode, otherMton.diagramNode, selfRelationshipTypeCode, otherRelationshipTypeCode);
                            //ME: Fix this m_sfd.AddConnectorProperty(lc, "MbRelationshup", 6);

                            relList.Add(rel);
                        }

                    }
                }
            }
            m_sfDiagramForm.SetLayout();
        }

        //*******************************************************************
        async Task<int> AddMemberToModel(AddModelData amd)
        {
            try
            {
//                MemberReadModel m = await this.memberManager.CreateMember(m_universeId, amd.Name, amd.Name, amd.Name, amd.Name);
                MemberReadModel m = await this.memberManager.CreateMember(m_universeId, "F", "M", "L", amd.Name);
                m_sfDiagramForm.AddNodeProperty(amd.node, "MbNode", m);
                return 0;
            }
            catch (Exception ex)
            {

            }
            return 1;
        }

        //*******************************************************************
        List<SfDiagranRel> GetRelationships(Node node, Syncfusion.Windows.Forms.Diagram.LabelCollection nodeLabels)
        {
            List<SfDiagranRel> rels = new List<SfDiagranRel>();
            SfDiagranRel sfdRel;

            if (nodeLabels.Count == 0)
                return rels;

            MemberViewModel member = (MemberViewModel)m_sfDiagramForm.GetNodeProperty(node, "MbNode");
            foreach (CombinedRelationshipReadModel rel in member.Relationships)
            {
                CombinedRelationshipMemberReadModel selfRelationship = rel.RelationshipMembers.First(x => x.MemberId == member.Id);
                string selfRelationshipTypeCode = selfRelationship.MemberRelationshipTypeCode;
                CombinedRelationshipMemberReadModel otherRelationship = rel.RelationshipMembers.First(x => x.MemberId != member.Id);
                string otherRelationshipTypeCode = otherRelationship.MemberRelationshipTypeCode;
                MemberViewModel otherMember = m_group.Members.First(x => x.Id == otherRelationship.MemberId);

                sfdRel = new SfDiagranRel();
                sfdRel.RelatedTo = otherMember.CommonName;
                sfdRel.RelationType = otherRelationshipTypeCode;
                rels.Add(sfdRel);
            }
            return rels;
        }

        //*******************************************************************
        public int AddRelationshipToModelDelegate(AddRelInfo ari)
        {
            MemberViewModel member1 = (MemberViewModel) m_sfDiagramForm.GetNodeProperty(ari.Member1, "MbNode");
            MemberViewModel member2 = (MemberViewModel) m_sfDiagramForm.GetNodeProperty(ari.Member2, "MbNode");

            this.relationshipManager.CreateTwoPersonRelationship(member1.MemoryBookUniverseId, member1.Id, member2.Id, ari.RelType1, ari.RelType2, DateTime.Now, null);
            return 0;
        }


        //******************************************************************************************************
        //******************************************************************************************************
        private void button1_Click(object sender, EventArgs e)
        {
            object[] myArray = new object[1];

            myArray[0] = this.TestDataLabel;

            this.TestDataLabel.BeginInvoke(new InvokeDelegate(this.RunTestScenario), myArray);
        }

        //******************************************************************************************************
        private async Task<string> RunTestScenario(System.Windows.Forms.Label label)
        {
            const string UniverseName = "TestUniverse";

            await this.LoadSeedData().ConfigureAwait(false);

            Repository.MemoryBookUniverse.Models.MemoryBookUniverseReadModel universe = await this.memoryBookUniverseManager.GetUniverse(UniverseName).ConfigureAwait(false);

            m_universeId = universe.Id;

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

        //******************************************************************************************************
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

            var mikeDianeRelationshipId = await this.relationshipManager.CreateTwoPersonRelationship(universeId, mike.Id, diane.Id, RelationshipTypeEnum.Husband, RelationshipTypeEnum.Wife, new DateTime(1986, 6, 14), null);

            await this.relationshipManager.CreateTwoPersonRelationship(universeId, mike.Id, lisa.Id, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.relationshipManager.CreateTwoPersonRelationship(universeId, diane.Id, lisa.Id, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1988, 2, 21), null);
            await this.relationshipManager.CreateTwoPersonRelationship(universeId, mike.Id, sara.Id, RelationshipTypeEnum.Father, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);
            await this.relationshipManager.CreateTwoPersonRelationship(universeId, diane.Id, sara.Id, RelationshipTypeEnum.Mother, RelationshipTypeEnum.Daughter, new DateTime(1993, 05, 11), null);

            await this.relationshipManager.CreateTwoPersonRelationship(universeId, lisa.Id, sara.Id, RelationshipTypeEnum.Sister, RelationshipTypeEnum.Sister, new DateTime(1993, 05, 11), null);
            await this.relationshipManager.CreateTwoPersonRelationship(universeId, lisa.Id, sara.Id, RelationshipTypeEnum.Friend, RelationshipTypeEnum.Friend, new DateTime(2011, 05, 11), null);

            await this.relationshipManager.CreateTwoPersonRelationship(universeId, david.Id, sara.Id, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 26), null);
            await this.relationshipManager.CreateTwoPersonRelationship(universeId, ian.Id, lisa.Id, RelationshipTypeEnum.Boyfriend, RelationshipTypeEnum.Girlfriend, new DateTime(2018, 10, 11), null);

            await this.relationshipDetailManager.CreateWedding(mike, mikeDianeRelationshipId, new DateTime(1986, 6, 14), "St. Johns Lutheran Church in New Baltimore, MI")
                .ConfigureAwait(false);

            DateTime stevenWeddingDate = new DateTime(2020, 2, 29);

            await this.memberDetailManager.CreateEvent(mike, allMembers.Select(x => x.Id).ToList(), stevenWeddingDate, stevenWeddingDate, "Steven and Jonathan's Wedding Celebration")
                .ConfigureAwait(false);

            return groupId;
        }

        //******************************************************************************************************
        private async Task DeleteTestData(Guid memoryBookUniverseId)
        {
            await this.memoryBookUniverseManager.DeleteUniverse(memoryBookUniverseId);
        }

    }
}