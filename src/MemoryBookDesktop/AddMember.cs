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

namespace SfDiagramPlay
{
    public partial class AddMember : Form
    {
        public SfDiagram m_sfMainForm;

        public AddMember(SfDiagram sfMainForm)
        {
            m_sfMainForm = sfMainForm;
            InitializeComponent();
        }

        private void OnOkButton(object sender, EventArgs e)
        {
            SfdMemberData amd = new SfdMemberData();
            amd.FirstName = tbFirstName.Text;
            amd.MiddleName = tbMiddleName.Text;
            amd.LastName = tbLastName.Text;
            amd.CommonName = tbCommonName.Text;
            Node newMember = m_sfMainForm.AddMemberNode(amd, string.Empty);
            amd.MemberNode = newMember;
            m_sfMainForm.DrawDiagram();
            this.Close();
        }

        private void OnCancelButton(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
