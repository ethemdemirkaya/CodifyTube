using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodifyTube
{
    public partial class Form1 : Form
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private const int DWMWA_CAPTION_COLOR = 35;
        private Form activeForm = null;
        public Form1()
        {
            InitializeComponent();
            
        }
        private void SetCaptionColor(Color color)
        {
            int colorRef = ColorTranslator.ToWin32(color);
            DwmSetWindowAttribute(this.Handle, DWMWA_CAPTION_COLOR, ref colorRef, sizeof(int));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetCaptionColor(Color.FromArgb(217, 4, 41));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string url = "https://instagram.com/ethemdmrky_";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/ethemdemirkaya";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelchildForm.Controls.Add(childForm);
            panelchildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void closeChildForm()
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            Mp4Converter frmMp4 = new Mp4Converter();
            openChildForm(frmMp4);
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            closeChildForm();
        }

        private void bunifuTileButton2_Click(object sender, EventArgs e)
        {
            Mp3Converter frmMp3 = new Mp3Converter();
            openChildForm(frmMp3);
        }
    }
}
