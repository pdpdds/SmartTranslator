using Newtonsoft.Json.Linq;
using SmartTran.forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static SmartTran.SmartUtil;

namespace SmartTran
{

    public partial class MainForm : Form
    {
        public SmartGridUtil smartGridUtils;

        private int selectedRow = 0;
        public string currentFileName = "";
        private int numEntries = 0;
        private bool documentChanged = false;

        string _clientId;
        string _clientSecrete;
        string _language_source;
        string _language_target;

        public MainForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Smart Translation File(*.trs,*.txt)|*.trs;*.txt";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                //Clear the DataGrid
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                numEntries = 0;
                Dictionary<string, string> entryList = null;
                currentFileName = fileDialog.FileName;

                if (fileDialog.FileName.Contains(".trs"))
                {
                    entryList = SmartTranslation.ParseTranslation(fileDialog.FileName);
                }
                else if (fileDialog.FileName.Contains(".txt"))
                {
                    entryList = SmartTranslation.ParseTranslationTxt(fileDialog.FileName);
                }

                if (entryList != null)
                {
                    foreach (KeyValuePair<string, string> pair in entryList)
                    {
                        //Populate DataGridView
                        string[] newRow = { (numEntries + 1).ToString(), pair.Key, pair.Value };
                        dataGridView1.Rows.Add(newRow);
                        numEntries++;
                    }
                }

                //lblFileStatus.Text = "File loaded";

                toolStripStatusLabel1.Text = "Entries: " + numEntries;

                //Set Form text to filename
                this.Text = currentFileName + " - Smart Translation Editor";
                documentChanged = false;
            }
        }

        private void translationGrid_SelectionChanged(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            //translationGridUtils.Search.ResetPosition();
            selectedRow = dataGridView1.SelectedRows[0].Index;

            string originalText = (string)dataGridView1.Rows[selectedRow].Cells[1].Value;
            richTextBox1.Text = originalText;

            string translationText = (string)dataGridView1.Rows[selectedRow].Cells[2].Value;
            /*if(translationText.Length <= 0) {
                string tempText = originalText.Substring(originalText.IndexOf(" "));

                richTextBox2.Text = YandexTranslationApi.translate(null, "de", tempText, null, null);
            } else*/
            richTextBox2.Text = translationText;


        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void translateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (documentChanged)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    SaveFile(currentFileName);
                    documentChanged = false;
                }
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            smartGridUtils = new SmartGridUtil(dataGridView1);

            var configIni = new INIHelper("config.ini");

            _clientId = configIni.Read("CLIENT-ID", "CONFIG");
            _clientSecrete = configIni.Read("CLIENT-SECRETE", "CONFIG");
            _language_source = configIni.Read("LANGUAGE_SOURCE", "CONFIG");
            _language_target = configIni.Read("LANGUAGE_TARGET", "CONFIG");
        }

        private void BTN_TRAN_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            //translationGridUtils.Search.ResetPosition();
            selectedRow = dataGridView1.SelectedRows[0].Index;

            string originalText = (string)dataGridView1.Rows[selectedRow].Cells[1].Value;
            richTextBox1.Text = originalText;

            string translatedSentence = SmartUtil.RequestTranslation(_clientId, _clientSecrete, _language_source, _language_target, originalText);

            if(translatedSentence.Length > 0)
            {
                richTextBox2.Text = translatedSentence;
                dataGridView1.Rows[selectedRow].Cells[2].Value = translatedSentence;
                dataGridView1.Focus();

                smartGridUtils.Search.ResetPosition();
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void searchTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var searchForm = new SearchTextForm();
            searchForm.Show(this);
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string newText = richTextBox2.Text;
                dataGridView1.Rows[selectedRow].Cells[2].Value = newText;
                dataGridView1.Focus();

                smartGridUtils.Search.ResetPosition();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.Text = currentFileName + " • - Smart Translation Editor";

            if (dataGridView1.SelectedRows.Count != 0)
                documentChanged = true;
        }

        private void goToRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var searchForm = new GoToRowForm();
            searchForm.Show(this);
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (documentChanged)
            {
                SaveBeforeExiting();
            }
        }

        private void SaveBeforeExiting()
        {
            string question = currentFileName.Substring(currentFileName.LastIndexOf("\\") + 1) +
                        " has been modified. Do you want to save the changes?";

            //Ask if user wants to save if data was changed
            if (MessageBox.Show(question, "Smart Translation Editor", MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
            {
                //Save changes then exit
                if (dataGridView1.Rows.Count > 0)
                {

                    Match extension = Regex.Match(currentFileName, "\\.[0-9a-z]+$");
                    if (extension.Value.Equals(".trs"))
                    {
                        SaveFile(currentFileName);
                    }
                    else
                    {
                        SaveAs();
                    }
                }

            }

        }

        public void SaveFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            StreamWriter fw = new StreamWriter(fs, Encoding.Default);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                fw.WriteLine(row.Cells[1].Value);
                fw.WriteLine(row.Cells[2].Value);
            }
            fw.Close();
            fs.Close();
        }

      
        private void SaveAs()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "dat";
            //saveDialog.AddExtension = true;
            saveDialog.Filter = "Smart Translation File(*.trs)|*.trs";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFile(saveDialog.FileName);

                MessageBox.Show(
                    $"File was saved as {saveDialog.FileName}",
                    "File saved",
                    MessageBoxButtons.OK);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveAs();
            }
        }

        private void createTRAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var traForm = new FormCreateTRA();
            
            traForm.Show(this);       
        }
    }


}
