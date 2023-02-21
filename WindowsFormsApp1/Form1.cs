using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int xVal = 10;
        private int selectedX = 0;
        private int selectedY = 0;
        private int selectedCenterX = 0;
        private int selectedCenterY = 0;
        private int selectedHeight = 0;
        private int selectedWidth = 0;
        private int shapeType = 0;
        private Color selectedColor = Color.Black;
        private List<Shapes> shapes;
        private List<PictureBox> shapeButtons;
        private List<PictureBox> colorButtons;
        private Shapes activeShape;
        private Shapes shapeHighlight;
        private bool IsMouseDown = false;
        private bool SelectionActive;

        public enum ShapeTypes
        {
            Square = 1,
            Circle = 2,
            Triangle = 3,
            Hexagon = 4
        }

        public enum Colors
        {
            Red = 1,
            Blue = 2,
            Green = 3,
            Orange = 4,
            Black = 5,
            Yellow = 6,
            Purple = 7,
            Brown = 8,
            White = 9
        }


        public Form1()
        {
            InitializeComponent();
            shapeButtons = new List<PictureBox>
            {
                btnSquare,
                btnCircle,
                btnTriangle,
                btnHexagon
            };

            colorButtons = new List<PictureBox>
            {
                btnRed,
                btnGreen,
                btnBlue,
                btnOrange,
                btnBlack,
                btnYellow,
                btnYellow,
                btnPurple,
                btnBrown,
                btnWhite
            };
            shapes = new List<Shapes>();
            SelectionActive = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            
            IsMouseDown = true;
            selectedCenterX = e.X;
            selectedCenterY = e.Y;
            

            if (shapeType == (int)ShapeTypes.Square)
            {
                activeShape = new SquareShape();
            }
            else if (shapeType == (int)ShapeTypes.Circle)
            {
                activeShape = new CircleShape();
            }
            else if (shapeType == (int)ShapeTypes.Triangle)
            {
                activeShape = new TriangleShape();
            }
            else if (shapeType == (int)ShapeTypes.Hexagon)
            {
                activeShape = new HexagonShape();
            }
            else if (shapeType == 0)
            {
                if (SelectionActive)
                {
                    if (activeShape != null)
                    {

                        shapes.Add(activeShape);
                    }

                    foreach (var item in shapes.Reverse<Shapes>().ToList())
                    {
                        if (e.X >= item.X && e.X <= (item.X + item.Width) && e.Y >= item.Y && e.Y <= (item.Y + item.Height))
                        {
                            activeShape = item;
                            selectedColor = activeShape._color;
                            shapes.Remove(activeShape);
                            Console.WriteLine($"shape he: {activeShape.Height} wi: {activeShape.Width} ");

                            assignHighlightShape(activeShape);

                            panel1.Refresh();
                            break;
                        }
                    }


                }
            }
            if (activeShape != null)
            {
                activeShape._color = selectedColor;
            }
        }

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            
            IsMouseDown = false;
            
            if (!SelectionActive)
            {               
                shapes.Add(activeShape);
                activeShape = null;
            }
                       

        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            bool validDraw = false;
            if (IsMouseDown)
            {
                if (!SelectionActive)
                {
                    if (e.X < selectedCenterX)
                    {
                        if (panel1.Location.X <= e.X )
                        {
                            selectedX = e.X;                            
                            validDraw = true;
                        }
                        if(panel1.Width >= ((2 * (selectedCenterX - e.X)) + e.X))
                        {
                            selectedWidth = 2 * (selectedCenterX - e.X);
                            validDraw = true;
                        }
                    }
                    else if (e.X >= selectedCenterX)
                    {
                        int tmpX = (2 * selectedCenterX - e.X);
                        if ((panel1.Location.X) <= tmpX )
                        {                            
                            selectedX = tmpX;
                            validDraw= true;
                        }
                        if(panel1.Width >= ((2 * (e.X - selectedCenterX)) + tmpX))
                        {
                            selectedWidth = 2 * (e.X - selectedCenterX);
                            validDraw = true;
                        }
                    }

                    if (e.Y < selectedCenterY )
                    {
                        if (panel1.Location.Y <= e.Y )
                        {
                            selectedY = e.Y;                            
                            validDraw = true;
                        }
                        if(panel1.Height >= ((2 * (selectedCenterY - e.Y))) + e.Y)
                        {
                            selectedHeight = 2 * (selectedCenterY - e.Y);
                            validDraw = true;
                        }
                    }
                    else if (e.Y >= selectedCenterY )
                    {
                        int tmpY = (2 * selectedCenterY - e.Y);
                        if ((panel1.Location.Y) <= tmpY)
                        {                            
                            selectedY = tmpY;
                            validDraw = true;
                        }
                        if(panel1.Height >= (2 * (e.Y - selectedCenterY)) + tmpY)
                        {
                            selectedHeight = 2 * (e.Y - selectedCenterY);
                            validDraw = true;
                        }
                    }
                    if (activeShape != null && validDraw)
                    {
                        activeShape.X = selectedX;
                        activeShape.Y = selectedY;
                        activeShape.Width = selectedWidth;
                        activeShape.Height = selectedHeight;
                        activeShape.Edge = Math.Max(activeShape.Width, activeShape.Height);
                    }

                }
                else
                {
                    if (activeShape != null)
                    {
                        int newX = activeShape.X + e.X - selectedCenterX;
                        int newY = activeShape.Y + e.Y - selectedCenterY;
                        if ((newX + activeShape.Width) <= panel1.Width && newX >= panel1.Location.X && (newY + activeShape.Height) <= panel1.Height && newY >= panel1.Location.Y)
                        {
                            activeShape.X = newX;
                            activeShape.Y = newY;
                            shapeHighlight.X = newX;
                            shapeHighlight.Y = newY;
                            selectedCenterY = e.Y;
                            selectedCenterX = e.X;
                            validDraw = true;
                        }
                    }


                }
                if (validDraw)
                {
                    this.Refresh();
                }



            }


            label_location.Text = $"Mouse Location: x:{e.X}, y:{e.Y}";
        }

        private void assignHighlightShape(Shapes shape)
        {
            

            if (shape.GetType() == typeof(SquareShape))
            {
                shapeHighlight = new SquareShape(Color.FromArgb(64, 0, 0, 0), activeShape.X - 5, activeShape.Y - 5, activeShape.Edge);
            }
            else if (shape.GetType() == typeof(CircleShape))
            {
                shapeHighlight = new CircleShape(Color.FromArgb(64, 0, 0, 0), activeShape.X - 10, activeShape.Y - 10, activeShape.Edge);
            }
            else if (shape.GetType() == typeof(TriangleShape))
            {
                shapeHighlight = new TriangleShape(Color.FromArgb(64, 0, 0, 0), activeShape.X, activeShape.Y, activeShape.Edge);
            }
            else if (shape.GetType() == typeof(HexagonShape))
            {
                shapeHighlight = new HexagonShape(Color.FromArgb(64, 0, 0, 0), activeShape.X - 10, activeShape.Y - 10, activeShape.Edge);
            }
        }

        private void drawingMethod(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var item in shapes)
            {
                item.DrawIt(g);
            }

            if (activeShape != null)
            {
                activeShape.DrawIt(g);
                if (shapeHighlight != null)
                {
                    shapeHighlight.DrawIt(g);
                }
            }


        }

        #region shape_btn

        private void btnSquare_Click(object sender, EventArgs e)
        {
            shapeType = (int)ShapeTypes.Square;
            displaySelectedBtn(shapeButtons, sender);
            btnSelect.BackColor = System.Drawing.SystemColors.Control;
            deactiveShape();
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            shapeType = (int)ShapeTypes.Circle;
            displaySelectedBtn(shapeButtons, sender);
            btnSelect.BackColor = System.Drawing.SystemColors.Control;
            deactiveShape();
        }

        private void btnTriangle_Click(object sender, EventArgs e)
        {
            shapeType = (int)ShapeTypes.Triangle;
            displaySelectedBtn(shapeButtons, sender);
            btnSelect.BackColor = System.Drawing.SystemColors.Control;
            deactiveShape();
        }

        private void btnHexagon_Click(object sender, EventArgs e)
        {
            shapeType = (int)ShapeTypes.Hexagon;
            displaySelectedBtn(shapeButtons, sender);
            btnSelect.BackColor = System.Drawing.SystemColors.Control;
            deactiveShape();
        }
        #endregion

        #region color_btn
        private void btnRed_Click(object sender, EventArgs e)
        {

            selectedColor = Color.Red;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            selectedColor = Color.Blue;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnGreen_Click(object sender, EventArgs e)
        {
            selectedColor = Color.Green;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnOrange_Click(object sender, EventArgs e)
        {
            selectedColor = Color.Orange;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            selectedColor = Color.Black;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnYellow_Click(object sender, EventArgs e)
        {
            selectedColor = Color.Yellow;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnPurple_Click(object sender, EventArgs e)
        {
            selectedColor = Color.Purple;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnBrown_Click(object sender, EventArgs e)
        {
            selectedColor = Color.Brown;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        private void btnWhite_Click(object sender, EventArgs e)
        {
            selectedColor = Color.White;
            displaySelectedBtn(colorButtons, sender);
            paintShape();
        }

        #endregion

        private void paintShape()
        {
            if (activeShape != null)
            {
                activeShape._color = selectedColor;
                panel1.Refresh();
            }
        }

        private void displaySelectedBtn(List<PictureBox> list, object sender)
        {
            foreach (var item in list)
            {
                item.BackColor = System.Drawing.SystemColors.Control;
            }
            PictureBox thisBox = (PictureBox)sender;
            thisBox.BackColor = System.Drawing.SystemColors.ControlDark;
        }

        private void deactiveShape()
        {
            if (activeShape != null)
            {
                activeShape.IsSelected = false;
                shapes.Add(activeShape);
                activeShape = null;
            }

            shapeHighlight = null;
            SelectionActive = false;
            panel1.Refresh();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            displaySelectedBtn(shapeButtons, sender);
            SelectionActive = true;
            shapeType = 0;

        }

        private void btnDeleteitem_Click(object sender, EventArgs e)
        {
            if (activeShape != null)
            {
                shapes.Remove(activeShape);
                activeShape = null;
                panel1.Refresh();
            }

        }

        private void btnNewpage_Click(object sender, EventArgs e)
        {
            shapes.Clear();
            activeShape = null;
            shapeHighlight = null;
            panel1.Refresh();
        }

        private void btnSavefile_Click(object sender, EventArgs e)
        {
            List<Shapes> tmpList = shapes;
            if (activeShape != null)
            {
                activeShape.IsSelected = true;
                tmpList.Add(activeShape);
            }

            if (tmpList.Count > 0)
            {
                Console.WriteLine(JsonConvert.SerializeObject(tmpList));

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Browse Files";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.Filter = "Shape File (*.ext)|*.ext|All files (*.*)|*.*";

                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine(saveFileDialog.FileName);
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                    sw.WriteLine(JsonConvert.SerializeObject(tmpList, settings));
                    sw.Close();
                }
            }




        }

        private void btnOpenfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Browse Files";
            openFileDialog.Filter = "Shape File (*.ext)|*.ext|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    var tmpList = JsonConvert.DeserializeObject<List<Shapes>>(File.ReadAllText(openFileDialog.FileName), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    //Console.WriteLine("test");
                    shapes.Clear();
                    foreach (var item in tmpList)
                    {
                        if (!item.IsSelected)
                        {
                            shapes.Add(item);
                        }
                        else
                        {
                            activeShape = item;
                            assignHighlightShape(activeShape);
                        }
                    }

                    displaySelectedBtn(shapeButtons, sender);
                    SelectionActive = true;
                    btnSelect.BackColor = System.Drawing.SystemColors.ControlDark;
                    shapeType = 0;

                    this.Refresh();
                }
            }
        }


        

        
        

        


    }
}
