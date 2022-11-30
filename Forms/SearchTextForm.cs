using System;
using System.Windows.Forms;

namespace SmartTran.forms {
    public partial class SearchTextForm : Form {

        public SearchTextForm() {
            InitializeComponent();
        }

        private void startSearch(bool forward) {
            MainForm parent = (MainForm)this.Owner;

            SmartGridUtil.SearchOptions options = new SmartGridUtil.SearchOptions();
            options.CaseSensitive = caseSensitiveCheck.Checked;
            options.Forward = forward;
            options.WrapAround = wrapAroundCheck.Checked;

            String text = txtBoxSearchText.Text;
            if (!String.IsNullOrWhiteSpace(text.Trim())) {
                if (parent.smartGridUtils.Search.IsEnabled() && !parent.smartGridUtils.Search.SearchedText.Equals(text)) {
                    parent.smartGridUtils.Search.ResetPosition();
                }
                parent.smartGridUtils.SearchText(text, options);
            } else {
                //AudioManager.getInstance().audio.PlaySystemSound(System.Media.SystemSounds.Exclamation);
                MessageBox.Show("Text to search is empty.", "Empty text");
            }
        }

        private void btnSearchNext_Click(object sender, EventArgs e) {
            startSearch(true);
            
        }

        private void btnSearchPreview_Click(object sender, EventArgs e) {
            startSearch(false);
        }

        private void SearchTextForm_FormClosed(object sender, FormClosedEventArgs e) {
            MainForm parent = (MainForm)this.Owner;
            parent.smartGridUtils.Search.Disable();
        }
    }
}
