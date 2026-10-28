namespace Battle
{
    public partial class FormMain : Form
    {
        int dx = -5;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            PlaceCannon();
            InitGame();
        }

        void PlaceCannon()
        {
            pictureBoxCanon.Location = new Point(
                (ClientSize.Width - pictureBoxCanon.Width) / 2,
                ClientSize.Height - pictureBoxCanon.Height);

            PlaceCanonBall();
        }

        void PlaceCanonBall()
        {
            pictureBoxCanonBall.Location = new Point(
                pictureBoxCanon.Left + (pictureBoxCanon.Width - pictureBoxCanonBall.Width) / 2,
                pictureBoxCanon.Top - pictureBoxCanonBall.Height / 2);
        }

        void InitGame()
        {
//Задание 1: fullscreen режим, пушка, ядро переместить в низ в центр
            pictureBoxCanonBall.Visible = false;
            pictureBoxBoom.Visible = false;
            PlaceCanonBall();
            pictureBoxShip.Location = new Point(ClientSize.Width - pictureBoxShip.Width, 2);
            pictureBoxShip.Visible = true;
            timerCanonBall.Enabled = false;
            timerShip.Enabled = true;
        }

        private void timerShip_Tick(object sender, EventArgs e)
        {
            pictureBoxShip.Location = new Point(pictureBoxShip.Location.X + dx, pictureBoxShip.Location.Y);
            if (pictureBoxShip.Location.X < 0 ||
                    pictureBoxShip.Location.X > ClientSize.Width - pictureBoxShip.Width) dx = -dx;
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == ' ')
            {
// Задание 2: если летит ядро, новое не разрешать выпускать
                if (timerCanonBall.Enabled) return;

                PlaceCanonBall();
                pictureBoxCanonBall.Visible = true;
                timerCanonBall.Enabled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private void timerCanonBall_Tick(object sender, EventArgs e)
        {
            pictureBoxCanonBall.Location = new Point(pictureBoxCanonBall.Location.X,
                                                        pictureBoxCanonBall.Location.Y - 5);

//Задание 3: точность попадания - сравниваем центры ядра и кораблm
            Point ballCenter = new Point(
                pictureBoxCanonBall.Left + pictureBoxCanonBall.Width / 2,
                pictureBoxCanonBall.Top + pictureBoxCanonBall.Height / 2);
            Point shipCenter = new Point(
                pictureBoxShip.Left + pictureBoxShip.Width / 2,
                pictureBoxShip.Top + pictureBoxShip.Height / 2);

            if (Distance(ballCenter, shipCenter) < pictureBoxShip.Width / 2)
            {
                pictureBoxBoom.Location = pictureBoxShip.Location;
                pictureBoxShip.Visible = false;
                pictureBoxCanonBall.Visible = false;
                pictureBoxBoom.Visible = true;
                timerCanonBall.Enabled = false;
                timerShip.Enabled = false;

                if (MessageBox.Show("Хотите еще сыграть?", "Цель поражена", MessageBoxButtons.YesNo) ==
                      DialogResult.Yes) InitGame();
                else Close();
            }
            if (pictureBoxCanonBall.Location.Y < -pictureBoxCanonBall.Height)
            {
                pictureBoxCanonBall.Visible = false;
                timerCanonBall.Enabled = false;
            }
        }
    }
}