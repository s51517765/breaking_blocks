using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ブロックくずし
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int playerSize_X = 75;
        const int playerPosition_Y = 430;
        double ball_Speed = 8;

        double ball_Direction = Math.PI / 4;
        int Form1_x = 0;
        int Cursol_X = 0;
        bool hitting = false; //打ち返し中

        bool[] Block_flag = { true, true, true, true, true, true, true, true, true, true, true };  //ブロックが残っているか

        private void buttonPlayer_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Form1_x = this.Left; //Form1の位置
            Cursol_X = MousePosition.X - Form1_x - playerSize_X / 2;
            buttonPlayer.Left = Cursol_X;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonPlayer.Location = new Point(0, playerPosition_Y);
            labelBall.Location = new Point(50, 50);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //ボールの進行角度を 0～2*PIに正規化
            if (ball_Direction < 0) ball_Direction += Math.PI * 2;
            else if (ball_Direction > Math.PI * 2) ball_Direction -= Math.PI * 2;

            int ball_X = labelBall.Left;　//ボールの位置を取得
            int ball_Y = labelBall.Top;

            //横壁との反射
            if (ball_X < 0 || this.Width - 30 < ball_X)
            {
                ball_Direction = 2 * Math.PI - ball_Direction;
            }

            //縦壁との反射
            if (ball_Y < 0 || this.Height - 60 < ball_Y)
            {
                ball_Direction = Math.PI - ball_Direction;
            }

            //ブロック当たり判定
            if (ball_Y < 30) attack(ball_X, ball_Y);

            //パドル当たり判定
            if (Math.Abs(playerPosition_Y - ball_Y) < ball_Speed && hitting == false) //高さ方向を限定&&打ち返し中でない
            {
                int center_distance = Cursol_X + playerSize_X / 2 - ball_X;
                if (Math.Abs(center_distance) <= 5) //中心
                {
                    ball_Direction = Math.PI - ball_Direction;
                }
                else if (5 < center_distance && center_distance < 25) //端っこに当たったら反射角度を変える
                {
                    if (ball_Direction > Math.PI * 0.125 && Math.PI * 1.875 > ball_Direction) ball_Direction += Math.PI * 0.95;
                }
                else if (25 <= center_distance && center_distance < 50) //端っこに当たったら反射角度を変える
                {
                    if (ball_Direction > Math.PI * 0.125 && Math.PI * 1.875 > ball_Direction) ball_Direction += Math.PI * 1.03;
                }

                hitting = true;　//打ち返し中フラグ
            }
            else if (Math.Abs(playerPosition_Y - ball_Y) > ball_Speed && hitting == true)　//打ち返し中を抜けたら
            {
                hitting = false;
            }
            labelBall.Left += (int)(ball_Speed * Math.Sin(ball_Direction));
            labelBall.Top += (int)(ball_Speed * Math.Cos(ball_Direction));
            //   label1.Text = ((int)(ball_Speed * Math.Sin(ball_Direction))).ToString() + " , " + ((int)(ball_Speed * Math.Cos(ball_Direction))).ToString() + " , " + ball_Direction.ToString();

            Application.DoEvents(); //画面更新その他をすべて処理
        }
        private void game_end()
        {
            timer1.Stop();
            MessageBox.Show("Game Clear!");
            buttonStart.Enabled = false;
        }
        private void attack(int ball_x, int ball_y)
        {
            int count = 0;

            for (int i = 0; i < Block_flag.Length; i++)
            {
                if (Block_flag[i]) count++;
            }

            int block_no = ball_x / 80;

            if (ball_y < 30)
            {
                if (Block_flag[block_no])
                {
                    Block_flag[block_no] = false;

                    switch (block_no)
                    {
                        case 0:
                            button0.Visible = false;
                            break;
                        case 1:
                            button1.Visible = false;
                            break;
                        case 2:
                            button2.Visible = false;
                            break;
                        case 3:
                            button3.Visible = false;
                            break;
                        case 4:
                            button4.Visible = false;
                            break;
                        case 5:
                            button5.Visible = false;
                            break;
                        case 6:
                            button6.Visible = false;
                            break;
                        case 7:
                            button7.Visible = false;
                            break;
                        case 8:
                            button8.Visible = false;
                            break;
                        case 9:
                            button9.Visible = false;
                            break;
                        case 10:
                            button10.Visible = false;
                            break;
                    }
                    if (count == 1) //残り一個でブロックを消したとき
                    {
                        game_end();
                    }

                    ball_Direction = Math.PI - ball_Direction;

                }
            }
        }


        private void buttonSpeedUp_Click(object sender, EventArgs e)
        {
            ball_Speed *= 1.2;
        }

        private void buttonSpeedDown_Click(object sender, EventArgs e)
        {
            ball_Speed /= 1.2;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
                buttonStart.Text = "Stop";
                buttonStart.ForeColor = Color.Red;
            }
            else
            {
                timer1.Stop();
                buttonStart.Text = "Start";
                buttonStart.ForeColor = Color.Black;
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            labelBall.Left = 50;
            labelBall.Top = 50;
            ball_Direction = Math.PI / 4;
            button0.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            button10.Visible = true;

            buttonStart.Enabled = true;

            for (int i = 0; i < Block_flag.Length; i++) Block_flag[i] = true;
        }
    }
}
