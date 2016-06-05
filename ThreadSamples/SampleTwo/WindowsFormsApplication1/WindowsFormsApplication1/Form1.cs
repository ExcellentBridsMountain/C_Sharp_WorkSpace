using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private int nowNumber = 0;

        private bool tagbool = false;

        private bool autoTag = true; // Thread Pause and Continue Tag

        private delegate void testDelegate();

        private Thread timeThread;

        private AutoResetEvent autoResetEvent = new AutoResetEvent(true);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( null == this.timeThread)
            {
                Thread newThread = new Thread(new ThreadStart(testMethod));
                newThread.IsBackground = true;
                newThread.Start();
                newThread.Name = "TimerThread";
                this.timeThread = newThread;
                this.button1.Text = "To Zero";
            }
            else
            {
                lock (this)
                {
                    this.nowNumber = 0;

                    if ( true == this.tagbool) // if Pause
                    {
                        this.button2_Click(sender, e);
                    }
                }
                //MessageBox.Show("Is Running");
            }

        }

        private void testMethod()
        {
            if ( this.InvokeRequired ) // In Thread which is not UI thread
            {
                testDelegate testd = testMethod;
                while (true)
                {
                    if (false == autoTag)
                    {
                        autoResetEvent.WaitOne(); // Pause Thread (No.2 thread TimerThread")
                        autoTag = true;
                    }

                    Thread.Sleep(100);
                    this.Invoke(testd);
                    this.nowNumber++;
                }
            }
            else // In UI thread
            {
                this.label1.Text = this.nowNumber.ToString();
                Application.DoEvents(); // refresh Application UI
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ( false == this.tagbool)
            {
                this.tagbool = true;
                this.autoResetEvent.Reset(); // Pause Thread (No.2 thread TimerThread")
                this.autoTag = false; 
                this.button2.Text = "Continue";
                return;
            }
            if (true == this.tagbool )
            {
                this.tagbool = false;
                this.autoResetEvent.Set();
                this.autoTag = false;
                this.button2.Text = "Pause";
                return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
