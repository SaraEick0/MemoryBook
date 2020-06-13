using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Imports the Diagram control’s namespaces
using Syncfusion.Windows.Forms.Diagram.Controls;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing.Drawing2D;
using System.IO;

using SfdMember = Syncfusion.Windows.Forms.Diagram.Node;

namespace SfDiagramPlay
{
    public partial class Form1 : Form, ISfHelper
    {
        SfDiagram m_sfDiagramForm;
        SfdMember Mike;
        SfdMember Diane;
        SfdMember Lisa;
        SfdMember Sara;
        SfdMember Ian;
        SfdMember David;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_sfDiagramForm = new SfDiagram();
            m_sfDiagramForm.SfHelper = this;

            m_sfDiagramForm.Show();
        }


        //*******************************************************************
        //*******************************************************************
        //*******************************************************************
        public void SfhLoadMbData()
        {
            SfdMemberData amd = new SfdMemberData();
            amd.CommonName = "Mike";
            Mike = m_sfDiagramForm.AddMemberNode(amd, "../../Mike.bmp");
            amd.CommonName = "Diane";
            Diane = m_sfDiagramForm.AddMemberNode(amd, "../../Diane.bmp");
            amd.CommonName = "Lisa";
            Lisa = m_sfDiagramForm.AddMemberNode(amd, "../../Lisa.bmp");
            amd.CommonName = "Sara";
            Sara = m_sfDiagramForm.AddMemberNode(amd, "../../Sara.bmp");
            amd.CommonName = "Ian";
            Ian = m_sfDiagramForm.AddMemberNode(amd, "../../Ian.bmp");
            amd.CommonName = "David";
            David = m_sfDiagramForm.AddMemberNode(amd, "../../David.bmp");

            m_sfDiagramForm.AddMemberNodeProperty(David, "MbNode", 3);

            m_sfDiagramForm.DrawDiagram(); // Spread the nodes out :)

            ConnectorBase connector = m_sfDiagramForm.AddRelationshop(Mike, Diane, "Husband", "Wife");
            m_sfDiagramForm.AddRelationshopProperty(connector, "MbRelationshup", 6);

            m_sfDiagramForm.AddRelationshop(Mike, Lisa, "Father", "Daughter");
            m_sfDiagramForm.AddRelationshop(Mike, Sara, "Father", "Daughter");
            m_sfDiagramForm.AddRelationshop(Diane, Lisa, "Mother", "Daughter");
            m_sfDiagramForm.AddRelationshop(Diane, Sara, "Mother", "Daughter");
            m_sfDiagramForm.AddRelationshop(Lisa, Sara, "Sestra", "Sestra");
            m_sfDiagramForm.AddRelationshop(Lisa, Ian, "Homie", "Homie");
            m_sfDiagramForm.AddRelationshop(Sara, David, "Homie", "Homie");

            m_sfDiagramForm.DrawDiagram();
            // UGH  m_diagram.Model.LineRouter.RoutingMode = RoutingMode.Automatic;
        }

        //*******************************************************************
        public List<SfdRelationshipData> SfhGetRelationships(SfdMember node, Syncfusion.Windows.Forms.Diagram.LabelCollection nodeLabels)
        {
            List<SfdRelationshipData> rels = new List<SfdRelationshipData>();
            SfdRelationshipData rel;

            if (nodeLabels.Count == 0)
                return rels;

            string nodeName = nodeLabels[0].Text;

            switch (nodeName)
            {
                case "Mike":
                    rel = new SfdRelationshipData();
                    rel.RelatedMemberNode = Diane;
                    rel.RelatedTo = "Diane";
                    rel.RelationType = "Wife";
                    rels.Add(rel);

                    rel = new SfdRelationshipData();
                    rel.RelatedMemberNode = Lisa;
                    rel.RelatedTo = "Lisa";
                    rel.RelationType = "Daughter";
                    rels.Add(rel);

                    rel = new SfdRelationshipData();
                    rel.RelatedMemberNode = Sara;
                    rel.RelatedTo = "Sara";
                    rel.RelationType = "Daughter";
                    rels.Add(rel);
                    break;

                case "Diane":
                    rel = new SfdRelationshipData();
                    rel.RelatedMemberNode = Mike;
                    rel.RelatedTo = "Mike";
                    rel.RelationType = "Husband";
                    rels.Add(rel);

                    rel = new SfdRelationshipData();
                    rel.RelatedMemberNode = Lisa;
                    rel.RelatedTo = "Lisa";
                    rel.RelationType = "Daughter";
                    rels.Add(rel);

                    rel = new SfdRelationshipData();
                    rel.RelatedMemberNode = Sara;
                    rel.RelatedTo = "Sara";
                    rel.RelationType = "Daughter";
                    rels.Add(rel);
                    break;
            }
            return rels;
        }

        //*******************************************************************
        public async Task<int> SfAddMemberToModel(SfdMemberData amd)
        {
            return 0;
        }

        //*******************************************************************
        public int SfhAddRelationship(SfdAddRelInfo ari)
        {
            return 0;
        }

        //*******************************************************************
        public bool SfhGetMemberInfo(SfdMemberData amd)
        {
            switch (amd.MemberNode.Name)
            {
                case "Mike":
                    amd.FirstName = "Michael";
                    amd.MiddleName = "David";
                    amd.LastName = "Eick";
                    amd.CommonName = "Mike";

                    amd.Birthday = new DateTime(1956, 12, 1);
                    amd.BirthdayKnown = true;

                    amd.Deathday = new DateTime(2056, 12, 1);
                    amd.DeathdayKnown = true;
                    break;

                case "Diane":
                    amd.FirstName = "Diane";
                    amd.MiddleName = "Gail";
                    amd.LastName = "Eick";
                    amd.CommonName = "Diane";

                    amd.Birthday = new DateTime(1957, 1, 25);
                    amd.BirthdayKnown = true;
                    break;

                case "Lisa":
                    amd.FirstName = "Lisa";
                    amd.MiddleName = "Jean";
                    amd.LastName = "Eick";
                    amd.CommonName = "Lisa";

                    amd.Birthday = new DateTime(1988, 2, 21);
                    amd.BirthdayKnown = true;
                    break;

                case "Sara":
                    amd.FirstName = "Sara";
                    amd.MiddleName = "Ann";
                    amd.LastName = "Eick";
                    amd.CommonName = "Sara";

                    amd.Birthday = new DateTime(1993, 5, 11);
                    amd.BirthdayKnown = true;
                    break;

                default:
                    amd.BirthdayKnown = false;
                    return false;
            }
            return false;
        }
        //*******************************************************************
        //*******************************************************************
        //*******************************************************************

    }
}
