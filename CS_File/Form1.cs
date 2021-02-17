using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace Ekran_SS_Alma_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public static List<Color> TenMostUsedColors { get; private set; }
        public static List<int> TenMostUsedColorIncidences { get; private set; }
        public static int AverageTen { get; private set; }

        public static Color MostUsedColor { get; private set; }
        public static int MostUsedColorIncidence { get; private set; }

        private static int pixelColor;

        private static Dictionary<int, int> dctColorIncidence;

        static Bitmap btmap = new Bitmap(1920, 1080, System.Drawing.Imaging.PixelFormat.Format24bppRgb);



        public static void GetMostUsedColor(Bitmap theBitMap)
        {
            Graphics grafik = Graphics.FromImage(btmap);
            grafik.CopyFromScreen(0, 0, 0, 0, new Size(btmap.Width, btmap.Height));


            TenMostUsedColors = new List<Color>();
            TenMostUsedColorIncidences = new List<int>();

            MostUsedColor = Color.Empty;
            MostUsedColorIncidence = 0;

            // does using Dictionary<int,int> here
            // really pay-off compared to using
            // Dictionary<Color, int> ?

            // would using a SortedDictionary be much slower, or ?

            dctColorIncidence = new Dictionary<int, int>();

            // this is what you want to speed up with unmanaged code
            for (int row = 0; row < theBitMap.Size.Width; row+=5)
            {
                for (int col = 0; col < theBitMap.Size.Height; col+=5)
                {
                    pixelColor = theBitMap.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        dctColorIncidence.Add(pixelColor, 1);
                    }
                }
            }

            // note that there are those who argue that a
            // .NET Generic Dictionary is never guaranteed
            // to be sorted by methods like this
            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            int i = 1;
            // this should be replaced with some elegant Linq ?
            foreach (KeyValuePair<int, int> kvp in dctSortedByValueHighToLow.Take(10))
            {
                

                TenMostUsedColors.Add(Color.FromArgb(kvp.Key));
                TenMostUsedColorIncidences.Add(kvp.Value);

                /*AverageTen += dctSortedByValueHighToLow[kvp.Key]/i;
                i++;*/
            }
            i = 1;
            
            
            
            

            MostUsedColor = Color.FromArgb(dctSortedByValueHighToLow.First().Key);
            MostUsedColorIncidence = dctSortedByValueHighToLow.First().Value;


        }







        private void Form1_Load(object sender, EventArgs e)
        {

            timer1.Interval = 150;

            
            

            string[] ports = SerialPort.GetPortNames();  //Seri portları diziye ekleme
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);               //Seri portları comboBox1'e ekleme
            }

            serialPort1.PortName = "COM10";
            serialPort1.Open();

            if(serialPort1.IsOpen)
            {
                timer1.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* GetMostUsedColor(btmap);
            textBox4.Text = MostUsedColor.ToString();
            serialPort1.Write(new[] { MostUsedColor.R, MostUsedColor.G, MostUsedColor.B }, 0, 3);
            textBox1.Text = MostUsedColor.GetSaturation().ToString();*/

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetMostUsedColor(btmap);
            textBox4.Text = MostUsedColor.ToString();
            serialPort1.Write(new[] { MostUsedColor.R, MostUsedColor.G, MostUsedColor.B }, 0, 3);
            //textBox1.Text = MostUsedColor.GetSaturation().ToString();
        }

        bool x = false;
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (x == false)
            {
                timer1.Stop();
                serialPort1.Write(new[] { Color.Black.R, Color.Black.G, Color.Black.B /*'0','0','0' */}, 0, 3);
                x = true;
            }
            else if(x == true)
            {
                timer1.Start();
                x = false;
            }
            
        }






        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*serialPort1.PortName = comboBox1.SelectedItem.ToString();
            serialPort1.Open();*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = AverageTen.ToString();
        }
    }
}
