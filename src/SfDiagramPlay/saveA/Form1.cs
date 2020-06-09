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



namespace SfDiagramPlay
{
    public partial class Form1 : Form
    {
        SfDiagram m_sfDiagramForm;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_sfDiagramForm = new SfDiagram();
            m_sfDiagramForm.AddModelDelegate = AddMNodelToGraph;
            m_sfDiagramForm.GetRelationshipsDelegate = GetRelationships;
            m_sfDiagramForm.AddMemberToModelDelegate = AddMemberToModel;
            m_sfDiagramForm.AddRelationshipToModelDelegate = AddRelationshipToModelDelegate;
            m_sfDiagramForm.Show();
        }



        //*******************************************************************
        void AddMNodelToGraph()
        {
            Node Mike = m_sfDiagramForm.AddNode("Mike", "../../Mike.bmp");
            Node Diane = m_sfDiagramForm.AddNode("Diane", "../../Diane.bmp");
            Node Lisa = m_sfDiagramForm.AddNode("Lisa", "../../Lisa.bmp");
            Node Sara = m_sfDiagramForm.AddNode("Sara", "../../Sara.bmp");
            Node Ian = m_sfDiagramForm.AddNode("Ian", "../../Ian.bmp");
            Node David = m_sfDiagramForm.AddNode("David", "../../David.bmp");

            m_sfDiagramForm.AddNodeProperty(David, "MbNode", 3);

            m_sfDiagramForm.SetLayout(); // Spread the nodes out :)

            ConnectorBase connector = m_sfDiagramForm.ConnectMembers(Mike, Diane, "Husband", "Wife");
            m_sfDiagramForm.AddConnectorProperty(connector, "MbRelationshup", 6);

            m_sfDiagramForm.ConnectMembers(Mike, Lisa, "Father", "Daughter");
            m_sfDiagramForm.ConnectMembers(Mike, Sara, "Father", "Daughter");
            m_sfDiagramForm.ConnectMembers(Diane, Lisa, "Mother", "Daughter");
            m_sfDiagramForm.ConnectMembers(Diane, Sara, "Mother", "Daughter");
            m_sfDiagramForm.ConnectMembers(Lisa, Sara, "Sestra", "Sestra");
            m_sfDiagramForm.ConnectMembers(Lisa, Ian, "Homie", "Homie");
            m_sfDiagramForm.ConnectMembers(Sara, David, "Homie", "Homie");

            m_sfDiagramForm.SetLayout();
            // UGH  m_diagram.Model.LineRouter.RoutingMode = RoutingMode.Automatic;
        }

        //*******************************************************************
        List<SfDiagranRel> GetRelationships(Node node, Syncfusion.Windows.Forms.Diagram.LabelCollection nodeLabels)
        {
            List<SfDiagranRel> rels = new List<SfDiagranRel>();
            SfDiagranRel rel;

            if (nodeLabels.Count == 0)
                return rels;

            string nodeName = nodeLabels[0].Text;

            switch (nodeName)
            {
                case "Mike":
                    rel = new SfDiagranRel();
                    rel.RelatedTo = "Diane";
                    rel.RelationType = "Wife";
                    rels.Add(rel);

                    rel = new SfDiagranRel();
                    rel.RelatedTo = "Lisa";
                    rel.RelationType = "Daughter";
                    rels.Add(rel);

                    rel = new SfDiagranRel();
                    rel.RelatedTo = "Sara";
                    rel.RelationType = "Daughter";
                    rels.Add(rel);
                    break;
            }
            return rels;
        }

        //*******************************************************************
        int AddMemberToModel(AddModelData amd)
        {
            return 0;
        }

        public int AddRelationshipToModelDelegate(AddRelInfo ari)
        {
            return 0;
        }

    }
}
