namespace Steam_Cats
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.mainCatListBox = new System.Windows.Forms.ListBox();
            this.subsetCatListBox = new System.Windows.Forms.ListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_Compute = new System.Windows.Forms.Button();
            this.newCatItemsList = new System.Windows.Forms.ListBox();
            this.btn_fileSelect = new System.Windows.Forms.Button();
            this.mainCatSelection = new System.Windows.Forms.TextBox();
            this.subsetCatSelection = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(267, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Steam Category Set Substractor";
            // 
            // mainCatListBox
            // 
            this.mainCatListBox.FormattingEnabled = true;
            this.mainCatListBox.ItemHeight = 15;
            this.mainCatListBox.Location = new System.Drawing.Point(12, 70);
            this.mainCatListBox.Name = "mainCatListBox";
            this.mainCatListBox.Size = new System.Drawing.Size(120, 124);
            this.mainCatListBox.Sorted = true;
            this.mainCatListBox.TabIndex = 2;
            // 
            // subsetCatListBox
            // 
            this.subsetCatListBox.FormattingEnabled = true;
            this.subsetCatListBox.ItemHeight = 15;
            this.subsetCatListBox.Location = new System.Drawing.Point(159, 70);
            this.subsetCatListBox.Name = "subsetCatListBox";
            this.subsetCatListBox.Size = new System.Drawing.Size(120, 124);
            this.subsetCatListBox.Sorted = true;
            this.subsetCatListBox.TabIndex = 3;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_Compute
            // 
            this.btn_Compute.Location = new System.Drawing.Point(318, 200);
            this.btn_Compute.Name = "btn_Compute";
            this.btn_Compute.Size = new System.Drawing.Size(105, 23);
            this.btn_Compute.TabIndex = 5;
            this.btn_Compute.Text = "Compute";
            this.btn_Compute.UseVisualStyleBackColor = true;
            // 
            // newCatItemsList
            // 
            this.newCatItemsList.FormattingEnabled = true;
            this.newCatItemsList.ItemHeight = 15;
            this.newCatItemsList.Location = new System.Drawing.Point(303, 12);
            this.newCatItemsList.Name = "newCatItemsList";
            this.newCatItemsList.Size = new System.Drawing.Size(120, 154);
            this.newCatItemsList.Sorted = true;
            this.newCatItemsList.TabIndex = 6;
            // 
            // btn_fileSelect
            // 
            this.btn_fileSelect.Location = new System.Drawing.Point(12, 41);
            this.btn_fileSelect.Name = "btn_fileSelect";
            this.btn_fileSelect.Size = new System.Drawing.Size(26, 23);
            this.btn_fileSelect.TabIndex = 7;
            this.btn_fileSelect.Text = "...";
            this.btn_fileSelect.UseVisualStyleBackColor = true;
            this.btn_fileSelect.Click += new System.EventHandler(this.btn_fileSelect_Click);
            // 
            // mainCatSelection
            // 
            this.mainCatSelection.Location = new System.Drawing.Point(12, 200);
            this.mainCatSelection.Name = "mainCatSelection";
            this.mainCatSelection.Size = new System.Drawing.Size(120, 23);
            this.mainCatSelection.TabIndex = 8;
            // 
            // subsetCatSelection
            // 
            this.subsetCatSelection.Location = new System.Drawing.Point(159, 200);
            this.subsetCatSelection.Name = "subsetCatSelection";
            this.subsetCatSelection.Size = new System.Drawing.Size(120, 23);
            this.subsetCatSelection.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 232);
            this.Controls.Add(this.subsetCatSelection);
            this.Controls.Add(this.mainCatSelection);
            this.Controls.Add(this.btn_fileSelect);
            this.Controls.Add(this.newCatItemsList);
            this.Controls.Add(this.btn_Compute);
            this.Controls.Add(this.subsetCatListBox);
            this.Controls.Add(this.mainCatListBox);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Steam Catss";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox1;
        private ListBox mainCatListBox;
        private ListBox subsetCatListBox;
        private OpenFileDialog openFileDialog1;
        private Button btn_Compute;
        private ListBox newCatItemsList;
        private Button btn_fileSelect;
        private TextBox mainCatSelection;
        private TextBox subsetCatSelection;
    }
}