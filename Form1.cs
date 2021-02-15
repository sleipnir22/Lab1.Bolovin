using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1.Bolovin
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> table = new Dictionary<CheckBox, Cell>();
        decimal day = 0;
        int money = 10;
        int speed = 0;

        public Form1()
        {
            InitializeComponent();
            foreach (CheckBox cb in panel.Controls)
                table.Add(cb, new Cell());
            label2.Text = money.ToString();
            speed = trackBar1.Value;
            label5.Text = trackBar1.Value.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (sender as CheckBox);
            var state = table[cb].state;
            if (money > 0)
                if (cb.Checked) Plant(cb);
                else Harvest(cb);
            else if (state == CellState.Planted || state == CellState.Overgrow || state == CellState.Empty)
                MessageBox.Show("You don't have enough money!");
            else Harvest(cb);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (CheckBox cb in panel.Controls)
            {
                NextStep(cb);
            }
            label1.Text = day.ToString() + " days";
            label2.Text = money.ToString() + " dollars";
            day += 0.5M;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            speed = (sender as TrackBar).Value;
            label5.Text = speed.ToString();
            timer1.Interval = 100 / speed;
        }

        private void Plant(CheckBox cb)
        {
            table[cb].Plant();
            UpdateCell(cb);
            if (money > 1)
                money -= 2;
            else MessageBox.Show("You don't have enough money!");
        }

        private void Harvest(CheckBox cb)
        {
            UpdateMoney(cb);
            table[cb].Harvest();
            UpdateCell(cb);
        }

        private void NextStep(CheckBox cb)
        {
            table[cb].NextStep();
            UpdateCell(cb);
        }

        private void UpdateCell(CheckBox cb)
        {
            Color color = Color.White;
            switch (table[cb].state)
            {
                case CellState.Planted:
                    color = Color.Black;
                    break;
                case CellState.Green:
                    color = Color.Green;
                    break;
                case CellState.Immature:
                    color = Color.Yellow;
                    break;
                case CellState.Mature:
                    color = Color.Red;
                    break;
                case CellState.Overgrow:
                    color = Color.Brown;
                    break;
            }
            cb.BackColor = color;
        }
        private void UpdateMoney(CheckBox cb)
        {
            switch (table[cb].state)
            {
                case CellState.Planted:
                    if (money < 2)
                        MessageBox.Show("You don't have enough money!");
                    else
                        money -= 2;
                    break;
                case CellState.Green:
                    break;
                case CellState.Immature:
                    money += 3;
                    break;
                case CellState.Mature:
                    money += 5;
                    break;
                case CellState.Overgrow:
                    if (money < 1)
                        MessageBox.Show("You don't have enough money!");
                    else
                        money -= 1;
                    break;
            }
        }

    }

    enum CellState
    {
        Empty,
        Planted,
        Green,
        Immature,
        Mature,
        Overgrow
    }
    class Cell
    {
        public CellState state = CellState.Empty;
        public int progress = 0;

        private int prPlanted = 20;
        private int prGreen = 70;
        private int prImmature = 100;
        private int prMature = 150;
        public void Plant()
        {
            state = CellState.Planted;
            progress = 1;
        }

        public void Harvest()
        {
            state = CellState.Empty;
            progress = 0;
        }

        public void NextStep()
        {
            if ((state != CellState.Empty) && (state != CellState.Overgrow))
            {
                progress++;
                if (progress < prPlanted) state = CellState.Planted;
                else if (progress < prGreen) state = CellState.Green;
                else if (progress < prImmature) state = CellState.Immature;
                else if (progress < prMature) state = CellState.Mature;
                else state = CellState.Overgrow;
            }
        }
    }
}
