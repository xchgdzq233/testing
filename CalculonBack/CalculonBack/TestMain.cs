using CalculonBack.NodeClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CalculonBack
{
    public partial class TestMain : Form
    {
        private static TestMain _instance;

        private TestMain()
        {
            InitializeComponent();

            lblTopic.Text = Document.GetInstance().GetTopic();
            LoadDocument(Document.GetInstance().GetParagraphs());
        }

        public static TestMain getInstance()
        {
            if (Object.ReferenceEquals(null, _instance))
                _instance = new TestMain();
            return _instance;
        }

        public void LoadDocument(List<String> paragraphs)
        {
            TextBox lastTextBox = null;

            for (int i = 0; i < paragraphs.Count; i++)
            {
                TextBox txtBox = new TextBox();

                //set layout
                txtBox.Name = "paragraph" + (i + 1).ToString();
                txtBox.Font = new Font("Microsoft Sans Serif", 9);
                txtBox.Multiline = true;

                TextFormatFlags flags = TextFormatFlags.WordBreak;
                Size sz = new Size(744, int.MaxValue);
                sz = TextRenderer.MeasureText(paragraphs[i], txtBox.Font, sz, flags);
                txtBox.Size = new Size(744, sz.Height + 10);
                Console.WriteLine(sz.Height);

                int y = 10 + (i > 0 ? lastTextBox.Location.Y + lastTextBox.Size.Height : 40);
                txtBox.Location = new Point(20, y);

                //set value
                txtBox.Text = paragraphs[i];
                txtBox.Tag = i;
                txtBox.Click += txtBoxParagraph_Click;

                this.Controls.Add(txtBox);
                    lastTextBox = txtBox;
            }
        }

        private void txtBoxParagraph_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Paragra No. " + ((TextBox)sender).Tag.ToString());
        }
    }
}
