using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace memorygame
{
    public partial class Form1 : Form
    {

        string path = "";
        Random r = new Random();
        PictureBox[] mangAnh;
        int n = 12, dem = 0;
        private DateTime curTime;
        int diem = 0;
        int count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            path = Application.StartupPath + @"\image\";
            mangAnh = new PictureBox[n];
            int k = 0, j = 1;
            for (int i = 0; i < mangAnh.Length; i++)
            {
                mangAnh[i] = new PictureBox();
                mangAnh[i].Size = new Size(120, 120);
                mangAnh[i].Image = Image.FromFile(path + "0.png");
                mangAnh[i].Location = new Point(30 + k * 130, 30 + j * 130);
                mangAnh[i].Enabled = true;
                mangAnh[i].SizeMode = PictureBoxSizeMode.StretchImage;
                mangAnh[i].Click += new EventHandler(pic_Click);
                this.Controls.Add(mangAnh[i]);
                k++;
                if (k == 4)
                {
                    k = 0; j++;
                }
            }
        }


        private void pic_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            pic.Image = Image.FromFile(path + pic.Tag.ToString() + ".png");
            count++;
            lbClick.Text = count.ToString();
            pic.Enabled = false;
            //pic.Text = " ";
            checkpic();
            if (dem == mangAnh.Length / 2)
            {
                dem = 0;
                timer2.Stop();
                //Check_HighScore();
                if (MessageBox.Show("Bạn đã hoàn thành trò chơi!\nBạn có muốn chơi mới?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    rePlay();
            }
        }

        private void checkpic()
        {
            for (int i = 0; i < mangAnh.Length - 1; i++)
            {
                for (int j = i + 1; j < mangAnh.Length; j++)
                {
                    if (mangAnh[i].Enabled == false && mangAnh[j].Enabled == false)       // lỗi chỗ này nếu ko để true
                    {
                        if (mangAnh[i].Tag.ToString() == mangAnh[j].Tag.ToString())
                        {
                            System.Threading.Thread.Sleep(300);
                            mangAnh[i].Visible = false;
                            mangAnh[j].Visible = false;

                            mangAnh[i].Enabled = true;     // vẫn để true lại là để tránh lỗi khi click cặp hình sau
                            mangAnh[j].Enabled = true;     // nếu để false thì sẽ bị lỗi khi duyệt đều kiện ở vòng lặp trên
                            dem++;
                            diem += 20;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(300);
                            mangAnh[i].Enabled = true;
                            mangAnh[j].Enabled = true;
                            mangAnh[i].Image = Image.FromFile(path + "0.png");
                            mangAnh[j].Image = Image.FromFile(path + "0.png");
                            TimeSpan dt = new TimeSpan(0, 0, 0, 1, 0);
                            curTime = curTime.Subtract(dt);
                            if (diem > 0)
                                diem -= 5;
                             
                        }
                        lbDiem.Text = diem.ToString();
                       
                    }
                }
            }
        }


        private void ranPic()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < mangAnh.Length / 2; i++)
            {
                int j = r.Next(1, 16);
                mangAnh[i].Tag = j;
                list.Add(j);
            }
            for (int i = mangAnh.Length / 2; i < mangAnh.Length; i++)
            {
                int x = r.Next(0, list.Count - 1);       // lấy ngẫy nhiên vị trí 1 phần từ trong list
                mangAnh[i].Tag = list[x];
                list.RemoveAt(x);
            }

        }

        private void GameStart()
        {
            curTime = new DateTime(2020, 1, 1, 0, 0, 30);
            Init();
            diem = 0;
            dem = 0;
            count = 0;
            lbDiem.Text = "";
            lbClick.Text = "";
            timer2.Start();
            ranPic();
            //Show_HighScore();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameStart();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đóng ứng dụng?", "Cảnh báo",
                                MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
        }
        int dx = 10;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label2.Left < 0 || label2.Right > ClientRectangle.Width)
                dx = -dx;
            label2.Left += dx;

                
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            TimeSpan dt = new TimeSpan(0, 0, 0, 1, 0);

            
            curTime = curTime.Subtract(dt);
            lbTime.Text = curTime.Second.ToString();

            if (curTime.Second == 0)
            {
                timer2.Enabled = false;
                if (MessageBox.Show("Bạn đã thua!\nBạn muốn chơi lại?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    rePlay();
                else
                {
                    for (int i = 0; i < mangAnh.Length; i++)
                    {
                        mangAnh[i].Visible = false;
                    }
                }    

            }
        }

        private void newGame_Click(object sender, EventArgs e)
        {
            rePlay();
        }

        private void thoatGame_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rePlay()
        {
            // xóa các arrPic đã được tạo bằng cách ẩn nó đi                        // chưa giải phóng được bộ nhớ
            for (int i = 0; i < mangAnh.Length; i++)
            {
                mangAnh[i].Visible = false;
            }


            //pgbTimePlay.Maximum = 0;
            GameStart();
        }










    }


}
