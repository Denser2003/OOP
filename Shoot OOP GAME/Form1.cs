using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Shoot_OOP_GAME
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int enemySpeed = 3;
        int score;
        bool Health = true;
        bool Boss_wake = false;
        int boss_health = 10;
        bool view = true;

        Random randNum = new Random();
        
        List<PictureBox> enemysList = new List<PictureBox>();
        List<PictureBox> bossList = new List<PictureBox>();


        public Form1()
        {
            InitializeComponent();
            RestartGame();
            bossBar.ForeColor = Color.Red;
            ViewElemntChanger(false);
        }
        public void ViewElemntChanger(bool view)
        {
            bossBar.Visible = view;
            BossHP.Visible = view;

        }


        private void MainTimerEvent(object sender, EventArgs e)//Основне события(появление патрон,аптечки и 
        {
            if (playerHealth > 100 && gameOver == false)
            {
                playerHealth = 100;
            }

            if (playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            

            else
            {
                gameOver = true;
                player.Image = Properties.Resources.Death_blood;
                GameTimer.Stop();
            }
            //if (playerHealth < 25 && Health == true)
            //{
            //    Health = true;
            //    DropHelp(Health);
            //    Health = false;


            //}
            if (playerHealth < 25 && Health == true)
            {
                DropHelp();
                Health = false;



            }
            if (score > 1 && Boss_wake == false )
            {
                Boss_wake = true;
                for (int i = 0; i < 1; i++)
                {
                    ViewElemntChanger(true);
                    MakeBoss();
                }

            }
            if (boss_health > 1)
            {
                bossBar.Value = boss_health;
            }


            txtAmmo.Text = "Патроны: " + ammo;
            txtScore.Text = "Убийства: " + score;
           

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }

            if (goUp == true && player.Top > 55)
            {
                player.Top -= speed;
            }
            if (goDown == true && player.Top + player.Height < this.ClientSize.Width)
            {
                player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 10;
                    }
                }
                if (x is PictureBox && (string)x.Tag == "help")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        playerHealth += 50;
                        Health = true;

                    }
                }
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        
                        playerHealth -= 1;
                    }
                }



                if (x is PictureBox && (string)x.Tag == "enemy")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                        
                    }
                    if (x.Left > player.Left)
                    {
                        x.Left -= enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.EnemyLeft;
                    }
                    if (x.Left < player.Left)
                    {
                        x.Left += enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.EnemyRight;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.EnemyDown;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.EnemyUp;
                    }

                }

                if (x is PictureBox && (string)x.Tag == "boss")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;

                    }
                    if (x.Left > player.Left)
                    {
                        x.Left -= enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.boss;
                    }
                    if (x.Left < player.Left)
                    {
                        x.Left += enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.boss;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.boss;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += enemySpeed;
                        ((PictureBox)x).Image = Properties.Resources.boss;
                    }
                    

                }
                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "enemy")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            enemysList.Remove((PictureBox)x);
                            MakeEnemy();

                        }


                    }
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "boss")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score = score + 5;
                            boss_health= boss_health - 1;
                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            if (boss_health < 1)
                            {
                                ((PictureBox)x).Image = Properties.Resources.boom;
                                
                                

                               

                                GameTimer.Stop();
                                MessageBox.Show("Ура!Победа!!!");
                                RestartGame();
                                this.Controls.Remove(x);

                            }

                           

                        }

                    }
                }

            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameOver == true)
            {
                return;
            }
            if (e.KeyCode == Keys.A)
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }
            if (e.KeyCode == Keys.W)
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }

            if (e.KeyCode == Keys.S)
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.A)
            {
                goLeft = false;
              
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = false;
                
            }
            if (e.KeyCode == Keys.W)
            {
                goUp = false;
             
            }

            if (e.KeyCode == Keys.S)
            {
                goDown = false;
               
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootBullet(facing);
                if (ammo < 1)
                {
                    DropAmmo();
                }
                
            }
            //if (e.KeyCode == Keys.E && playerHealth < 25 && gameOver == false && Health == true)
            //{
            //    DropHelp();
            //    Health = false;

                
                

            //}

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
           


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void txtScore_Click(object sender, EventArgs e)
        {

        }

        private void ShootBullet(string direction)
        {
            Bullet shootbullet = new Bullet();
            shootbullet.direction = direction;
            shootbullet.bulletLeft = player.Left + (player.Width / 2);
            shootbullet.bulletTop = player.Top + (player.Height / 2);
            shootbullet.MakeBullet(this);

        }

        private void wall_Click(object sender, EventArgs e)
        {

        }

        private void MakeEnemy()
        {
            PictureBox enemy = new PictureBox();
            enemy.Tag = "enemy";
            enemy.Image = Properties.Resources.edown;
            enemy.Left = randNum.Next(0, 1339);
            enemy.Top = randNum.Next(0, 852);
            enemy.SizeMode = PictureBoxSizeMode.AutoSize;
            enemysList.Add(enemy);

            this.Controls.Add(enemy);
            player.BringToFront();


        }

        private void bossBar_Click(object sender, EventArgs e)
        {

        }
        
        private void MakeBoss()
        {
            PictureBox boss = new PictureBox();

            boss.Tag = "boss";
            boss.Image = Properties.Resources.boss;
            boss.Left = randNum.Next(0, 1339);
            boss.Top = randNum.Next(0, 852);
            boss.SizeMode = PictureBoxSizeMode.AutoSize;
            bossList.Add(boss);

            this.Controls.Add(boss);
            player.BringToFront();
            foreach (Control x in this.Controls)
            {
               
                if (x is PictureBox && (string)x.Tag == "enemy")
                {
                    ((PictureBox)x).Dispose();

                }
            }




        }

        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_bullet_;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();

           
        }
        private void DropHelp()
        {
           
                PictureBox help = new PictureBox();
                help.Image = Properties.Resources.help;
                help.SizeMode = PictureBoxSizeMode.AutoSize;
                help.Left = randNum.Next(10, this.ClientSize.Width - help.Width);
                help.Top = randNum.Next(60, this.ClientSize.Height - help.Height);
                help.Tag = "help";
                this.Controls.Add(help);

                help.BringToFront();
                player.BringToFront();
                

        }

        private void RestartGame()
        {
            ViewElemntChanger(false);
            player.Image = Properties.Resources.up;
            foreach (PictureBox i in enemysList)
            {
                this.Controls.Remove(i);
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {   
                        ((PictureBox)x).Dispose();
                   
                }
                if (x is PictureBox && (string)x.Tag == "help")
                {
                    ((PictureBox)x).Dispose();

                }
                if (x is PictureBox && (string)x.Tag == "boss")
                {
                    ((PictureBox)x).Dispose();

                }
            }
            boss_health = 5;
            Health = true;
            Boss_wake = false;
            enemysList.Clear();

            for (int i = 0; i < 3; i++)
            {
                MakeEnemy();
            }
            
            goUp = false;
            goRight = false;
            goDown = false;
            goLeft = false;
            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammo = 10;
            GameTimer.Start();
        }
    }
}
