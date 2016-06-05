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

        private bool autoTag = true;

        private delegate void testDelegate();

        private Thread timeThread;

        private AutoResetEvent autoResetEvent = new AutoResetEvent(true);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(testMethod));
            newThread.IsBackground = true;
            newThread.Start();
            this.timeThread = newThread;
        }

        private void testMethod()
        {
            if ( this.InvokeRequired )
            {
                testDelegate testd = testMethod;
                for (int i = 0; i < 100; i++)
                {
                    if (false == autoTag)
                    {
                        autoResetEvent.WaitOne();
                        autoTag = true;
                    }
                    Thread.Sleep(100);
                    this.nowNumber = i;
                    this.Invoke(testd);
                }

            }
            else
            {
                this.label1.Text = this.nowNumber.ToString();
                Application.DoEvents();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ( false == this.tagbool)
            {
                this.tagbool = true;
                this.autoResetEvent.Set();
                this.autoTag = false;
                return;
            }

            if (true == this.tagbool )
            {
                this.tagbool = false;
                this.autoResetEvent.Reset();
                this.autoTag = false;
                return;
            }
        }
    }
}
