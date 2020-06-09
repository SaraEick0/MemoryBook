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
    public partial class SfAddMember : Form
    {
        public SfDiagram m_sfMainForm;

        public SfAddMember(SfDiagram sfMainForm)
        {
            m_sfMainForm = sfMainForm;
            InitializeComponent();
        }

        private void OnOkButton(object sender, EventArgs e)
        {
            AddModelData amd = new AddModelData();
            amd.Name = tbCommonName.Text;
            Node newMember = m_sfMainForm.AddNode(amd.Name, string.Empty);
            amd.node = newMember;
            m_sfMainForm.SetLayout();
            m_sfMainForm.AddMemberToModelDelegate(amd);
        }

        private void OnCancelButton(object sender, EventArgs e)
        {

        }
    }
}
