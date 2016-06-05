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

namespace ThreadWineFrom
{
    public partial class Form1 : Form
    {
        private int nowNumber=0;
        private bool stopTage = false;
        private Thread newThread;


        private delegate void addNumberDelegate();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if ( null == this.newThread)
            {
                newThread = new Thread(new ThreadStart(addNumber));
                stopTage = false;
                newThread.Start();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void addNumber()
        {
            if ( this.InvokeRequired )
            {
                addNumberDelegate ad = addNumber;
                this.Invoke(ad);
            }
            else
	        {
                while ( false == this.stopTage )
                {
                    System.Threading.Thread.Sleep(100);
                    nowNumber++;
                    this.label1.Text = nowNumber.ToString();
                    Application.DoEvents();
                }

	        }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if ( null!=this.newThread )
            {
                if ( false == this.stopTage)
                {
                    this.stopTage = true;
                    this.newThread.Abort();
                    this.newThread = null;
                }
            }
        }

        private void button_toZero_Click(object sender, EventArgs e)
        {
            if ( this.stopTage == true )
            {
                this.nowNumber = 0;
                this.label1.Text = this.nowNumber.ToString();
                Application.DoEvents();
            }
        }
    }
}
