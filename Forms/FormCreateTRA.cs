using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmartTran.SmartUtil;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SmartTran
{
    public partial class FormCreateTRA : Form
    {
        public FormCreateTRA()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm parent = (MainForm)this.Owner;
            parent.SaveFile(parent.currentFileName);

            Gameinfo gamInfo = new Gameinfo();
            if (this.comboBox1.SelectedIndex == 0)
            {
                gamInfo.GameTitle = "Space Quest: Vohaul Strikes Back";
                gamInfo.GameUID = "FBAAD142";
                gamInfo.Version = "3.2.0";

                CreateTraFile(gamInfo, "Korean.tra", SmartTranslation.ParseTranslation(parent.currentFileName));
            }
            else if (this.comboBox1.SelectedIndex == 1)
            {
                gamInfo.GameTitle = "Gemini Rue";
                gamInfo.GameUID = "84668800";
                gamInfo.Version = "3.2.0";

                CreateTraFile(gamInfo, "Korean.tra", SmartTranslation.ParseTranslation(parent.currentFileName));
            }
            else if (this.comboBox1.SelectedIndex == 3)
            {
                gamInfo.GameTitle = "The Cabin";
                gamInfo.GameUID = "9AF09E06";
                gamInfo.Version = "3.5.0.24";

                CreateTraFile(gamInfo, "Korean.tra", SmartTranslation.ParseTranslation(parent.currentFileName));
            }
            else if (this.comboBox1.SelectedIndex == 4)
            {
                gamInfo.GameTitle = "Space Quest 2 VGA";
                gamInfo.GameUID = "F15983E";
                gamInfo.Version = "3.2.0";

                CreateTraFile(gamInfo, "Korean.tra", SmartTranslation.ParseTranslation(parent.currentFileName));
            }

        }
    }
}
