using Syncfusion.SfDiagram.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram.Controls;
using Syncfusion.Windows.Forms.Diagram;
using Node = Syncfusion.Windows.Forms.Diagram.Node;

namespace SfDiagramPlay
{
    //ME: Stole from MB repository relationshiptype
    public enum RelationshipTypeEnum
    {
        Wife,
        Husband,
        Partner,
        Boyfriend,
        Girlfriend,
        Friend,
        Aunt,
        Uncle,
        Brother,
        Sister,
        Mother,
        Father,
        Daughter,
        Son,
        Grandmother,
        Grandfather,
        Cousin
    }

    public partial class AddRelationship : Form
    {
        SfDiagram m_sfMainForm;
        string m_startMember;
        string m_endMember;

        public AddRelationship(SfDiagram sfMainForm, string startMember, string endMember)
        {
            m_startMember = startMember;
            m_endMember = endMember;
            m_sfMainForm = sfMainForm;

            InitializeComponent();
        }

        private void AddRelationship_Load(object sender, EventArgs e)
        {
            if (m_sfMainForm.NodeList.Count < 1)
                return;

            int focusIdx1 = 0;
            int focusIdx2 = 0;
            foreach (Node n in m_sfMainForm.NodeList)
            {
                comboMember1.Items.Add(n.Name);
                comboMember2.Items.Add(n.Name);

                if (n.Name == m_startMember)
                    focusIdx1 = comboMember1.Items.Count - 1;

                if (n.Name == m_endMember)
                    focusIdx2 = comboMember2.Items.Count - 1;
            }

            comboMember1.SelectedIndex = focusIdx1;
            comboMember2.SelectedIndex = focusIdx2;

            foreach (string name in Enum.GetNames(typeof(RelationshipTypeEnum)))
            {
                comboRelType1.Items.Add(name);
                comboRelType2.Items.Add(name);
            }
            comboRelType1.SelectedIndex = 0;
            comboRelType2.SelectedIndex = 1;
        }

        //***************************
        private void OnOk(object sender, EventArgs e)
        {
            AddRelInfo ari = new AddRelInfo();
#if false
            IEnumerable<Node> v = m_sfMainForm.NodeList.Where(m => Name == (string)comboMember1.SelectedItem);
            string name = (string) comboMember1.SelectedItem;
            var v2 = m_sfMainForm.NodeList.FirstOrDefault(a => Name == name);
            ari.Member1 = v.FirstOrDefault();
            ari.Member1 = m_sfMainForm.NodeList.Find(m => Name == (string)comboMember1.SelectedItem);
            ari.Member2 = m_sfMainForm.NodeList.Find(m => Name == (string)comboMember2.SelectedItem);
#endif
            foreach (Node n in m_sfMainForm.NodeList)
            {
                if (n.Name == (string)comboMember1.SelectedItem)
                    ari.Member1 = n;

                if (n.Name == (string)comboMember2.SelectedItem)
                    ari.Member2 = n;
            }
            ari.RelType1 = (RelationshipTypeEnum) Enum.Parse(typeof(RelationshipTypeEnum), (String)comboRelType1.SelectedItem);
            ari.RelType2 = (RelationshipTypeEnum)Enum.Parse(typeof(RelationshipTypeEnum), (String)comboRelType2.SelectedItem);
            m_sfMainForm.AddRel(ari);
            this.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class AddRelInfo
    {
        public Node Member1;
        public Node Member2;
        public RelationshipTypeEnum RelType1;
        public RelationshipTypeEnum RelType2;
    }

}
