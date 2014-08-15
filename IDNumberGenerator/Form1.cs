using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IDNumberGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerateID_Click(object sender, EventArgs c)
        {
            tbIDNumber.Text = GenerateID();
        }
        
        private bool CheckIsEmpty(string Str)
        {
            if (Str.Length == 0)
                return true;
            else
                return false;
        }

        private bool CheckLengthIsTwoChars(string Str)
        {
            if (Str.Length == 2)
                return true;
            else
                return false;
        }

        private string GenerateID()
        {
            string Year, Month, Day, Gender, Citizen, A, SSS, Digit13;
            Random ran = new Random();

            if (cbYear.Text.Length != 0)
                Year = cbYear.Text;
            else
                Year = ran.Next(0, 100).ToString("D2");

            if ((cbMonth.Text.Length != 0))
                Month = cbMonth.Text;
            else
                Month = ran.Next(1, 13).ToString("D2");

            if (cbDay.Text.Length != 0)
                Day = cbDay.Text;
            else
                Day = ran.Next(1, GetNumberOfDaysInMonth(Month)).ToString("D2");

            if ((cbGender.Text.Length != 0) && (cbGender.Text == "Male"))
                Gender = "5";
            else if ((cbGender.Text.Length != 0) && (cbGender.Text == "Female"))
                Gender = "4";
            else
                Gender = ran.Next(4, 6).ToString();

            SSS = ran.Next(1, 1000).ToString("D3");
            Citizen = ran.Next(0, 2).ToString();
            A = ran.Next(8, 10).ToString();

            char[] Array = (Year + Month + Day + Gender + SSS + Citizen + A).ToCharArray(0, 12);

            int OddSum = 0;

            for (int i = 0; i < Array.Length; i++)
            {
                OddSum = OddSum + int.Parse(Array[i].ToString());
                i++;
            }

            string EvenConcat = "";
            for (int j = 1; j < Array.Length; j++)
            {
                EvenConcat = EvenConcat + Array[j].ToString();
                j++;
            }

            int DoubleEvens = int.Parse(EvenConcat) * 2;

            char[] EvensArray = DoubleEvens.ToString().ToCharArray();
            int SumDoubleEvens = 0;
            for (int k = 0; k < EvensArray.Length; k++)
            {
                SumDoubleEvens = SumDoubleEvens + int.Parse(EvensArray[k].ToString());
            }

            int Total = OddSum + SumDoubleEvens;
            int LastDigit = int.Parse(Total.ToString().Substring(Total.ToString().Length - 1));

            if (LastDigit % 10 == 0)
                Digit13 = "0";
            else
                Digit13 = (10 - LastDigit).ToString();

            return Year + Month + Day + Gender + SSS + Citizen + A + Digit13;
        }

        private int GetNumberOfDaysInMonth(string month)
        {
            int Month = Convert.ToInt16(month);
            switch (Month)
            {
                case 2:
                    return 28;
                case 4:
                    return 30;
                case 6:
                    return 30;
                case 9:
                    return 30;
                case 11:
                    return 30;
                default:
                    return 31;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbYear.Text = "";
            cbMonth.Text = "";
            cbDay.Text = "";
            cbGender.Text = "";
        }

        private void btnGenerateCopy_Click(object sender, EventArgs e)
        {
            tbIDNumber.Text = GenerateID();
            Clipboard.SetText(tbIDNumber.Text);
        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMonth.Text.Length != 0)
            {
                int Month = Convert.ToInt16(cbMonth.Text);

                if (Month == 2)
                {
                    cbDay.Items.Clear();
                    string[] Days = new string[] { "", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28" };
                    cbDay.Items.AddRange(Days);
                }
                else if ((Month == 4) || (Month == 6) || (Month == 9) || (Month == 11))
                {
                    cbDay.Items.Clear();
                    string[] Days = new string[] { "", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
                    cbDay.Items.AddRange(Days);
                }
                else
                {
                    cbDay.Items.Clear();
                    string[] Days = new string[] { "", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                    cbDay.Items.AddRange(Days);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = saveFile.FileName;
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if ((ValidateExportCountField() == true) && (ValidateFilePathField() == true))
            {
                int ExportCount = int.Parse(tbExportCount.Text);
                string[] ExportArray = new string[ExportCount];
                pbProgress.Value = 0;
                pbProgress.Minimum = 0;
                pbProgress.Maximum = ExportCount;
                pbProgress.Value = 0;
                pbProgress.Step = 1;

                for (int i = 0; i < ExportCount; i++)
                {
                    ExportArray[i] = GenerateID();
                    while ((i != 0) && (ExportArray[i] == ExportArray[i - 1]))
                    {
                        ExportArray[i] = GenerateID();
                    }
                    pbProgress.PerformStep();

                }

                try
                {
                    System.IO.File.WriteAllLines(tbFilePath.Text, ExportArray);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
            else
                MessageBox.Show("Please complete both fields correctly.");
        }

        private bool ValidateExportCountField()
        {
            try
            {
                int x = int.Parse(tbExportCount.Text);
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private bool ValidateFilePathField()
        {
            if (tbFilePath.Text == "")
                return false;
            else
                return true;
        }
     
    }

}
