using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Janet_sWebFormApplication
{
    public partial class Form1 : Form
    {
        private Thread thread;
        public Form1()
        {
            InitializeComponent();
        }
        Boolean keepRunning = true;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Boolean GetUpdateValue(Boolean keepRunning = true)
        {
            if (keepRunning)
            {
                return this.keepRunning;
            }
            else
            {
                return (this.keepRunning = keepRunning);
            }
        }

        delegate void delUpdateLabel(String text);

        delegate void HandleException(Exception ex);


        void UpdateLabel(String text)
        {

            if (InvokeRequired)
            {
                this.Invoke(new delUpdateLabel(UpdateLabel), new Object[] { text });
            }
            else
            {
                this.lblNumber.Text = text;
            }
        }

        delegate void delHandleException(Exception ex);


        void HandleException(Exception ex)
        {

        }

        public void doLongWork()
        {
            for (int i = 0; i < 1000000; i++)
            {
                for (int j = 0; j < 1000000; j++)
                {
                    for (int k = 0; k < 1000000; k++)
                    {
                        for (int l = 0; l < 1000000; l++)
                        {
                            if (this.GetUpdateValue())
                            {
                                Random rand = new Random();
                                int r = rand.Next();
                                //if (r % 10 == 0)
                                {
                                    this.UpdateLabel(r.ToString());
                                }

                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(() => doLongWork()));
            thread.Start();
            this.button1.Enabled = false;
            this.button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.GetUpdateValue(false);
            thread.Abort();

            this.button1.Enabled = true;
            this.button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.GetUpdateValue(false);
        }
    }
}
