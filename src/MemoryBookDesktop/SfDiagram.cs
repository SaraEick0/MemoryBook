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
using NodaTime;

using SfdMember = Syncfusion.Windows.Forms.Diagram.Node;

namespace SfDiagramPlay
{
    enum LayoutEnum
    {
        Table,
        Symmetric,
        DirectedTree,
        RadialTree,
        Hierarchic,
        SubgraphTree,
        OrgChart,
    }

    enum ConnectorEnum
    {
        Line,
        DirectedLine,
        Orthogonal,
        OrgLine,
        Polyline,
        Spline,
        Bezier,
    }

    public interface ISfHelper
    {
        // This method is to load all MemoryBook Members, Relationships onto the SF Diagram for display purposes
        void SfhLoadMbData();

        // These methods are to return information about Members and Relationships for display purposes
        bool SfGetMemberInfo(SfdMemberData amd);
        List<SfdRelationshipData> SfhGetRelationships(SfdMember node, Syncfusion.Windows.Forms.Diagram.LabelCollection nodeLabels);

        // These methods are to add add elements to the MemoryBook model
        Task<int> SfAddMemberToModel(SfdMemberData amd);
        int SfhAddRelationship(AddRelInfo ari);
    }


    public partial class SfDiagram : Form
    {
        public ISfHelper SfHelper;

        Diagram m_diagram = null;
        LayoutEnum m_layoutSel = LayoutEnum.Table;
        ConnectorEnum m_connectorSel = ConnectorEnum.Line;
        bool m_UseGraphicNode = false;
        public List<Node> NodeList = new List<Node>();

        SfdMemberData MemberData = null;
        List<SfdRelationshipData> Relationships;

        public SfDiagram()
        {
            InitializeComponent();
            this.Shown += OnFormLoad;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQyODAyQDMxMzgyZTMxMmUzMFpydHYvRHNPN0JTN2JtMnF0VHp4Uk0wMFJuS1BIa014d2FXZUhZclY5aEU9");
        }


        private void OnFormLoad(object sender, EventArgs e)
        {
            // Set up some combo boxes
            foreach (var item in Enum.GetNames(typeof(LayoutEnum)))
                cbLayout.Items.Add(item.ToString());
            cbLayout.SelectedIndex = 0;

            foreach (var item in Enum.GetNames(typeof(ConnectorEnum)))
                cbConnector.Items.Add(item.ToString());
            cbConnector.SelectedIndex = 0;

            // Add the SF diagram to the form
            AddDiagram();

            // Tell the user to add the model to the diagram
            SfHelper.SfhLoadMbData();
        }

        //*********************************************************************************************************
        //*********************************************************************************************************
        // Various minor control actions

        private void OnLayoutChanged(object sender, EventArgs e)
        {
            m_layoutSel = (LayoutEnum)Enum.Parse(typeof(LayoutEnum), (string) cbLayout.SelectedItem);
        }

        private void OnConnectorChanged(object sender, EventArgs e)
        {
            m_connectorSel = (ConnectorEnum)Enum.Parse(typeof(ConnectorEnum), (string)cbConnector.SelectedItem);
        }

        private void OnSave_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string m_fileName = this.saveFileDialog.FileName;
                m_diagram.SaveBinary(m_fileName);
            }
        }

        private void OnLoad_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string m_fileName = this.openFileDialog.FileName;
                this.m_diagram.LoadBinary(m_fileName);
                this.m_diagram.Refresh();
            }
        }

        private void OnDraw_Click(object sender, EventArgs e)
        {
            m_UseGraphicNode = checkBoxUseImages.Checked;
            AddDiagram();
            SfHelper.SfhLoadMbData(); 
        }

        //*********************************************************************************************************
        //*********************************************************************************************************
        // Mouse Click on Diagram
        public Node m_lastClickedNode = null;
        public Node m_prevClickedNode = null;

        //*********************************************************************************************************
        void OnDiagram_MouseClick(object sender, MouseEventArgs e)
        {
            INode clickedNode = m_diagram.Controller.GetNodeUnderMouse(e.Location);
            if (clickedNode == null)
                return;

            m_prevClickedNode = m_lastClickedNode;
            m_lastClickedNode = (Node) clickedNode;

            Syncfusion.Windows.Forms.Diagram.LabelCollection labs = null;
            SfdMemberData PeerMemberData;
            bool memberClicked = false;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Type t = clickedNode.GetType();
                if (t.Equals(typeof(Ellipse)))
                {
                    memberClicked = true;
                    Ellipse node = (Ellipse)clickedNode;
                    labs = node.Labels;

                    MemberData = new SfdMemberData();
                    MemberData.MemberNode = node;
                }
                else if (t.Equals(typeof(BitmapNode)))
                {
                    memberClicked = true;
                    BitmapNode node = (BitmapNode)clickedNode;
                    labs = node.Labels;
                    MemberData = new SfdMemberData();
                    MemberData.MemberNode = node;
                }
                else if (t.Equals(typeof(LineConnector)))
                {
                    LineConnector lc = (LineConnector)clickedNode;
                    labs = lc.Labels;
                }

                if (memberClicked)
                {
                    SfHelper.SfGetMemberInfo(MemberData);
                    Relationships = SfHelper.SfhGetRelationships(MemberData.MemberNode, labs);
                    dgvRelationships.Rows.Clear();
                    foreach (SfdRelationshipData rel in Relationships)
                    {
                        PeerMemberData = new SfdMemberData();
                        PeerMemberData.MemberNode = rel.RelatedMemberNode;
                        SfHelper.SfGetMemberInfo(PeerMemberData);
                        string ageDiff;
                        if (MemberData.Birthday < PeerMemberData.Birthday)
                        {
                            ageDiff = Analytics.GetTimeBetween(MemberData.Birthday, PeerMemberData.Birthday, PeriodUnits.YearMonthDay, true);
                            ageDiff = ageDiff + " Younger";
                        }
                        else
                        {
                            ageDiff = Analytics.GetTimeBetween(PeerMemberData.Birthday, MemberData.Birthday, PeriodUnits.YearMonthDay, true);
                            ageDiff = ageDiff + " Older";
                        }

                        dgvRelationships.Rows.Add(rel.RelatedTo, rel.RelationType, ageDiff);
                    }

                    if (labs != null)
                    {
                        foreach (Syncfusion.Windows.Forms.Diagram.Label l in labs)
                        {
                            tbMemberName.Text = l.Text;
                        }
                    }
                }
                timerScreenRefresh.Enabled = true;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Type t = clickedNode.GetType();
                if (t.Equals(typeof(Ellipse)))
                {
                    Ellipse node = (Ellipse)clickedNode;
                    labs = node.Labels;
                    string s = string.Empty;
                    if (labs.Count > 0)
                        s = labs[0].Text;

                    OnDiagram_RightClick(true,s);
                }
                else if (t.Equals(typeof(BitmapNode)))
                {
                    BitmapNode node = (BitmapNode)clickedNode;
                    labs = node.Labels;
                    string s = string.Empty;
                    if (labs.Count > 0)
                        s = labs[0].Text;

                    OnDiagram_RightClick(true, s);
                }
                else if (t.Equals(typeof(LineConnector)))
                {
                    LineConnector lc = (LineConnector)clickedNode;
                    labs = lc.Labels;
                    string s = string.Empty;
                    if (labs.Count > 0)
                        s = labs[0].Text;

                    OnDiagram_RightClick(false, s);
                }

                if (labs != null)
                {
                    foreach (Syncfusion.Windows.Forms.Diagram.Label l in labs)
                    {
                    }
                }
            }
            m_diagram.Controller.SelectionList.Clear();
        }

        //*********************************************************************************************************
        public void OnDiagram_RightClick(bool isNode, string nodeName)
        {
        }



    //*********************************************************************************************************
    //*********************************************************************************************************
    //*********************************************************************************************************
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
            Size sz = DiagramPanel.Size;
            m_diagram.Size = sz; // new Size(1200, 1200);

            //Positioning the diagram
            Point location = DiagramPanel.Location;
            m_diagram.Location = location; // new Point(20, 20);

            //Enable diagram rulers
            m_diagram.View.Grid.GridStyle = GridStyle.Line;
            m_diagram.View.Grid.Color = Color.Transparent;

            //Enable diagram rulers
            m_diagram.ShowRulers = false;


            //m_diagram.EventSink.NodeClick += EventSink_NodeClick;
            m_diagram.MouseClick += OnDiagram_MouseClick;

            m_diagram.DefaultContextMenuEnabled = false;

            //Create a model
            Model model = new Model();
            m_diagram.Model = model;
            m_diagram.Model.LineRoutingEnabled = true; //not work so far
            m_diagram.Model.LineBridgingEnabled = true;
            DiagramPanel.Controls.Add(m_diagram);
        }


        //*********************************************************************************************************
        public SfdMember AddNode (SfdMemberData amd, string bitmapFile = "")
        {
            try
            {
                if ((m_UseGraphicNode == false) || string.IsNullOrEmpty(bitmapFile) || (File.Exists (bitmapFile) == false) )
                {
                    //Create a circle node
                    Syncfusion.Windows.Forms.Diagram.Ellipse el
                                    = new Syncfusion.Windows.Forms.Diagram.Ellipse(60, 60, 120, 120);

                    el.Name = amd.CommonName;
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
                    label.Text = amd.CommonName;
                    label.FontStyle.Family = "Arial";
                    label.FontColorStyle.Color = Color.White;
                    el.Labels.Add(label);

                    m_diagram.Model.AppendChild(el);
                    NodeList.Add(el);
                    return el;
                }
                else
                {
                    Bitmap bm = new Bitmap(bitmapFile);
                    Syncfusion.Windows.Forms.Diagram.BitmapNode el = new Syncfusion.Windows.Forms.Diagram.BitmapNode(bm);

                    el.Name = amd.CommonName;
                    el.TreatAsObstacle = true;
                    el.EnableCentralPort = true;

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
#else
                    Syncfusion.Windows.Forms.Diagram.Label label = new Syncfusion.Windows.Forms.Diagram.Label();
                    label.Text = amd.CommonName;
                    label.FontStyle.Family = "Arial";
                    label.FontColorStyle.Color = Color.Transparent;
                    el.Labels.Add(label);
#endif
                    m_diagram.Model.AppendChild(el);
                    NodeList.Add(el);
                    return el;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        //*****************************************************************************************
        //******************************************************
        public void AddNodeProperty(SfdMember node, string propertyName, object obj)
        {
            node.PropertyBag.Add(propertyName, obj);
        }

        //*****************************************************************************************
        public object GetNodeProperty(SfdMember node, string propertyName)
        {
            object o = null;
            node.PropertyBag.TryGetValue(propertyName, out o);
            return o;
        }

        //******************************************************
        //******************************************************

        public ConnectorBase ConnectMembers(SfdMember m1, SfdMember m2, string m1RelText, string m2RelText)
        {
            ConnectorBase c = null;

            //return ConnectMembersTest(m1, m2, m1RelText, m2RelText);
            bool combineRelText = false;

            switch (m_connectorSel)
            {
                case ConnectorEnum.Line:
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

                    lc.LineRoutingEnabled = true;
                    lc.TreatAsObstacle = true;

                    c = lc;
                    break;

                case ConnectorEnum.DirectedLine:
                    return null;
                    break;

                case ConnectorEnum.Orthogonal:
                    //Create an orthogonal connector
                    OrthogonalConnector oc = new OrthogonalConnector(m1.PinPoint, m2.PinPoint);

                    m1.CentralPort.TryConnect(oc.TailEndPoint); //process is tail node
                    m2.CentralPort.TryConnect(oc.HeadEndPoint); //decision is head node

                    //Style the link
                    oc.LineStyle.LineColor = Color.RosyBrown;
                    oc.LineStyle.LineWidth = 2f;

                    //Head decorator style
                    oc.HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    oc.HeadDecorator.Size = new SizeF(8, 8);
                    oc.HeadDecorator.FillStyle.Color = Color.RosyBrown;
                    oc.HeadDecorator.LineStyle.LineColor = Color.RosyBrown;

                    //Tail decorator style
                    oc.TailDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    oc.TailDecorator.Size = new SizeF(8, 8);
                    oc.TailDecorator.FillStyle.Color = Color.RosyBrown;
                    oc.TailDecorator.LineStyle.LineColor = Color.RosyBrown;

                    c = oc;
                    break;

                case ConnectorEnum.OrgLine:
                    OrgLineConnector org = new OrgLineConnector(m1.PinPoint, m2.PinPoint);
                    m1.CentralPort.TryConnect(org.TailEndPoint);
                    m2.CentralPort.TryConnect(org.HeadEndPoint);

                    //Style the link
                    org.LineStyle.LineColor = Color.RosyBrown;
                    org.LineStyle.LineWidth = 2f;

                    //Head decorator style
                    org.HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    org.HeadDecorator.Size = new SizeF(8, 8);
                    org.HeadDecorator.FillStyle.Color = Color.RosyBrown;
                    org.HeadDecorator.LineStyle.LineColor = Color.RosyBrown;

                    //Tail decorator style
                    org.TailDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    org.TailDecorator.Size = new SizeF(8, 8);
                    org.TailDecorator.FillStyle.Color = Color.RosyBrown;
                    org.TailDecorator.LineStyle.LineColor = Color.RosyBrown;

                    c = org;
                    break;

                case ConnectorEnum.Polyline:
                    //Create an orthogonal connector
                    PolyLineConnector pc = new PolyLineConnector(m1.PinPoint, m2.PinPoint);
                    m1.CentralPort.TryConnect(pc.TailEndPoint);
                    m2.CentralPort.TryConnect(pc.HeadEndPoint);

                    //Style the link
                    pc.LineStyle.LineColor = Color.RosyBrown;
                    pc.LineStyle.LineWidth = 2f;

                    //Head decorator style
                    pc.HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    pc.HeadDecorator.Size = new SizeF(8, 8);
                    pc.HeadDecorator.FillStyle.Color = Color.RosyBrown;
                    pc.HeadDecorator.LineStyle.LineColor = Color.RosyBrown;

                    //Tail decorator style
                    pc.TailDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    pc.TailDecorator.Size = new SizeF(8, 8);
                    pc.TailDecorator.FillStyle.Color = Color.RosyBrown;
                    pc.TailDecorator.LineStyle.LineColor = Color.RosyBrown;

                    c = pc;
                    break;

                case ConnectorEnum.Spline:
                    //SplineNode sc = new SplineNode((m1.PinPoint, m2.PinPoint);
                    //c = sc;
                    MessageBox.Show("Sorry, Connector type not implemented");
                    return null;
                    break;

                case ConnectorEnum.Bezier:

                    //Create a bezierCurve
                    BezierCurve bezierCurve = new BezierCurve(new PointF[]
                    {
                        new PointF(0, 0), 
                            new PointF(0, 100),
                            new PointF(100, 0),
                            new PointF(100, 100) });
#if false
                    new PointF(0, 60), 
                            new PointF(28, 6), 
                            new PointF(72, 0), 
                            new PointF(100, 60), 
                            new PointF(124, 6), 
                            new PointF(144, 0), 
                            new PointF(200, 60), 
                            new PointF(248, 6), 
                            new PointF(288, 0), 
                            new PointF(320, 60) });
#endif

                    //Connect a Nodes.
                    m1.CentralPort.TryConnect(bezierCurve.TailEndPoint);
                    m2.CentralPort.TryConnect(bezierCurve.HeadEndPoint);


                    //PointF p1 = new PointF(m1.CentralPort.OffsetX, m1.CentralPort.OffsetY);
                    //PointF p2 = new PointF(m2.CentralPort.OffsetX, m2.CentralPort.OffsetY);
                    //BezierCurve bc = new BezierCurve(p1, p2);

                    //Style the link
                    bezierCurve.LineStyle.LineColor = Color.RosyBrown;
                    bezierCurve.LineStyle.LineWidth = 2f;

                    //Head decorator style
                    bezierCurve.HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    bezierCurve.HeadDecorator.Size = new SizeF(8, 8);
                    bezierCurve.HeadDecorator.FillStyle.Color = Color.RosyBrown;
                    bezierCurve.HeadDecorator.LineStyle.LineColor = Color.RosyBrown;

                    //Tail decorator style
                    bezierCurve.TailDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
                    bezierCurve.TailDecorator.Size = new SizeF(8, 8);
                    bezierCurve.TailDecorator.FillStyle.Color = Color.RosyBrown;
                    bezierCurve.TailDecorator.LineStyle.LineColor = Color.RosyBrown;

                    m_diagram.Model.AppendChild(bezierCurve);
                    return null;
                    //c = bc;
                    break;

            }

            //Add labels to the conector
            if (string.IsNullOrEmpty(m1RelText) == false)
            {
                Syncfusion.Windows.Forms.Diagram.Label label1;
                label1 = new Syncfusion.Windows.Forms.Diagram.Label(c, m1RelText);
                label1.FontStyle.Family = "Arial";
                label1.FontColorStyle.Color = Color.Black;
                label1.Orientation = LabelOrientation.Horizontal;
                label1.Position = Position.TopLeft;
                label1.UpdatePosition = true;

                if (combineRelText && (string.IsNullOrEmpty(m2RelText) == false) && (m1RelText != m2RelText))
                {
                    string combinedText = string.Format("{0} > < {1}", m1RelText, m2RelText);
                    label1.Text = combinedText;
                }
                c.Labels.Add(label1);
            }

            if (!combineRelText && string.IsNullOrEmpty(m2RelText) == false)
            {
                Syncfusion.Windows.Forms.Diagram.Label label2;
                label2 = new Syncfusion.Windows.Forms.Diagram.Label(c, m2RelText);
                label2.FontStyle.Family = "Arial";
                label2.FontColorStyle.Color = Color.Black;
                label2.Orientation = LabelOrientation.Horizontal;
                label2.Position = Position.TopRight;
                label2.UpdatePosition = true;
                c.Labels.Add(label2);
            }

            m_diagram.Model.AppendChild(c);
            return c;
        }


        //*******************************************************************
        public void AddConnectorProperty(ConnectorBase connector, string propertyName, object obj)
        {
            connector.PropertyBag.Add(propertyName, obj);
        }

        public object GetConectorProperty(ConnectorBase connector, string propertyName)
        {
            object o = null;
            connector.PropertyBag.TryGetValue(propertyName, out o);
            return o;
        }

        //*******************************************************************
        //*******************************************************************
        // Called from the AddRel form
        public void AddRel(AddRelInfo ari)
        {
            ConnectorBase co = ConnectMembers(ari.Member1, 
                                              ari.Member2,
                                              ari.RelType1.ToString(),
                                              ari.RelType2.ToString());
            SfHelper.SfhAddRelationship(ari);
        }


        //*******************************************************************
        //*******************************************************************
        public void SetLayout ()
        {
            switch (m_layoutSel)
            {
                case LayoutEnum.Symmetric:
                    SymmetricLayoutManager symmetricLayout = new SymmetricLayoutManager(m_diagram.Model, 500);
                    symmetricLayout.SpringFactor = 0.442;
                    symmetricLayout.SpringLength = 200;
                    symmetricLayout.MaxIteraction = 500;
                    m_diagram.LayoutManager = symmetricLayout;
                    break;

                case LayoutEnum.Table:
                    TableLayoutManager tlLayout = new TableLayoutManager(m_diagram.Model, 7, 7);
                    tlLayout.VerticalSpacing = 100;
                    tlLayout.HorizontalSpacing = 100;
                    tlLayout.CellSizeMode = CellSizeMode.EqualToMaxNode;
                    tlLayout.Orientation = Orientation.Horizontal;
                    tlLayout.MaxSize = new SizeF(500, 600);
                    m_diagram.LayoutManager = tlLayout;
                    break;

                case LayoutEnum.DirectedTree:
                    DirectedTreeLayoutManager directedLayout = new DirectedTreeLayoutManager(m_diagram.Model, 0, 20, 20);
                    m_diagram.LayoutManager = directedLayout;
                    break;

                case LayoutEnum.RadialTree:
                    RadialTreeLayoutManager radialLayout = new RadialTreeLayoutManager(m_diagram.Model, 0, 20, 20);
                    m_diagram.LayoutManager = radialLayout;
                    break;

                case LayoutEnum.Hierarchic:
                    HierarchicLayoutManager hierarchyLayout = new HierarchicLayoutManager(m_diagram.Model, 0, 10, 20);
                    m_diagram.LayoutManager = hierarchyLayout;
                    break;

                case LayoutEnum.SubgraphTree:
                    SubgraphTreeLayoutManager st = new SubgraphTreeLayoutManager(m_diagram.Model, 0, 20, 20);
                    st.SubgraphPreferredLayout += new SubgraphPreferredLayoutEventHandler(st_SubgraphPreferredLayout);
                    m_diagram.LayoutManager = st;
                    break;

                case LayoutEnum.OrgChart:
                    OrgChartLayoutManager manager = new OrgChartLayoutManager(m_diagram.Model, RotateDirection.TopToBottom, 20, 50);
                    m_diagram.LayoutManager = manager;
                    break;
            }
            m_diagram.LayoutManager.UpdateLayout(null);
        }


        private void st_SubgraphPreferredLayout(object sender, SubgraphPreferredLayoutEventArgs evtArgs)
        {
            evtArgs.ResizeSubgraphNodes = false;
            evtArgs.RotationDegree = 0;
        }

        //*****************************************************************************************
        private void mnuAddMember(object sender, EventArgs e)
        {
            AddMember amForm = new AddMember(this);
            amForm.Show();
        }

        //*****************************************************************************************
        private void OnAddRel_Click(object sender, EventArgs e)
        {
            string memberName1 = m_prevClickedNode != null ? m_prevClickedNode.Name : string.Empty;
            string memberName2 = m_lastClickedNode != null ? m_lastClickedNode.Name : string.Empty; 

            AddRelationship arf = new AddRelationship(this, memberName1, memberName2);
            arf.Show();
        }

        //*****************************************************************************************
        private void OnRelCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex + 1 > Relationships.Count)
                return;

            // See if user clicked to get relationship details
            if (e.ColumnIndex == 3)
            {
                SfdRelationshipData rel = Relationships[e.RowIndex];
            }

        }

        //*****************************************************************************************
        private void OnTimerScreenRefresh_Tick(object sender, EventArgs e)
        {
            DateTime dtNow = DateTime.Now;
            if (MemberData.BirthdayKnown)
            {
                tbBday.Text = MemberData.Birthday.ToString("MMMM d yyyy");
                string bdText = Analytics.GetTimeBetween(MemberData.Birthday, dtNow, PeriodUnits.YearMonthDay | PeriodUnits.HourMinuteSecond, true);
                tbCurrentAge.Text = bdText;

                DateTime nextBday = MemberData.Birthday;
                while (nextBday < dtNow)
                    nextBday = nextBday.AddYears(1);

                string nextBdText = Analytics.GetTimeBetween(nextBday, dtNow, PeriodUnits.YearMonthDay | PeriodUnits.HourMinuteSecond, true);
                tbTimeToBday.Text = nextBdText;
            }
            else
            {
                tbBday.Text = "?";
                tbCurrentAge.Text = "??";
                tbTimeToBday.Text = "???";
            }

            if (MemberData.DeathdayKnown)
            {
                tbDeathDay.Text = MemberData.Deathday.ToString("MMMM d yyyy");
                if (MemberData.BirthdayKnown)
                {
                    string ddText = Analytics.GetTimeBetween(dtNow, MemberData.Deathday, PeriodUnits.YearMonthDay | PeriodUnits.HourMinuteSecond, true);
                    tbTimeLeft.Text = ddText;
                }
            }
            else
            {
                tbDeathDay.Text = "???";
                tbTimeLeft.Text = "????";
            }
        }

        //******************************************************************************
        private void SfDiagram_Load(object sender, EventArgs e)
        {

        }
    }

    public class SfdRelationshipData
    {
        public SfdMember RelatedMemberNode;
        public string RelatedTo;
        public string RelationType;
    }

    public class SfdMemberData
    {
        public SfdMember MemberNode;
        public string FirstName;
        public string MiddleName;
        public string LastName;
        public string CommonName;

        public bool BirthdayKnown;
        public DateTime Birthday;

        public bool DeathdayKnown;
        public DateTime Deathday;
    }

}
