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

namespace SfDiagramPlay
{
    public partial class Form1 : Form
    {
        Diagram m_diagram = null;
        int m_layoutSel = 0;

        public Form1()
        {
            InitializeComponent();
            this.Shown += OnPlayLoad;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQyODAyQDMxMzgyZTMxMmUzMFpydHYvRHNPN0JTN2JtMnF0VHp4Uk0wMFJuS1BIa014d2FXZUhZclY5aEU9");

            AddDiagram();
            m_diagram.Location = new Point(20, 5);


        }

        private void OnPlayLoad(object sender, EventArgs e)
        {
        }

        void AddDiagram()
        {
            if (m_diagram != null)
                m_diagram.Dispose();

            //Create an instance
            m_diagram = new Diagram();

            //Enable scroll bars
            m_diagram.HScroll = true;
            m_diagram.VScroll = true;

            //Sizing the diagram
            m_diagram.Size = new Size(1200, 1200);

            //Positioning the diagram
            m_diagram.Location = new Point(20, 5);

            //Create a model
            Model model = new Model();

            //Add the model to the Diagram control
            m_diagram.Model = model;

            //Add the Diagram control to Diagram Form
            this.Controls.Add(m_diagram);

            //Enable diagram rulers
            m_diagram.ShowRulers = true;

            //Enable diagram rulers
            m_diagram.ShowRulers = true;

            m_diagram.Model.LineRoutingEnabled = true; //not work so far
            m_diagram.EventSink.NodeClick += EventSink_NodeClick; ;

        }

        //*******************************************************************
        void SetGraphNoImages(int layoutSel)
        {
            AddDiagram();

            Ellipse Mike = AddMember("Mike");
            m_diagram.Model.AppendChild(Mike);

            Ellipse Diane = AddMember("Diane");
            m_diagram.Model.AppendChild(Diane);

            Ellipse Lisa = AddMember("Lisa");
            m_diagram.Model.AppendChild(Lisa);

            Ellipse Sara = AddMember("Sara");
            m_diagram.Model.AppendChild(Sara);

            Ellipse Ian = AddMember("Ian");
            m_diagram.Model.AppendChild(Ian);

            Ellipse David = AddMember("David");
            m_diagram.Model.AppendChild(David);

            LineConnector lc;

            lc = ConnectMembers(Mike, Diane, "Husband/Wife");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Mike, Lisa, "Father/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Mike, Sara, "Father/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Diane, Lisa, "Mother/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Diane, Sara, "Mother/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Lisa, Sara, "Sestras");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Lisa, Ian, "Homie");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Sara, David, "Homie");
            m_diagram.Model.AppendChild(lc);

            SetLayout(layoutSel);
        }


        void SetGraphImages (int layoutSel)
        {
            AddDiagram();

            BitmapNode Mike = AddMember("Mike", "../../Mike.bmp");
            m_diagram.Model.AppendChild(Mike);

            BitmapNode Diane = AddMember("Diane", "../../Diane.bmp");
            m_diagram.Model.AppendChild(Diane);

            BitmapNode Lisa = AddMember("Lisa", "../../Lisa.bmp");
            m_diagram.Model.AppendChild(Lisa);

            BitmapNode Sara = AddMember("Sara", "../../Sara.bmp");
            m_diagram.Model.AppendChild(Sara);

            BitmapNode Ian = AddMember("Ian", "../../Ian.bmp");
            m_diagram.Model.AppendChild(Ian);

            BitmapNode David = AddMember("David", "../../David.bmp");
            m_diagram.Model.AppendChild(David);

            LineConnector lc;
            
            lc = ConnectMembers(Mike, Diane, "Husband/Wife");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Mike, Lisa, "Father/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Mike, Sara, "Father/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Diane, Lisa, "Mother/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Diane, Sara, "Mother/Daughter");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Lisa, Sara, "Sestras");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Lisa, Ian, "Homie");
            m_diagram.Model.AppendChild(lc);

            lc = ConnectMembers(Sara, David, "Homie");
            m_diagram.Model.AppendChild(lc);

            SetLayout(layoutSel);
        }

        private void EventSink_NodeClick(NodeMouseEventArgs evtArgs)
        {
            Syncfusion.Windows.Forms.Diagram.LabelCollection labs = null;
            Type t = evtArgs.Node.GetType();
            if (t.Equals(typeof(Ellipse)))
            {
                Ellipse node = (Ellipse)evtArgs.Node;
                labs = node.Labels;
            }
            else if (t.Equals(typeof(BitmapNode)))
            {
                BitmapNode node = (BitmapNode)evtArgs.Node;
                labs = node.Labels;
            }
            else if (t.Equals(typeof(LineConnector)))
            {
                LineConnector lc = (LineConnector)evtArgs.Node;
                labs = lc.Labels;
            }

            if (labs != null)
            {
                foreach (Syncfusion.Windows.Forms.Diagram.Label l in labs)
                {
                    MessageBox.Show(l.Text);
                }
            }
        }


        //*********************************************************************************************************
        Ellipse AddMember (string memberName)
        {
            
            //Create a circle node
            Syncfusion.Windows.Forms.Diagram.Ellipse el
                = new Syncfusion.Windows.Forms.Diagram.Ellipse(60, 60, 120, 120);

            el.TreatAsObstacle = true;

            //Style the circle node
            el.FillStyle.Type = FillStyleType.LinearGradient;
            el.FillStyle.Color = Color.FromArgb(128, 0, 0);
            el.FillStyle.ForeColor = Color.FromArgb(225, 0, 0);

            //el.ShadowStyle.Visible = true;

            //Border style
            el.LineStyle.LineColor = Color.RosyBrown;
            el.LineStyle.LineWidth = 2.0f;
            el.LineStyle.LineJoin = LineJoin.Miter;

            //Add a label to the rectangular node
            Syncfusion.Windows.Forms.Diagram.Label label = new Syncfusion.Windows.Forms.Diagram.Label();
            label.Text = memberName;
            label.FontStyle.Family = "Arial";
            label.FontColorStyle.Color = Color.White;
            el.Labels.Add(label);

            return el;

        }

        //******************************************************
        BitmapNode AddMember(string memberName, string bitmapFile)
        {

            try
            {


                Bitmap bm = new Bitmap(bitmapFile);
                Syncfusion.Windows.Forms.Diagram.BitmapNode el = new Syncfusion.Windows.Forms.Diagram.BitmapNode(bm);

                el.TreatAsObstacle = true;

                //el.ShadowStyle.Visible = true;

                //Border style
                el.LineStyle.LineColor = Color.RosyBrown;
                el.LineStyle.LineWidth = 2.0f;
                el.LineStyle.LineJoin = LineJoin.Miter;

                el.PinPoint = new System.Drawing.PointF(80, 80);
                el.Size = new System.Drawing.SizeF(100, 100);

                //Add a label to the rectangular node
#if false // Need to get label away from center
                Syncfusion.Windows.Forms.Diagram.Label label = new Syncfusion.Windows.Forms.Diagram.Label();
                label.Text = memberName;
                label.FontStyle.Family = "Arial";
                label.FontColorStyle.Color = Color.White;
                el.Labels.Add(label);
#endif
                return el;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        //******************************************************
        LineConnector ConnectMembers(Node m1, Node m2, string relText)
        {
            Syncfusion.Windows.Forms.Diagram.LineConnector lc = new Syncfusion.Windows.Forms.Diagram.LineConnector(new System.Drawing.PointF(10, 200), new System.Drawing.PointF(300, 250));
            m1.CentralPort.TryConnect(lc.TailEndPoint);
            m2.CentralPort.TryConnect(lc.HeadEndPoint);
            lc.LineStyle.LineColor = Color.MidnightBlue;

            lc.HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
            lc.HeadDecorator.FillStyle.Color = Color.MidnightBlue;
            lc.HeadDecorator.Size = new SizeF(10, 5);

            lc.TailDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
            lc.TailDecorator.FillStyle.Color = Color.MidnightBlue;
            lc.TailDecorator.Size = new SizeF(10, 5);

            //Add a label to the rectangular node
            Syncfusion.Windows.Forms.Diagram.Label label;
            if (string.IsNullOrEmpty(relText) == false)
            {
                label = new Syncfusion.Windows.Forms.Diagram.Label();
                label.Text = relText;
                label.FontStyle.Family = "Arial";
                label.FontColorStyle.Color = Color.Black;
                label.Orientation = LabelOrientation.Horizontal;

                // Sets to align label horizontally relative to given offset 
#if false // needs work
                label.HorizontalAlignment = StringAlignment.Near;
                label.VerticalAlignment = StringAlignment.Near;
                label.OffsetX = 0;
                label.OffsetY = (float)0.5;

                OrthogonalLineSegment seg = new OrthogonalLineSegment(m1.CentralPort, m2.CentralPort)
                OrthogonalLineSegment[] segs = { new OrthogonalLineSegment() }
                lc.LineSegments.Add(OrthogonalLineSegment)
#endif

                lc.Labels.Add(label);
            }


            lc.LineRoutingEnabled = true;
            return lc;
        }

        //*******************************************************************
        void SetLayout (int layoutSel)
        {
            layoutSel = 1;

            if (layoutSel == 0)
            {
                tbLayout.Text = "SymmetricLayoutManager";
                SymmetricLayoutManager symmetricLayout = new SymmetricLayoutManager(m_diagram.Model, 500);
                symmetricLayout.SpringFactor = 0.442;
                symmetricLayout.SpringLength = 200;
                symmetricLayout.MaxIteraction = 500;
                m_diagram.LayoutManager = symmetricLayout;
                m_diagram.LayoutManager.UpdateLayout(null);
            }

            if (layoutSel == 1)
            {
                tbLayout.Text = "TableLayoutManager";
                TableLayoutManager tlLayout = new TableLayoutManager(m_diagram.Model, 7, 7);
                tlLayout.VerticalSpacing = 100;
                tlLayout.HorizontalSpacing = 100;
                tlLayout.CellSizeMode = CellSizeMode.EqualToMaxNode;
                tlLayout.Orientation = Orientation.Horizontal;
                tlLayout.MaxSize = new SizeF(500, 600);
                m_diagram.LayoutManager = tlLayout;
                m_diagram.LayoutManager.UpdateLayout(null);
            }

            if (layoutSel == 2)
            {
                tbLayout.Text = "DirectedTreeLayoutManager";
                DirectedTreeLayoutManager directedLayout = new DirectedTreeLayoutManager(m_diagram.Model, 0, 20, 20);
                m_diagram.LayoutManager = directedLayout;
                m_diagram.LayoutManager.UpdateLayout(null);
            }

            if (layoutSel == 3)
            {
                tbLayout.Text = "RadialTreeLayoutManager"; 
                RadialTreeLayoutManager radialLayout = new RadialTreeLayoutManager(m_diagram.Model, 0, 20, 20);
                m_diagram.LayoutManager = radialLayout;
                m_diagram.LayoutManager.UpdateLayout(null);
            }

            if (layoutSel == 4)
            {
                tbLayout.Text = "HierarchicLayoutManager"; 
                HierarchicLayoutManager hierarchyLayout = new HierarchicLayoutManager(m_diagram.Model, 0, 10, 20);
                m_diagram.LayoutManager = hierarchyLayout;
                m_diagram.LayoutManager.UpdateLayout(null);
            }
            

            if (layoutSel == 5)
            {
                tbLayout.Text = "RadialTreeLayoutManager"; 
                RadialTreeLayoutManager dtlm = new RadialTreeLayoutManager(m_diagram.Model, 0, 20, 20);
                dtlm.PreferredLayout += new PreferredLayoutEventHandler(dtlm_PreferredLayout);
                m_diagram.LayoutManager = dtlm;
                m_diagram.LayoutManager.UpdateLayout(null);
            }

            if (layoutSel == 6)
            {
                tbLayout.Text = "SubgraphTreeLayoutManager"; 
                SubgraphTreeLayoutManager st = new SubgraphTreeLayoutManager(m_diagram.Model, 0, 20, 20);
                st.SubgraphPreferredLayout += new SubgraphPreferredLayoutEventHandler(st_SubgraphPreferredLayout);
                m_diagram.LayoutManager = st;
                m_diagram.LayoutManager.UpdateLayout(null);
            }

            if (layoutSel == 7)
            {
                tbLayout.Text = "OrgChartLayoutManager"; 
                OrgChartLayoutManager manager = new OrgChartLayoutManager(m_diagram.Model, RotateDirection.TopToBottom, 20, 50);
                m_diagram.LayoutManager = manager;
                m_diagram.LayoutManager.UpdateLayout(null);
            }
        }

        private void dtlm_PreferredLayout(object sender, PreferredLayoutEventArgs evtArgs)
        {
            if (evtArgs.IsGraphUnderLayout)
            {
                evtArgs.ResizeGraphNodes = false;
                evtArgs.Location = new PointF(150, 150);
                evtArgs.Size = new SizeF(100, 100);
            }
        }

        private void st_SubgraphPreferredLayout(object sender, SubgraphPreferredLayoutEventArgs evtArgs)
        {
            evtArgs.ResizeSubgraphNodes = false;
            evtArgs.RotationDegree = 0;
        }


        //****************************
        private void DrawWithImages_Click(object sender, EventArgs e)
        {
            SetGraphImages(m_layoutSel++);
            if (m_layoutSel >= 2)
                m_layoutSel = 0;
        }

        private void DrawWithoutImages_Click(object sender, EventArgs e)
        {
            SetGraphNoImages(m_layoutSel++);
            if (m_layoutSel >= 2)
                m_layoutSel = 0;
        }


        private void Save_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string m_fileName = this.saveFileDialog.FileName;
                m_diagram.SaveBinary(m_fileName);
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string m_fileName = this.openFileDialog.FileName;
                this.m_diagram.LoadBinary(m_fileName);
                this.m_diagram.Refresh();
            }
        }


    }
}
