using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                richTextBox1.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                textBox1.Visible = true;
                textBox1.Text = "10";
                label2.Visible = true;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                richTextBox1.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                label2.Visible = true;
                textBox1.Visible = true;
                textBox1.Text = "ABC";
            }
            if (comboBox1.SelectedIndex == 2)
            {
                richTextBox1.Text = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
                label2.Visible = false;
                textBox1.Visible = false;
            }
            if(comboBox1.SelectedIndex == 3)
            {
                textBox1.Text = "6,12,22,5,3";
                richTextBox1.Text = "ДЬУЙЬДЭУЙРЧЯТЩЬ";
                label2.Visible = true;
                textBox1.Visible = true;
            }
            if (comboBox1.SelectedIndex == 4)
            {
                richTextBox1.Text = "13 34 22 24 44 34 15 42 22 34 43 45 32";
                textBox1.Visible = false; 
                label2.Visible = false;
            }
            if (comboBox1.SelectedIndex == 5)
            {
                richTextBox1.Text = "МЫЩАЛ ЧОСОШ ЫЧПИЕК";
                textBox1.Visible = false; 
                label2.Visible = false;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                    richTextBox2.Text = Cesarus(richTextBox1.Text, int.Parse(textBox1.Text));
                if (comboBox1.SelectedIndex == 1)
                    richTextBox2.Text = Vigenere(richTextBox1.Text, textBox1.Text).ToUpper();
                if (comboBox1.SelectedIndex == 2)
                    richTextBox2.Text = Rot13Rus(richTextBox1.Text);
                if (comboBox1.SelectedIndex == 3)
                    richTextBox2.Text = Gamma(richTextBox1.Text, textBox1.Text.Split(',').Select(x => int.Parse(x)).ToArray());
                if (comboBox1.SelectedIndex == 4)
                    richTextBox2.Text = Polybius(richTextBox1.Text.Split(' ').Select(x => int.Parse(x)).ToArray());
                if (comboBox1.SelectedIndex == 5)
                    richTextBox2.Text = Tarabar(richTextBox1.Text);

                richTextBox1.SelectionStart = richTextBox1.TextLength;
            }
            catch
            {
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                    richTextBox1.Text = Cesarus(richTextBox2.Text, 26 - int.Parse(textBox1.Text));
                if (comboBox1.SelectedIndex == 1)
                    richTextBox1.Text = Vigenere(richTextBox2.Text, textBox1.Text, false).ToUpper();
                if (comboBox1.SelectedIndex == 2)
                    richTextBox1.Text = Rot13Rus(richTextBox2.Text, false);
                if (comboBox1.SelectedIndex == 3)
                    richTextBox1.Text = Gamma(richTextBox2.Text, textBox1.Text.Split(',').Select(x => int.Parse(x)).ToArray());
                if (comboBox1.SelectedIndex == 4)
                    richTextBox1.Text = DePolybius(richTextBox2.Text);
                if (comboBox1.SelectedIndex == 5)
                    richTextBox1.Text = Tarabar(richTextBox2.Text);

                richTextBox2.SelectionStart = richTextBox2.TextLength;
            }
            catch
            {
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1_TextChanged(null, null);
        }

        string Cesarus(string s, int offset)
        {
            if (offset < 1 || offset > 26) return s;
            return new string(s.ToList().Select(x => char.IsLetter(x) ? (char)(((x + offset - (char.IsLower(x) ? 'a' : 'A')) % 26) + (char.IsLower(x) ? 'a' : 'A')) : x).ToArray());
        }
        string Rot13Rus(string s, bool encrypt = true)
        {
            return new string(s.ToList().Select(x =>
            {
                bool isUpper = char.IsUpper(x);
                x = char.ToLower(x);
                x = char.IsLetter(x) ? (char)(((x + (encrypt ? 13 : -13) - 'А') % 32) + 'А') : x;
                return isUpper ? char.ToUpper(x) : char.ToLower(x);
            }).ToArray());
        }
        private string Vigenere(string text, string password, bool encrypt = true)
        {
            string GetRepeatKey(string s, int n)
            {
                var p = s;
                while (p.Length < n)
                    p += p;

                return p.Substring(0, n);
            }
            var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var gamma = GetRepeatKey(password, text.Length);
            var retValue = "";
            var q = letters.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = letters.IndexOf(text[i]);
                var codeIndex = letters.IndexOf(gamma[i]);
                if (letterIndex < 0)
                    retValue += text[i].ToString();
                else
                    retValue += letters[(q + letterIndex + ((encrypt ? 1 : -1) * codeIndex)) % q].ToString();
            }

            return retValue;
        }
        private string Gamma(string s, int[] gamma)
        {
            string kek = "";
            for (int i = 0; i < s.Length; i++)
                kek += Encoding.GetEncoding(1251).GetString(new byte[] { 
                    (byte)(Encoding.GetEncoding(1251).GetBytes(new char[] { s[i] })[0] ^ gamma[i % gamma.Length]) });
            return kek;
        }
        private string Tarabar(string s)
        {
            string kek = "бвгджзклмнщшчцхфтсрп";
            s = s.ToLower();
            string s1 = "";
            for (int i = 0; i < s.Length; i++)
                if (kek.Contains(s[i]))
                    s1 += kek[(10 + kek.IndexOf(s[i])) % 20];
                else
                    s1 += s[i];
            return s1;
        }
        private char[][] dict = new char[][] { new char[] { 'a', 'b', 'c', 'd', 'e' },
                                               new char[] { 'f', 'g', 'h', 'i', 'k' },
                                               new char[] { 'l', 'm', 'n', 'o', 'p' },
                                               new char[] { 'q', 'r', 's', 't', 'u' },
                                               new char[] { 'v', 'w', 'x', 'y', 'z' }};
        private string Polybius(int[] keys) =>
            new string(keys.Select(x => dict[(x / 10) - 1][--x % 10]).ToArray());
        private string DePolybius(string s)
        {
            string k = "";
            foreach(char c in s)
            {
                for (int i = 0; i < dict.Length; i++)
                    if (dict[i].Contains(c))
                        for (int j = 0; j < dict[i].Length; j++)
                            if(dict[i][j] == c)
                                k += (i + 1).ToString() + (j + 1).ToString() + " ";
            }
            return k;
        }
    }
}