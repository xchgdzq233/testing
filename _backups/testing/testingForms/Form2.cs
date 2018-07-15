using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testingForms
{
    public partial class Form2 : Form
    {
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken cancelToken;
        //private readonly AsyncLock  

        public Form2()
        {
            InitializeComponent();

            cancelTokenSource = new CancellationTokenSource();
            cancelToken = cancelTokenSource.Token;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task t1 = new Task(() => ranNum());
            t1.Start();
        }

        private void ranNum()
        {
            while(true)
            {
                Random r = new Random();
                label1.Text = r.Next().ToString();
                if (cancelToken.IsCancellationRequested)
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cancelTokenSource.Cancel();
        }
    }
}
