using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace steganography_ui
{
    public partial class Form1 : Form
    {


        string file;
        string txtFile;
        string encodedFile;
        string toDecodeFile;
        string decodedText;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * ENCODE
             */
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = workingDirectory;

            int degree = (int)this.numericUpDown1.Value;

            long length = new System.IO.FileInfo(this.file).Length - 54;
            long textL = new System.IO.FileInfo(this.txtFile).Length;
            if (length * degree / 16 < textL) {
                Console.WriteLine("!!!!!!");
                MessageBox.Show("Слишком длинный текст: попробуйте увеличить кол-во перезаписываемых бит");
                return;
            }
            // wav_data_len * degree / 16

            Console.WriteLine(length);

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = string.Format("{0}\\venv\\Scripts\\python.exe", projectDirectory);
            start.Arguments = string.Format("{0}\\main.py", projectDirectory) +
                string.Format(
                    " --input {0} --output {1} --text_file {2} --degree {3} --encode",
                    this.file, this.encodedFile, this.txtFile, degree
                );
            Console.WriteLine(start.Arguments);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                try
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        Console.WriteLine("Res: ", result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Audio files |*.wav";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                this.file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(this.file);
                    size = text.Length;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text files | *.txt";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                this.txtFile = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Audio files |*.wav";
            saveFileDialog1.Title = "Выберите куда сохранить файл";
            saveFileDialog1.ShowDialog();

            this.encodedFile = saveFileDialog1.FileName;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int size = -1;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Audio files | *.wav";
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    this.toDecodeFile = openFileDialog1.FileName;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text files |*.txt";
            saveFileDialog1.Title = "Выберите куда сохранить файл";
            saveFileDialog1.ShowDialog();

            this.decodedText = saveFileDialog1.FileName;
            Console.WriteLine(this.decodedText);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            /*
            * DECODE
            */
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = workingDirectory;

            int degree = (int)this.numericUpDown2.Value;

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = string.Format("{0}\\venv\\Scripts\\python.exe", projectDirectory);
            start.Arguments = string.Format("{0}\\main.py", projectDirectory) +
                string.Format(
                    " --input {0} --text_file {1} --degree {2}",
                    this.toDecodeFile, this.decodedText, degree
                );
            Console.WriteLine(start.Arguments);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }
    }
}
