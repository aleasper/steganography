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


            Console.WriteLine(projectDirectory);
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = string.Format("{0}\\venv\\Scripts\\python.exe", projectDirectory);
            start.Arguments = string.Format("{0}\\main.py", projectDirectory) +
                string.Format(
                    " --input {0} --output {1} --text_file {2} --encode",
                    this.file, this.encodedFile, this.txtFile
                );
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
                Console.Write(process.ExitCode);
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
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
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
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
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
                Console.WriteLine(size); // <-- Shows file size in debugging mode.
                Console.WriteLine(result); // <-- For debugging use.
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


            Console.WriteLine(projectDirectory);
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = string.Format("{0}\\venv\\Scripts\\python.exe", projectDirectory);
            start.Arguments = string.Format("{0}\\main.py", projectDirectory) +
                string.Format(
                    " --input {0} --text_file {1}",
                    this.toDecodeFile, this.decodedText
                );
            Console.WriteLine(start.Arguments);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
                Console.Write(process.ExitCode);
            }
        }
    }
}
