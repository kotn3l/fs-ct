namespace FLVERtoASCII
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
            this.cb_Tex = new System.Windows.Forms.CheckBox();
            this.parts_list = new System.Windows.Forms.ListBox();
            this.Select_ER_workingDir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.merge_armors = new System.Windows.Forms.Button();
            this.map = new System.Windows.Forms.Button();
            this.mapBox = new System.Windows.Forms.ListBox();
            this.cb_GameList = new System.Windows.Forms.ComboBox();
            this.lefthand = new System.Windows.Forms.ListBox();
            this.righthand = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.beardparts = new System.Windows.Forms.ListBox();
            this.hairparts = new System.Windows.Forms.ListBox();
            this.eyebrowparts = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.platform = new System.Windows.Forms.Button();
            this.tb_GameDir = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.basicFLVER = new System.Windows.Forms.TabPage();
            this.playerModel = new System.Windows.Forms.TabPage();
            this.cb_BodyUnder = new System.Windows.Forms.CheckBox();
            this.mapExt = new System.Windows.Forms.TabPage();
            this.mapInt = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.platf = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_destPlatf = new System.Windows.Forms.ComboBox();
            this.cb_sourcePlatf = new System.Windows.Forms.ComboBox();
            this.tb_dcxDir = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.basicFLVER.SuspendLayout();
            this.playerModel.SuspendLayout();
            this.mapExt.SuspendLayout();
            this.platf.SuspendLayout();
            this.SuspendLayout();
            // 
            // flverPath
            // 
            this.flverPath.Enabled = false;
            this.flverPath.Location = new System.Drawing.Point(6, 41);
            this.flverPath.Name = "flverPath";
            this.flverPath.Size = new System.Drawing.Size(399, 20);
            this.flverPath.TabIndex = 0;
            this.flverPath.Text = "Browse for flver";
            // 
            // browse_Button
            // 
            this.browse_Button.Location = new System.Drawing.Point(411, 41);
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
            this.bones.Location = new System.Drawing.Point(6, 97);
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
            this.asciiPath.Location = new System.Drawing.Point(6, 71);
            this.asciiPath.Name = "asciiPath";
            this.asciiPath.Size = new System.Drawing.Size(399, 20);
            this.asciiPath.TabIndex = 3;
            this.asciiPath.Text = "Browse for save location (optional)";
            // 
            // browseSave
            // 
            this.browseSave.Enabled = false;
            this.browseSave.Location = new System.Drawing.Point(411, 70);
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
            this.cb_Save.Location = new System.Drawing.Point(6, 127);
            this.cb_Save.Name = "cb_Save";
            this.cb_Save.Size = new System.Drawing.Size(162, 17);
            this.cb_Save.TabIndex = 5;
            this.cb_Save.Text = "Save to same dir as FLVER?";
            this.cb_Save.UseVisualStyleBackColor = true;
            this.cb_Save.CheckedChanged += new System.EventHandler(this.cb_Save_CheckedChanged);
            // 
            // merge
            // 
            this.merge.Location = new System.Drawing.Point(165, 7);
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
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Convert all chrbnds from folder";
            // 
            // browseTex
            // 
            this.browseTex.Location = new System.Drawing.Point(293, 7);
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
            this.root.Location = new System.Drawing.Point(107, 97);
            this.root.Name = "root";
            this.root.Size = new System.Drawing.Size(105, 17);
            this.root.TabIndex = 10;
            this.root.Text = "Add Root Bone?";
            this.root.UseVisualStyleBackColor = true;
            // 
            // cb_Tex
            // 
            this.cb_Tex.AutoSize = true;
            this.cb_Tex.Checked = true;
            this.cb_Tex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Tex.Location = new System.Drawing.Point(370, 42);
            this.cb_Tex.Name = "cb_Tex";
            this.cb_Tex.Size = new System.Drawing.Size(109, 17);
            this.cb_Tex.TabIndex = 11;
            this.cb_Tex.Text = "Extract Textures?";
            this.cb_Tex.UseVisualStyleBackColor = true;
            // 
            // parts_list
            // 
            this.parts_list.FormattingEnabled = true;
            this.parts_list.Location = new System.Drawing.Point(16, 24);
            this.parts_list.Name = "parts_list";
            this.parts_list.Size = new System.Drawing.Size(76, 446);
            this.parts_list.TabIndex = 12;
            this.parts_list.SelectedIndexChanged += new System.EventHandler(this.parts_list_SelectedIndexChanged);
            // 
            // Select_ER_workingDir
            // 
            this.Select_ER_workingDir.Location = new System.Drawing.Point(192, 10);
            this.Select_ER_workingDir.Name = "Select_ER_workingDir";
            this.Select_ER_workingDir.Size = new System.Drawing.Size(172, 23);
            this.Select_ER_workingDir.TabIndex = 13;
            this.Select_ER_workingDir.Text = "Select game directory";
            this.Select_ER_workingDir.UseVisualStyleBackColor = true;
            this.Select_ER_workingDir.Click += new System.EventHandler(this.Select_ER_workingDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Armors";
            // 
            // merge_armors
            // 
            this.merge_armors.Location = new System.Drawing.Point(285, 24);
            this.merge_armors.Name = "merge_armors";
            this.merge_armors.Size = new System.Drawing.Size(76, 23);
            this.merge_armors.TabIndex = 15;
            this.merge_armors.Text = "Merge";
            this.merge_armors.UseVisualStyleBackColor = true;
            this.merge_armors.Click += new System.EventHandler(this.merge_armors_Click);
            // 
            // map
            // 
            this.map.Location = new System.Drawing.Point(354, 3);
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
            this.mapBox.Location = new System.Drawing.Point(3, 3);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(345, 511);
            this.mapBox.TabIndex = 17;
            this.mapBox.SelectedIndexChanged += new System.EventHandler(this.mapBox_SelectedIndexChanged);
            // 
            // cb_GameList
            // 
            this.cb_GameList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_GameList.FormattingEnabled = true;
            this.cb_GameList.Location = new System.Drawing.Point(12, 12);
            this.cb_GameList.Name = "cb_GameList";
            this.cb_GameList.Size = new System.Drawing.Size(174, 21);
            this.cb_GameList.TabIndex = 18;
            this.cb_GameList.SelectedIndexChanged += new System.EventHandler(this.cb_GameList_SelectedIndexChanged);
            // 
            // lefthand
            // 
            this.lefthand.FormattingEnabled = true;
            this.lefthand.Location = new System.Drawing.Point(98, 24);
            this.lefthand.Name = "lefthand";
            this.lefthand.Size = new System.Drawing.Size(76, 212);
            this.lefthand.TabIndex = 20;
            this.lefthand.SelectedIndexChanged += new System.EventHandler(this.lefthand_SelectedIndexChanged);
            // 
            // righthand
            // 
            this.righthand.FormattingEnabled = true;
            this.righthand.Location = new System.Drawing.Point(98, 258);
            this.righthand.Name = "righthand";
            this.righthand.Size = new System.Drawing.Size(76, 212);
            this.righthand.TabIndex = 21;
            this.righthand.SelectedIndexChanged += new System.EventHandler(this.righthand_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Left WP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Right WP";
            // 
            // beardparts
            // 
            this.beardparts.Enabled = false;
            this.beardparts.FormattingEnabled = true;
            this.beardparts.Location = new System.Drawing.Point(180, 24);
            this.beardparts.Name = "beardparts";
            this.beardparts.Size = new System.Drawing.Size(76, 95);
            this.beardparts.TabIndex = 24;
            this.beardparts.SelectedIndexChanged += new System.EventHandler(this.beardparts_SelectedIndexChanged);
            // 
            // hairparts
            // 
            this.hairparts.Enabled = false;
            this.hairparts.FormattingEnabled = true;
            this.hairparts.Location = new System.Drawing.Point(180, 141);
            this.hairparts.Name = "hairparts";
            this.hairparts.Size = new System.Drawing.Size(76, 95);
            this.hairparts.TabIndex = 25;
            this.hairparts.SelectedIndexChanged += new System.EventHandler(this.hairparts_SelectedIndexChanged);
            // 
            // eyebrowparts
            // 
            this.eyebrowparts.Enabled = false;
            this.eyebrowparts.FormattingEnabled = true;
            this.eyebrowparts.Location = new System.Drawing.Point(180, 258);
            this.eyebrowparts.Name = "eyebrowparts";
            this.eyebrowparts.Size = new System.Drawing.Size(76, 95);
            this.eyebrowparts.TabIndex = 27;
            this.eyebrowparts.SelectedIndexChanged += new System.EventHandler(this.eyebrowparts_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(193, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Beards";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(202, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Hairs";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(193, 242);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Eyebrows";
            // 
            // platform
            // 
            this.platform.Location = new System.Drawing.Point(279, 17);
            this.platform.Name = "platform";
            this.platform.Size = new System.Drawing.Size(137, 23);
            this.platform.TabIndex = 31;
            this.platform.Text = "Convert Platform";
            this.platform.UseVisualStyleBackColor = true;
            this.platform.Click += new System.EventHandler(this.platform_Click);
            // 
            // tb_GameDir
            // 
            this.tb_GameDir.Enabled = false;
            this.tb_GameDir.Location = new System.Drawing.Point(12, 39);
            this.tb_GameDir.Name = "tb_GameDir";
            this.tb_GameDir.Size = new System.Drawing.Size(352, 20);
            this.tb_GameDir.TabIndex = 32;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.basicFLVER);
            this.tabControl1.Controls.Add(this.playerModel);
            this.tabControl1.Controls.Add(this.mapExt);
            this.tabControl1.Controls.Add(this.platf);
            this.tabControl1.Location = new System.Drawing.Point(12, 65);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(680, 544);
            this.tabControl1.TabIndex = 33;
            // 
            // basicFLVER
            // 
            this.basicFLVER.Controls.Add(this.label1);
            this.basicFLVER.Controls.Add(this.merge);
            this.basicFLVER.Controls.Add(this.browseTex);
            this.basicFLVER.Controls.Add(this.flverPath);
            this.basicFLVER.Controls.Add(this.browse_Button);
            this.basicFLVER.Controls.Add(this.asciiPath);
            this.basicFLVER.Controls.Add(this.root);
            this.basicFLVER.Controls.Add(this.browseSave);
            this.basicFLVER.Controls.Add(this.cb_Save);
            this.basicFLVER.Controls.Add(this.bones);
            this.basicFLVER.Location = new System.Drawing.Point(4, 22);
            this.basicFLVER.Name = "basicFLVER";
            this.basicFLVER.Padding = new System.Windows.Forms.Padding(3);
            this.basicFLVER.Size = new System.Drawing.Size(672, 518);
            this.basicFLVER.TabIndex = 0;
            this.basicFLVER.Text = "basic FLVER";
            this.basicFLVER.UseVisualStyleBackColor = true;
            // 
            // playerModel
            // 
            this.playerModel.Controls.Add(this.cb_BodyUnder);
            this.playerModel.Controls.Add(this.label2);
            this.playerModel.Controls.Add(this.parts_list);
            this.playerModel.Controls.Add(this.lefthand);
            this.playerModel.Controls.Add(this.label7);
            this.playerModel.Controls.Add(this.merge_armors);
            this.playerModel.Controls.Add(this.righthand);
            this.playerModel.Controls.Add(this.label6);
            this.playerModel.Controls.Add(this.label3);
            this.playerModel.Controls.Add(this.label5);
            this.playerModel.Controls.Add(this.label4);
            this.playerModel.Controls.Add(this.eyebrowparts);
            this.playerModel.Controls.Add(this.beardparts);
            this.playerModel.Controls.Add(this.hairparts);
            this.playerModel.Location = new System.Drawing.Point(4, 22);
            this.playerModel.Name = "playerModel";
            this.playerModel.Padding = new System.Windows.Forms.Padding(3);
            this.playerModel.Size = new System.Drawing.Size(672, 518);
            this.playerModel.TabIndex = 1;
            this.playerModel.Text = "Player Model";
            this.playerModel.UseVisualStyleBackColor = true;
            // 
            // cb_BodyUnder
            // 
            this.cb_BodyUnder.AutoSize = true;
            this.cb_BodyUnder.Location = new System.Drawing.Point(262, 53);
            this.cb_BodyUnder.Name = "cb_BodyUnder";
            this.cb_BodyUnder.Size = new System.Drawing.Size(140, 17);
            this.cb_BodyUnder.TabIndex = 34;
            this.cb_BodyUnder.Text = "Add Body Under Armor?";
            this.cb_BodyUnder.UseVisualStyleBackColor = true;
            this.cb_BodyUnder.CheckedChanged += new System.EventHandler(this.cb_BodyUnder_CheckedChanged);
            // 
            // mapExt
            // 
            this.mapExt.Controls.Add(this.mapInt);
            this.mapExt.Controls.Add(this.label9);
            this.mapExt.Controls.Add(this.map);
            this.mapExt.Controls.Add(this.mapBox);
            this.mapExt.Location = new System.Drawing.Point(4, 22);
            this.mapExt.Name = "mapExt";
            this.mapExt.Size = new System.Drawing.Size(672, 518);
            this.mapExt.TabIndex = 2;
            this.mapExt.Text = "Map";
            this.mapExt.UseVisualStyleBackColor = true;
            // 
            // mapInt
            // 
            this.mapInt.AutoSize = true;
            this.mapInt.Location = new System.Drawing.Point(355, 46);
            this.mapInt.Name = "mapInt";
            this.mapInt.Size = new System.Drawing.Size(0, 13);
            this.mapInt.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(351, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(141, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Selected map internal name:";
            // 
            // platf
            // 
            this.platf.Controls.Add(this.label8);
            this.platf.Controls.Add(this.cb_destPlatf);
            this.platf.Controls.Add(this.cb_sourcePlatf);
            this.platf.Controls.Add(this.tb_dcxDir);
            this.platf.Controls.Add(this.platform);
            this.platf.Location = new System.Drawing.Point(4, 22);
            this.platf.Name = "platf";
            this.platf.Size = new System.Drawing.Size(672, 518);
            this.platf.TabIndex = 3;
            this.platf.Text = "Platform";
            this.platf.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(130, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "->";
            // 
            // cb_destPlatf
            // 
            this.cb_destPlatf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_destPlatf.FormattingEnabled = true;
            this.cb_destPlatf.Location = new System.Drawing.Point(152, 19);
            this.cb_destPlatf.Name = "cb_destPlatf";
            this.cb_destPlatf.Size = new System.Drawing.Size(121, 21);
            this.cb_destPlatf.TabIndex = 34;
            // 
            // cb_sourcePlatf
            // 
            this.cb_sourcePlatf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_sourcePlatf.FormattingEnabled = true;
            this.cb_sourcePlatf.Location = new System.Drawing.Point(3, 19);
            this.cb_sourcePlatf.Name = "cb_sourcePlatf";
            this.cb_sourcePlatf.Size = new System.Drawing.Size(121, 21);
            this.cb_sourcePlatf.TabIndex = 33;
            // 
            // tb_dcxDir
            // 
            this.tb_dcxDir.Enabled = false;
            this.tb_dcxDir.Location = new System.Drawing.Point(0, 55);
            this.tb_dcxDir.Name = "tb_dcxDir";
            this.tb_dcxDir.Size = new System.Drawing.Size(416, 20);
            this.tb_dcxDir.TabIndex = 32;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 621);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tb_GameDir);
            this.Controls.Add(this.cb_GameList);
            this.Controls.Add(this.Select_ER_workingDir);
            this.Controls.Add(this.cb_Tex);
            this.MaximumSize = new System.Drawing.Size(720, 660);
            this.MinimumSize = new System.Drawing.Size(720, 660);
            this.Name = "Form1";
            this.Text = "fs-ct";
            this.tabControl1.ResumeLayout(false);
            this.basicFLVER.ResumeLayout(false);
            this.basicFLVER.PerformLayout();
            this.playerModel.ResumeLayout(false);
            this.playerModel.PerformLayout();
            this.mapExt.ResumeLayout(false);
            this.mapExt.PerformLayout();
            this.platf.ResumeLayout(false);
            this.platf.PerformLayout();
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
        private System.Windows.Forms.CheckBox cb_Tex;
        private System.Windows.Forms.ListBox parts_list;
        private System.Windows.Forms.Button Select_ER_workingDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button merge_armors;
        private System.Windows.Forms.Button map;
        private System.Windows.Forms.ListBox mapBox;
        private System.Windows.Forms.ComboBox cb_GameList;
        private System.Windows.Forms.ListBox lefthand;
        private System.Windows.Forms.ListBox righthand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox beardparts;
        private System.Windows.Forms.ListBox hairparts;
        private System.Windows.Forms.ListBox eyebrowparts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button platform;
        private System.Windows.Forms.TextBox tb_GameDir;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage basicFLVER;
        private System.Windows.Forms.TabPage playerModel;
        private System.Windows.Forms.TabPage mapExt;
        private System.Windows.Forms.TabPage platf;
        private System.Windows.Forms.TextBox tb_dcxDir;
        private System.Windows.Forms.ComboBox cb_destPlatf;
        private System.Windows.Forms.ComboBox cb_sourcePlatf;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label mapInt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cb_BodyUnder;
    }
}

