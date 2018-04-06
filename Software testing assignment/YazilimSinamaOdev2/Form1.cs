using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
//using System.Timers;

namespace YazilimSinamaOdev2
{
    public partial class Frm_Maze : Form
    {
        const int TIMER_MS = 300;
        const int DUVAR = 250;
        const int FARE = 225;
        const int PEYNIR = 200;

        Labirent lab = new Labirent();
        DataGridViewImageColumn col;
        Timer timer = new Timer();


        //int gidilenYon = 255;
        int[,] dataGridDizi;
        int[] fareKonumu = new int[2];
        int[] sonrakiKonum = new int[2];
        int oyunBasladi = 0;
        int peynirBulundu = 0;
        int sonrakiKonumDegeriDown = 0;
        int sonrakiKonumDegeriRight = 0;
        int sonrakiKonumDegeriLeft = 0;
        int sonrakiKonumDegeriUp = 0;



        Image im = YazilimSinamaOdev2.Properties.Resources.Brick_Block;
       
        Image im2 = YazilimSinamaOdev2.Properties.Resources.blank_image;
        
        Image imFareRight = YazilimSinamaOdev2.Properties.Resources.mouse_right;
        
        Image imFareLeft = YazilimSinamaOdev2.Properties.Resources.mouse_left;
        
        Image imFareUp = YazilimSinamaOdev2.Properties.Resources.mouse_up;
        
        Image imFareDown = YazilimSinamaOdev2.Properties.Resources.mouse_down;
        
        Image imPeynir = YazilimSinamaOdev2.Properties.Resources.peynir;
        
        Image imFarePeynir = YazilimSinamaOdev2.Properties.Resources.mouseeatingcheese;
        
        Image imFarePeynir2 = YazilimSinamaOdev2.Properties.Resources.mouseeatingcheese2;
        

        public Frm_Maze()
        {
            InitializeComponent();
            pictureBox1.Enabled = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ScrollBars = 0;
            dataGridView1.Height = 0;
            dataGridView1.Width = 0;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            button2.Enabled = false;
            dataGridView1.Enabled = false;


            

            WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Interval = TIMER_MS;
            timer.Tick += new EventHandler(timer_Tick);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (txtBoxSatir.Text == "" || txtBoxSutun.Text == "")
            {
                MessageBox.Show("bos deger girilemez");
            }
            else if (IsPunct(txtBoxSatir.Text) || IsPunct(txtBoxSutun.Text))
            {
                MessageBox.Show("It is an punctuation");
                txtBoxSatir.Clear();
                txtBoxSutun.Clear();
            }
            else if(IsAlpha(txtBoxSatir.Text) || IsAlpha(txtBoxSutun.Text))
            {
                MessageBox.Show("texttir");
                txtBoxSatir.Clear();
                txtBoxSutun.Clear();
            }
            else if( ( IsDigit(txtBoxSatir.Text) && IsDigit(txtBoxSutun.Text) )  &&
                     ( int.Parse(txtBoxSatir.Text) < 4 || int.Parse(txtBoxSutun.Text) < 4 || int.Parse(txtBoxSatir.Text) > 20 || int.Parse(txtBoxSutun.Text) > 30) )
            {
                MessageBox.Show("En fazla 20 satır- 30 Sutun ve en az 4 satır-Sutun girilebilir");
                txtBoxSatir.Clear();
                txtBoxSutun.Clear();
            }
            else if(IsDigit(txtBoxSatir.Text) && IsDigit(txtBoxSutun.Text))
            {
                dataGridDizi = new int[int.Parse(txtBoxSatir.Text),int.Parse(txtBoxSutun.Text)];
                lab = new Labirent(int.Parse(txtBoxSatir.Text), int.Parse(txtBoxSutun.Text));
                dataGridView1.Enabled = true;
                CreateLabirent();
                button1.Enabled = false;
                button2.Enabled = true;
                dataGridView1.Enabled = true;
                btnBasla.Enabled = true;
            }
            else
            {
                MessageBox.Show("Girdiginiz deger gecerli degildir");
                txtBoxSatir.Clear();
                txtBoxSutun.Clear();
            }
        }
        
        

        private void label1_Click(object sender, EventArgs e)
        {

        }




        private void CreateLabirent()
        {

            for (int i = 0; i< lab.sutunNo; i++)
            {
                col = new DataGridViewImageColumn();
                col.Width = 35;
                dataGridView1.Columns.Add(col);
                col.Image = im2;
            }
            for (int i = 0; i < lab.satirNo; i++)
            {
                DataGridViewRow cl = new DataGridViewRow();
                cl.Height = 35;
                dataGridView1.Rows.Add(cl);
            }
            
            for(int i = 0; i < lab.sutunNo; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = im;
                dataGridView1.Rows[lab.satirNo - 1].Cells[i].Value = im;
                dataGridDizi[0,i] = DUVAR;
                dataGridDizi[lab.satirNo - 1, i] = DUVAR;
            }
            for(int i = 0; i < lab.satirNo; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = im;
                dataGridView1.Rows[i].Cells[lab.sutunNo - 1].Value = im;
                dataGridDizi[i, 0] = DUVAR;
                dataGridDizi[i, lab.sutunNo - 1] = DUVAR;
            }
            dataGridView1.Rows[lab.satirNo - 2].Cells[1].Value = imFareDown;
            dataGridView1.Rows[1].Cells[lab.sutunNo - 2].Value = imPeynir;
            dataGridDizi[lab.satirNo - 2, 1] = FARE;
            fareKonumu[0] = lab.satirNo - 2;
            fareKonumu[1] = 1;
            dataGridDizi[1, lab.sutunNo - 2] = PEYNIR;

            dataGridView1.Height = 35 * lab.satirNo;
            dataGridView1.Width = 35 * lab.sutunNo;
        }




        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Height = 0;
            dataGridView1.Width = 0;
            button1.Enabled = true;
            button2.Enabled = false;
            btnBasla.Enabled = false;
            pictureBox1.Image = null;
            pictureBox1.Enabled = false;
            timer.Stop();
            dataGridDizi = new int[20, 20];
            fareKonumu = new int[2];
            sonrakiKonum = new int[2];
            oyunBasladi = 0;
            peynirBulundu = 0;
            sonrakiKonumDegeriDown = 0;
            sonrakiKonumDegeriRight = 0;
            sonrakiKonumDegeriLeft = 0;
            sonrakiKonumDegeriUp = 0;
        }
   


        public static bool IsAlpha(string a)
        {
            
            foreach (char c in a)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }
        public static bool IsPunct(string a)
        {
            foreach(char c in a)
            {
                if (!char.IsPunctuation(c))
                    return false;
            }
            return true;
        }
        public static bool IsDigit(string a)
        {
            foreach (char c in a)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
        private void txtBoxSatir_TextChanged(object sender, EventArgs e)
        {
            if (txtBoxSatir.Text != "")
            {

                if (IsAlpha(txtBoxSatir.Text))
                {
                    WarningLabel.Text = "girdiginiz deger karakter, lutfen bir tam sayı giriniz"; 
                }
                else if (IsPunct(txtBoxSatir.Text))
                {
                    WarningLabel.Text = "girdiginiz deger tam sayi degildir, lutfen bir tam sayı giriniz";
                }
                //else if (int.Parse(txtBoxSatir.Text) > 20)
                //{
                //    WarningLabel.Text = "Lutfen 20 den daha kucuk bir deger giriniz";

                //}
                txtBoxSatir.MaxLength = 5;
                
            }
            
        }
        private void txtBoxSutun_TextChanged(object sender, EventArgs e)
        {
            if (txtBoxSutun.Text != "")
            {
                if (IsAlpha(txtBoxSutun.Text))
                {
                    WarningLabel.Text = "girdiginiz deger karakter, lutfen bir tam sayı giriniz";
                }
                else if (IsPunct(txtBoxSutun.Text))
                {
                    WarningLabel.Text = "girdiginiz deger tam sayi degildir, lutfen bir tam sayı giriniz";
                }
                //else if (int.Parse(txtBoxSutun.Text) > 20)
                //{
                //    WarningLabel.Text = "Lutfen 20 den daha kucuk bir deger giriniz";

                //}
                txtBoxSutun.MaxLength = 5;
            }
        }



        

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
            
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 || e.RowIndex == lab.satirNo - 1 || e.ColumnIndex == 0 || e.ColumnIndex == lab.sutunNo - 1
                || (e.RowIndex == lab.satirNo - 2 && e.ColumnIndex == 1) || (e.RowIndex == 1 && e.ColumnIndex == lab.sutunNo - 2)
                )
            {
            }

            else
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == im2)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = im;
                    dataGridDizi[e.RowIndex, e.ColumnIndex] = DUVAR;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = im2;
                    dataGridDizi[e.RowIndex, e.ColumnIndex] = 0;
                }
            }
        }
        private void btnBasla_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Enabled == true)
            {
                dataGridView1.Enabled = false;
                timer.Start();
                oyunBasladi = 0;
                btnBasla.Enabled = false;
            }
            else
            {
                MessageBox.Show("Labirent hala olusturulmamıstır, once Labirent olustur sonra Basla butonuna basınız");
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            SonrakiKonumBul();
            dataGridDizi[sonrakiKonum[0], sonrakiKonum[1]] += FARE;

            if (oyunBasladi == 0)
            {
                oyunBasladi = 1;
                dataGridDizi[fareKonumu[0], fareKonumu[1]] = 1;
            }
            else
            {
                dataGridDizi[fareKonumu[0], fareKonumu[1]] = dataGridDizi[fareKonumu[0], fareKonumu[1]] - FARE + 1;
            }
            fareKonumu[0] = sonrakiKonum[0];
            fareKonumu[1] = sonrakiKonum[1];

            if (fareKonumu[0] == 1 && fareKonumu[1] == lab.sutunNo - 2)
            {
                peynirBulundu = 1;
                dataGridView1.Rows[1].Cells[lab.sutunNo - 2].Value = imFarePeynir;
                timer.Stop();
                pictureBox1.Enabled = true;
                pictureBox1.Image = imFarePeynir2;
            }
        }
        private int minbul(int down,int up,int right,int left)
        {
            int[] dizi = new int[4];
            dizi[0] = down;
            dizi[1] = up;
            dizi[2] = right;
            dizi[3] = left;
            int min = dizi[0];
            for(int i = 0;i < dizi.Length; i++)
            {
                if(min > dizi[i])
                {
                    min = dizi[i];
                }
            }
            if (down == 250 && up == 250 && right == 250 && left == 250)
            {
                min = - 1;
            }
            // eger peynire giden yol yoksa 
            if(down == 20 || up == 20 || right == 20|| left == 20)
            {
                timer.Stop();
                MessageBox.Show("peynire giden yol yok!!!!");
            }
            return min;
        }
        private void SonrakiKonumBul()
        {
            sonrakiKonumDegeriDown = dataGridDizi[fareKonumu[0] + 1, fareKonumu[1]];
            sonrakiKonumDegeriUp = dataGridDizi[fareKonumu[0] - 1, fareKonumu[1]];
            sonrakiKonumDegeriRight = dataGridDizi[fareKonumu[0], fareKonumu[1] + 1];
            sonrakiKonumDegeriLeft = dataGridDizi[fareKonumu[0], fareKonumu[1] - 1];
            int minRight = -1;
            int minLeft = -1;
            int minUp = -1;
            int minDown = -1;
            int minbulundu = minbul(sonrakiKonumDegeriDown, sonrakiKonumDegeriUp, sonrakiKonumDegeriRight, sonrakiKonumDegeriLeft);

            
            if(minbulundu == sonrakiKonumDegeriDown)
            {
                minDown = 0;
            }
            else if(minbulundu == sonrakiKonumDegeriUp)
            {
                minUp = 0;
            }
            else if(minbulundu == sonrakiKonumDegeriRight)
            {
                minRight = 0;
            }
            else if (minbulundu == sonrakiKonumDegeriLeft)
            {
                minLeft = 0;
            }
            
            //sag
            if (minRight != -1 || dataGridDizi[fareKonumu[0], fareKonumu[1] + 1] == PEYNIR)
            {
                sonrakiKonumDegeriRight = dataGridDizi[fareKonumu[0], fareKonumu[1] + 1];
                sonrakiKonum[0] = fareKonumu[0];
                sonrakiKonum[1] = fareKonumu[1] + 1;
                //
                dataGridView1.Rows[sonrakiKonum[0]].Cells[sonrakiKonum[1]].Value = imFareRight;
                dataGridView1.Rows[fareKonumu[0]].Cells[fareKonumu[1]].Value = im2;
                //
                if (dataGridDizi[fareKonumu[0], fareKonumu[1] + 1] == PEYNIR) return;
            }
            //yukarı
            else if (minUp != - 1 || dataGridDizi[fareKonumu[0] - 1, fareKonumu[1]] == PEYNIR)
            {
                sonrakiKonumDegeriUp = dataGridDizi[fareKonumu[0] - 1, fareKonumu[1]];
                sonrakiKonum[0] = fareKonumu[0] - 1;
                sonrakiKonum[1] = fareKonumu[1];
                //
                dataGridView1.Rows[sonrakiKonum[0]].Cells[sonrakiKonum[1]].Value = imFareUp;
                dataGridView1.Rows[fareKonumu[0]].Cells[fareKonumu[1]].Value = im2;
                //
                if (dataGridDizi[fareKonumu[0] - 1, fareKonumu[1]] == PEYNIR) return;
            }
            //sol
            else if (minLeft != - 1 || dataGridDizi[fareKonumu[0], fareKonumu[1] - 1] == PEYNIR)
            {
                sonrakiKonumDegeriLeft = dataGridDizi[fareKonumu[0], fareKonumu[1] - 1];
                sonrakiKonum[0] = fareKonumu[0];
                sonrakiKonum[1] = fareKonumu[1] - 1;
                //
                dataGridView1.Rows[sonrakiKonum[0]].Cells[sonrakiKonum[1]].Value = imFareLeft;
                dataGridView1.Rows[fareKonumu[0]].Cells[fareKonumu[1]].Value = im2;
                //
                if (dataGridDizi[fareKonumu[0], fareKonumu[1]] == PEYNIR) return;
            }
            //asagi
            else if(minDown != -1 || dataGridDizi[fareKonumu[0] + 1,fareKonumu[1]] == PEYNIR)
            {
                sonrakiKonumDegeriDown = dataGridDizi[fareKonumu[0] + 1, fareKonumu[1]];
                sonrakiKonum[0] = fareKonumu[0] + 1;
                sonrakiKonum[1] = fareKonumu[1];
                dataGridView1.Rows[sonrakiKonum[0]].Cells[sonrakiKonum[1]].Value = imFareDown;
                dataGridView1.Rows[fareKonumu[0]].Cells[fareKonumu[1]].Value = im2;
                if (dataGridDizi[fareKonumu[0], fareKonumu[1] + 1] == PEYNIR) return;
            }
            else
            {
                timer.Stop();
                MessageBox.Show("cikacak yolum yok :(");
            }
        }
    }
}
