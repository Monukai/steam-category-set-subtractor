namespace Steam_Cats
{
    public partial class Form1 : Form
    {
        private ValveDataFormatManager _valveDataFormatManager;

        public Form1()
        {
            InitializeComponent();
            _valveDataFormatManager = new ValveDataFormatManager();
        }

        private void btn_fileSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                string filepath = openFileDialog.FileName;

                if (ValveDataFormatManager.IsValidVDFFile(filepath) == true)
                {
                    _valveDataFormatManager.SetFilePath(filepath);

                    List<string> steamCategories = _valveDataFormatManager.ParseCategories();
                    
                    mainCatListBox.Items.Add(steamCategories);
                    subsetCatListBox.Items.Add(steamCategories);
                }
                else
                {
                    ToolTip badFilepathToolTip = new ToolTip();
                    badFilepathToolTip.Tag = ValveDataFormatManager.INVALID_FILE;
                    //badFilepathToolTip.Popup();
                }
            }
        }
    }
}