﻿namespace FLVERtoASCII
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flverPath = new System.Windows.Forms.TextBox();
            this.browse_Button = new System.Windows.Forms.Button();
            this.bones = new System.Windows.Forms.CheckBox();
            this.asciiPath = new System.Windows.Forms.TextBox();
            this.browseSave = new System.Windows.Forms.Button();
            this.cb_Save = new System.Windows.Forms.CheckBox();
            this.merge = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.browseTex = new System.Windows.Forms.Button();
            this.root = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.parts_list = new System.Windows.Forms.ListBox();
            this.Select_ER_workingDir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.merge_armors = new System.Windows.Forms.Button();
            this.map = new System.Windows.Forms.Button();
            this.mapBox = new System.Windows.Forms.ListBox();
            this.cb_GameList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // flverPath
            // 
            this.flverPath.Enabled = false;
            this.flverPath.Location = new System.Drawing.Point(13, 13);
            this.flverPath.Name = "flverPath";
            this.flverPath.Size = new System.Drawing.Size(399, 20);
            this.flverPath.TabIndex = 0;
            this.flverPath.Text = "Browse for flver";
            // 
            // browse_Button
            // 
            this.browse_Button.Location = new System.Drawing.Point(418, 13);
            this.browse_Button.Name = "browse_Button";
            this.browse_Button.Size = new System.Drawing.Size(75, 23);
            this.browse_Button.TabIndex = 1;
            this.browse_Button.Text = "Browse";
            this.browse_Button.UseVisualStyleBackColor = true;
            this.browse_Button.Click += new System.EventHandler(this.browse_Button_Click);
            // 
            // bones
            // 
            this.bones.AutoSize = true;
            this.bones.Checked = true;
            this.bones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bones.Location = new System.Drawing.Point(499, 16);
            this.bones.Name = "bones";
            this.bones.Size = new System.Drawing.Size(95, 17);
            this.bones.TabIndex = 2;
            this.bones.Text = "Export Bones?";
            this.bones.UseVisualStyleBackColor = true;
            this.bones.CheckedChanged += new System.EventHandler(this.bones_CheckedChanged);
            // 
            // asciiPath
            // 
            this.asciiPath.Enabled = false;
            this.asciiPath.Location = new System.Drawing.Point(13, 43);
            this.asciiPath.Name = "asciiPath";
            this.asciiPath.Size = new System.Drawing.Size(399, 20);
            this.asciiPath.TabIndex = 3;
            this.asciiPath.Text = "Browse for save location (optional)";
            // 
            // browseSave
            // 
            this.browseSave.Enabled = false;
            this.browseSave.Location = new System.Drawing.Point(418, 42);
            this.browseSave.Name = "browseSave";
            this.browseSave.Size = new System.Drawing.Size(75, 23);
            this.browseSave.TabIndex = 4;
            this.browseSave.Text = "Browse";
            this.browseSave.UseVisualStyleBackColor = true;
            this.browseSave.Click += new System.EventHandler(this.browseSave_Click);
            // 
            // cb_Save
            // 
            this.cb_Save.AutoSize = true;
            this.cb_Save.Checked = true;
            this.cb_Save.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Save.Location = new System.Drawing.Point(499, 46);
            this.cb_Save.Name = "cb_Save";
            this.cb_Save.Size = new System.Drawing.Size(162, 17);
            this.cb_Save.TabIndex = 5;
            this.cb_Save.Text = "Save to same dir as FLVER?";
            this.cb_Save.UseVisualStyleBackColor = true;
            this.cb_Save.CheckedChanged += new System.EventHandler(this.cb_Save_CheckedChanged);
            // 
            // merge
            // 
            this.merge.Location = new System.Drawing.Point(160, 70);
            this.merge.Name = "merge";
            this.merge.Size = new System.Drawing.Size(75, 23);
            this.merge.TabIndex = 7;
            this.merge.Text = "Browse";
            this.merge.UseVisualStyleBackColor = true;
            this.merge.Click += new System.EventHandler(this.merge_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Merge armor pieces together";
            // 
            // browseTex
            // 
            this.browseTex.Location = new System.Drawing.Point(418, 71);
            this.browseTex.Name = "browseTex";
            this.browseTex.Size = new System.Drawing.Size(75, 23);
            this.browseTex.TabIndex = 9;
            this.browseTex.Text = "Browse";
            this.browseTex.UseVisualStyleBackColor = true;
            this.browseTex.Click += new System.EventHandler(this.browseDCX_Click);
            // 
            // root
            // 
            this.root.AutoSize = true;
            this.root.Checked = true;
            this.root.CheckState = System.Windows.Forms.CheckState.Checked;
            this.root.Location = new System.Drawing.Point(600, 16);
            this.root.Name = "root";
            this.root.Size = new System.Drawing.Size(105, 17);
            this.root.TabIndex = 10;
            this.root.Text = "Add Root Bone?";
            this.root.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(711, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(109, 17);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "Extract Textures?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // parts_list
            // 
            this.parts_list.FormattingEnabled = true;
            this.parts_list.Location = new System.Drawing.Point(12, 284);
            this.parts_list.Name = "parts_list";
            this.parts_list.Size = new System.Drawing.Size(76, 420);
            this.parts_list.TabIndex = 12;
            // 
            // Select_ER_workingDir
            // 
            this.Select_ER_workingDir.Location = new System.Drawing.Point(12, 127);
            this.Select_ER_workingDir.Name = "Select_ER_workingDir";
            this.Select_ER_workingDir.Size = new System.Drawing.Size(175, 23);
            this.Select_ER_workingDir.TabIndex = 13;
            this.Select_ER_workingDir.Text = "Select game extracted directory";
            this.Select_ER_workingDir.UseVisualStyleBackColor = true;
            this.Select_ER_workingDir.Click += new System.EventHandler(this.Select_ER_workingDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "parts";
            // 
            // merge_armors
            // 
            this.merge_armors.Location = new System.Drawing.Point(95, 284);
            this.merge_armors.Name = "merge_armors";
            this.merge_armors.Size = new System.Drawing.Size(75, 23);
            this.merge_armors.TabIndex = 15;
            this.merge_armors.Text = "Merge";
            this.merge_armors.UseVisualStyleBackColor = true;
            this.merge_armors.Click += new System.EventHandler(this.merge_armors_Click);
            // 
            // map
            // 
            this.map.Location = new System.Drawing.Point(418, 284);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(75, 23);
            this.map.TabIndex = 16;
            this.map.Text = "Map";
            this.map.UseVisualStyleBackColor = true;
            this.map.Click += new System.EventHandler(this.map_Click);
            // 
            // mapBox
            // 
            this.mapBox.FormattingEnabled = true;
            this.mapBox.Location = new System.Drawing.Point(176, 284);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(236, 420);
            this.mapBox.TabIndex = 17;
            // 
            // cb_GameList
            // 
            this.cb_GameList.FormattingEnabled = true;
            this.cb_GameList.Location = new System.Drawing.Point(13, 100);
            this.cb_GameList.Name = "cb_GameList";
            this.cb_GameList.Size = new System.Drawing.Size(174, 21);
            this.cb_GameList.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 752);
            this.Controls.Add(this.cb_GameList);
            this.Controls.Add(this.mapBox);
            this.Controls.Add(this.map);
            this.Controls.Add(this.merge_armors);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Select_ER_workingDir);
            this.Controls.Add(this.parts_list);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.root);
            this.Controls.Add(this.browseTex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.merge);
            this.Controls.Add(this.cb_Save);
            this.Controls.Add(this.browseSave);
            this.Controls.Add(this.asciiPath);
            this.Controls.Add(this.bones);
            this.Controls.Add(this.browse_Button);
            this.Controls.Add(this.flverPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox flverPath;
        private System.Windows.Forms.Button browse_Button;
        private System.Windows.Forms.CheckBox bones;
        private System.Windows.Forms.TextBox asciiPath;
        private System.Windows.Forms.Button browseSave;
        private System.Windows.Forms.CheckBox cb_Save;
        private System.Windows.Forms.Button merge;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseTex;
        private System.Windows.Forms.CheckBox root;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ListBox parts_list;
        private System.Windows.Forms.Button Select_ER_workingDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button merge_armors;
        private System.Windows.Forms.Button map;
        private System.Windows.Forms.ListBox mapBox;
        private System.Windows.Forms.ComboBox cb_GameList;
    }
}
