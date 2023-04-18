using System.Windows.Forms;

namespace Steam_Cats
{
    public partial class Form1 : Form
    {
        private ValveDataFormatManager _valveDataFormatManager;
        private SteamAppDictionary _steamAppDictionary;

        private List<string>? _steamCategories;

        public Form1()
        {
            InitializeComponent();
            _valveDataFormatManager = new ValveDataFormatManager();

            this.subsetCatListBox.Click += delegate (object? sender, EventArgs e)
            { OnCatListBox_Click(sender, e, subsetCatSelection); };
            this.mainCatListBox.Click += delegate (object? sender, EventArgs e)
            { OnCatListBox_Click(sender, e, mainCatSelection); };
            this.btn_Compute.Click += delegate (object? sender, EventArgs e)
            { OnCompute_Click(sender, e, mainCatSelection, subsetCatSelection); };

            _steamCategories = new List<string>();
            _steamAppDictionary = new SteamAppDictionary();
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

                    _steamCategories = new List<string>();
                    _valveDataFormatManager.ParseCategories(ref _steamCategories);

                    if (_steamCategories == null)
                    {
                        ShowBadFilePathPopUp();
                    }
                    else
                    {
                        foreach (string cat in _steamCategories)
                        {
                            mainCatListBox.Items.Add(cat);
                            subsetCatListBox.Items.Add(cat);
                        }
                    }
                }
                else
                {
                    ShowBadFilePathPopUp();
                }
            }
        }

        private async void OnCompute_Click(object sender, EventArgs e, TextBox mainCat, TextBox subCat)
        {
            if (_valveDataFormatManager.HasValidVDFFile() == true)
            {
                newCatItemsList.Items.Clear();
                if (_steamAppDictionary.HasLoaded() == false && _steamAppDictionary.IsLoading() == false)
                {
                    await _steamAppDictionary.LoadSteamAppData();
                }

                List<int>? mainCategoryApps = new List<int>();
                List<int>? subCategoryApps = new List<int>();
                List<int>? resultSetApps = new List<int>();

                _valveDataFormatManager.GetAppsForCategory(ref mainCategoryApps, mainCat.Text);
                _valveDataFormatManager.GetAppsForCategory(ref subCategoryApps, subCat.Text);

                // error check to make sure main cat and sub cat apps have items
                string noItemsText = "No items found in {0}. Did you check your spelling?";
                if (mainCategoryApps.Count == 0)
                {
                    ShowCustomPopUp(String.Format(noItemsText, mainCat.Text), "Empty Category", MessageBoxButtons.OK);
                    return;
                }
                if (subCategoryApps.Count == 0)
                {
                    ShowCustomPopUp(String.Format(noItemsText, subCat.Text), "Empty Category", MessageBoxButtons.OK);
                    return;
                }

                resultSetApps = mainCategoryApps.Except(subCategoryApps).ToList();

                // if 0 items, messagebox ("no items!")
                if (resultSetApps.Count == 0)
                {
                    ShowCustomPopUp("No items computed! Consider reversing the category order!", "No Resulting Items", MessageBoxButtons.OK);
                    return;
                }

                AddNames(resultSetApps);
                if (_steamAppDictionary.HasLoaded() == false)
                {
                    await Task.Run(() => ResolveNames(resultSetApps));
                }
            }
            else
            {
                ShowBadFilePathPopUp();
            }
        }

        private void AddNames(List<int> resultSetApps)
        {
            if (_steamAppDictionary.HasLoaded() == false)
            {
                foreach (int app in resultSetApps)
                {
                    newCatItemsList.Items.Add(app.ToString());
                }

                // spawn new thread to attempt to resolve names of each app, using steampapifacilitator
            }
            else
            {
                foreach (int app in resultSetApps)
                {
                    if (_steamAppDictionary.ContainsKey(app))
                    {
                        newCatItemsList.Items.Add(_steamAppDictionary[app]);
                    }
                    else
                    {
                        newCatItemsList.Items.Add(app.ToString());
                    }
                }
            }
        }

        private void ResolveNames(List<int> resultSetApps)
        {
            while (_steamAppDictionary.HasLoaded() == false)
            {
                if (_steamAppDictionary.IsLoading() == false && _steamAppDictionary.HasLoaded() == false)
                {
                    return;
                }

                Thread.Sleep(1000);
            }

            newCatItemsList.Items.Clear();
            AddNames(resultSetApps);

            return;
        }

        private void ShowCustomPopUp(string popUpText, string popUpTitle, MessageBoxButtons popUpButtons)
        {
            MessageBox.Show(popUpText, popUpTitle, popUpButtons);
        }

        private void ShowBadFilePathPopUp()
        {
            MessageBox.Show(ValveDataFormatManager.INVALID_FILE, "Invalid File", MessageBoxButtons.OK);
        }

        private void OnCatListBox_Click(object sender, EventArgs e, TextBox catSelectionTextBox)
        {
            ListBox listBoxSender = sender as ListBox;

            catSelectionTextBox.Text = listBoxSender.Items[listBoxSender.SelectedIndex].ToString();
        }
    }
}