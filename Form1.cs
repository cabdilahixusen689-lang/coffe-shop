using System;
using System.IO;
using System.Windows.Forms;

namespace cofeProject
{
    public partial class Form1 : Form
    {
        private readonly Random Rand = new Random();

        public Form1()
        {
            InitializeComponent();

            // Ku xidh button kasta event-kiisa
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            button4.Click += button4_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter customer name.");
                return;
            }

            if (!int.TryParse(textBox2.Text, out int quantity) || quantity < 1)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            double drinkPrice;
            string drinkName;

            if (radioButton1.Checked)
            {
                drinkPrice = 1;
                drinkName = "Tea";
            }
            else if (radioButton2.Checked)
            {
                drinkPrice = 2;
                drinkName = "Coffee";
            }
            else if (radioButton3.Checked)
            {
                drinkPrice = 3;
                drinkName = "Juice";
            }
            else
            {
                MessageBox.Show("Please select a drink.");
                return;
            }

            listBox1.Items.Clear();

            double totalBeforeDiscount = drinkPrice * quantity;
            double memberDiscount = checkBox1.Checked
                ? totalBeforeDiscount * 0.10
                : 0;
            double totalAfterDiscount = totalBeforeDiscount - memberDiscount;

            bool hasPrize = false;
            int winningLuckyNumber = 0;

            for (int i = 1; i <= quantity; i++)
            {
                int luckyNumber = Rand.Next(1, 51);

                string order = i + ") " + textBox1.Text.Trim() + " - " +
                               drinkName + " - Lucky: " + luckyNumber +
                               " - $" + drinkPrice.ToString("0.00");

                listBox1.Items.Add(order);

                if (luckyNumber == 7 || luckyNumber == 17)
                {
                    hasPrize = true;
                    winningLuckyNumber = luckyNumber;
                }
            }

            string luckyMessage;
            string listLuckyMessage;

            if (hasPrize)
            {
                luckyMessage = "You got a Free Cookie!";
                listLuckyMessage = "You get a Free Cookie! (Lucky " +
                                   winningLuckyNumber + ")";
            }
            else
            {
                luckyMessage = "No Prize this time.";
                listLuckyMessage = "No Prize this time.";
            }

            listBox1.Items.Add("");

            if (checkBox1.Checked)
            {
                listBox1.Items.Add("Member Discount: 10%");
            }

            listBox1.Items.Add("Total Before Discount: $" +
                               totalBeforeDiscount.ToString("0.00"));
            listBox1.Items.Add("Total After Discount: $" +
                               totalAfterDiscount.ToString("0.00"));
            listBox1.Items.Add("Lucky Message: " + listLuckyMessage);

            label1.Text = "Total Price: $" + totalAfterDiscount.ToString("0.00");
            label2.Text = "Lucky Message: " + luckyMessage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("No order to save!");
                return;
            }

            try
            {
                using (StreamWriter writer = File.AppendText("orders.txt"))
                {
                    foreach (object item in listBox1.Items)
                    {
                        writer.WriteLine(item?.ToString() ?? "");
                    }

                    writer.WriteLine();
                }

                MessageBox.Show("Order saved!");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Could not save the order: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!File.Exists("orders.txt"))
            {
                MessageBox.Show("No file found!");
                return;
            }

            try
            {
                listBox1.Items.Clear();

                using (StreamReader reader = File.OpenText("orders.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        listBox1.Items.Add(reader.ReadLine());
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Could not load the orders: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            listBox1.Items.Clear();

            label1.Text = "Total Price:";
            label2.Text = "Lucky Message:";

            checkBox1.Checked = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }
    }
}