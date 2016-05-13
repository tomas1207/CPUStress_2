using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;

namespace CPUStress
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        public static void CPUKill(object cpuUsage)
        {
            Parallel.For(0, 1, new Action<int>((int i) =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (true)
                {
                    if (watch.ElapsedMilliseconds > (int)cpuUsage)
                    {
                        Thread.Sleep(100 - (int)cpuUsage);
                        watch.Reset();
                        watch.Start();
                    }
                }
            })
            );

        }


        private void test()
        {
            int time = Int32.Parse(textBox1.Text) * 1000;
            int cpuUsageIncreaseby = 100;
            while (true)
            {
              
                for (int i = 0; i < Environment.ProcessorCount ; i++)
                {
                    int cpuUsage = cpuUsageIncreaseby;
              
            
                    List<Thread> threads = new List<Thread>();
                    for (int j = 0; j < Environment.ProcessorCount; j++)
                    {
                        Thread t = new Thread(new ParameterizedThreadStart(CPUKill));
                        t.Start(cpuUsage);
                        threads.Add(t);
                    }
                    Thread.Sleep(time);
                    foreach (var t in threads)
                    {
                        t.Abort();
                    }
   
                    Application.Exit();
           
                    break;
                }
                break;
            }
        }
        



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.backgroundWorker1.CancellationPending == true)
            {
                e.Cancel = true;
                return;

            }
            else
            {
                test();

                backgroundWorker1.WorkerSupportsCancellation = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
             backgroundWorker1.RunWorkerAsync();         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBox1.Text.Remove(textBox1.Text.Length - 1);
                textBox1.Text = "";
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Start();
            label2.Text = "CPU Usage: " + Cpu();

        }
        private string Cpu() {
            return performanceCounter1.NextValue()+ " %";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = "CPU Usage: "+ Cpu();
            
        }
    }
}
