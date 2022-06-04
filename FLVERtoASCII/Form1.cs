using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SoulsFormats;

namespace FLVERtoASCII
{
    public enum Games
    {
        ELDEN_RING = 0,
        SEKIRO = 1,
        BLOODBORNE = 2,
        DARK_SOULS = 3
    }
    public partial class Form1 : Form
    {
        string[] Games_ToString = new string[] { "Elden Ring", "Sekiro", "Bloodborne", "Dark Souls 1/3" };
        private string ER_working_dir = "";
        private List<string> armorsets = new List<string>();
        string[] partsPrefix = new string[] { "am_m_", "bd_m_", "hd_m_", "lg_m_" };
        string[] bodyPrefix = new string[] { "fg_m_", "fc_m_", "hr_a_" };
        string weaponPrefix = "wp_a_";
        public static readonly Dictionary<string, string> ERmaps = new Dictionary<string, string>
        {
            { "m10_00_00_00", "Stormveil Castle, full detail" },
            { "m10_00_00_99", "Overworld, low detail from Stormveil" },
            { "m10_01_00_00", "Chapel of Anticipation, full detal" },
            //m10_01_00_99
            { "m11_00_00_00", "Leyndell, full detail" },
            { "m11_00_00_99", "Overworld, low detail from Leyndell" },
            { "m11_05_00_00", "Burned Leyndell, full detail" },
            { "m11_05_00_99", "Overworld, low detail from burned Leyndell" },
            { "m11_10_00_00", "Roundtable Hold, full detail" },
            { "m11_10_00_99", "Roundtable Hold, low detail" },
            { "m11_70_00_00", "Burned Leyndell, full detail" },
            { "m11_71_00_00", "Burned Leyndell, full detail" },
            { "m11_71_00_99", "Forge of the Giants, low detail" },
            { "m11_72_00_00", "Burned Leyndell, full detail" },
            { "m12_01_00_00", "Ainsel River Basin, full detail" },
            { "m12_01_00_99", "Roof stars" },
            { "m12_02_00_00", "Soifra River Basin, full detail" },
            { "m12_02_00_99", "Roof stars" },
            { "m12_03_00_00", "Deeproot Depths, full detail" },
            { "m12_03_00_99", "Roof" },
            { "m12_04_00_00", "Naturalborn boss arena, full detail" },
            { "m12_05_00_00", "Mohgwyn Palace, full detail" },
            { "m12_07_00_00", "Nokron entrance, full detail" },
            { "m12_08_00_00", "Ancestor Spirit boss arena, full detail" },
            { "m12_09_00_00", "Ancestor Spirit boss arena, full detail" },
            { "m13_00_00_00", "Crumbling Farum Azula, full detail" },
            { "m13_00_00_99", "Overworld, low detail from Farum Azula" },
            { "m14_00_00_00", "Academy of Raya Lucaria, full detail" },
            { "m14_00_00_99", "Overworld, low detail from Raya Lucaria" },
            { "m15_00_00_00", "Haligtree, full detail" },
            { "m15_00_00_99", "Overworld, low detail from Haligtree" },
            { "m16_00_00_00", "Volcano Manor, full detail" },
            { "m16_00_00_99", "Overworld, low detail from Volcano Manor" },
            { "m18_00_00_00", "Forgotten Hero's Grave" },
            { "m19_00_00_00", "Radagon boss arena" },
            { "m19_00_00_99", "Radagon boss arena, background details" },
            { "m19_70_00_00", "Radagon boss arena" },
            { "m30_00_00_00", "Some catacombs" },
            { "m30_01_00_00", "Some catacombs" },
            { "m30_02_00_00", "Some catacombs" },
            { "m30_03_00_00", "Some catacombs" },
            { "m30_04_00_00", "Some catacombs" },
            { "m30_05_00_00", "Some catacombs" },
            { "m30_06_00_00", "Some catacombs" },
            { "m30_07_00_00", "Some catacombs" },
            { "m30_08_00_00", "Some catacombs" },
            { "m30_09_00_00", "Gelmir Hero's Grave" },
            { "m30_10_00_00", "Auriza Hero's Grave" },
            { "m30_11_00_00", "Some catacombs" },
            { "m30_12_00_00", "Some catacombs" },
            { "m30_13_00_00", "Some catacombs" },
            { "m30_14_00_00", "Some catacombs" },
            { "m30_15_00_00", "Some catacombs" },
            { "m30_16_00_00", "War-Dead Catacombs" },
            { "m30_17_00_00", "Giant-Conquering Hero's Grave" },
            { "m30_18_00_00", "Some catacombs" },
            { "m30_19_00_00", "Some catacombs" },
            { "m30_20_00_00", "Hidden Path to the Haligtree" },
            { "m31_00_00_00", "Murkwater Cave" },
            { "m31_01_00_00", "Earthbore Cave" },
            { "m31_02_00_00", "Some cave" },
            { "m31_03_00_00", "Groveside Cave" },
            { "m31_04_00_00", "Some cave" },
            { "m31_05_00_00", "Some cave" },
            { "m31_06_00_00", "Academy Crystal Cave" },
            { "m31_07_00_00", "Some cave" },
            { "m31_09_00_00", "Some cave" },
            { "m31_10_00_00", "Some cave" },
            { "m31_11_00_00", "Sellia Hideaway" },
            { "m31_12_00_00", "Cave of the Forlorn" },
            { "m31_15_00_00", "Costal Cave" },
            { "m31_17_00_00", "Highroad Cave" },
            { "m31_18_00_00", "Perfumer's Grotto" },
            { "m31_19_00_00", "Sage's Cave" },
            { "m31_20_00_00", "Abandoned Cave" },
            { "m31_21_00_00", "Gaol Cave" },
            { "m31_22_00_00", "Some cave" },
            { "m32_00_00_00", "Some mine" },
            { "m32_01_00_00", "Some mine" },
            { "m32_02_00_00", "Some mine" },
            { "m32_04_00_00", "Some mine" },
            { "m32_05_00_00", "Some mine" },
            { "m32_07_00_00", "Some mine" },
            { "m32_08_00_00", "Sellia Crystal Tunnel" },
            { "m32_11_00_00", "Yelough Anix Tunnel" },
            { "m34_10_00_00", "Divine Tower of Limgrave, full detail" },
            { "m34_11_00_00", "Divine Tower of Liurnia, full detail" },
            { "m34_12_00_00", "Divine Tower of West Altus and Sealed Tunnel, full detail" },
            { "m34_13_00_00", "Divine Tower of Caelid, interior" },
            { "m34_14_00_00", "Divine Tower of East Altus, full detail" },
            { "m34_15_00_00", "Isloated Divine Tower, full detail" },
            { "m35_00_00_00", "Subterranean Shunning-Grounds" },
            { "m39_20_00_00", "Ruin-Strewn Precipice" },
        };
        public static readonly Dictionary<string, string> BBmaps = new Dictionary<string, string>
        {
            { "m21_00_00_00", "Hunter's Dream" },
            { "m21_01_00_00", "Abandoned Old Workshop (Hunter's Dream)" },
            { "m22_00_00_00", "Hemwick Charnel Lane" },
            { "m23_00_00_00", "Old Yharnam" },
            { "m24_00_00_00", "Cathedral Ward" },
            { "m24_01_00_00", "Central Yahrnam" },
            { "m24_02_00_00", "Upper Cathedral Ward" },
            { "m25_00_00_00", "Cainhurst" },
            { "m26_00_00_00", "Nightmare of Mensis" },
            { "m27_00_00_00", "Forbidden Woods" },
            { "m28_00_00_00", "Yahar'gul, Unseen Village" },
            { "m29_AA_BB_CC", "Chalice Dungeons" },
            { "m32_00_00_00", "Byrgenwerth" },
            { "m33_00_00_00", "Nightmare Frontier" },
            { "m34_00_00_00", "Hunter's Nightmare" },
            { "m35_00_00_00", "Research Hall" },
            { "m36_00_00_00", "Fishing Hamlet" },
        };


        public Form1()
        {
            InitializeComponent();
            mapBox.DataSource = ERmaps.Values.ToList();
            mapBox.SelectedIndex = 0;
            mapBox.Enabled = false;
            cb_GameList.DataSource = Games_ToString;
            //bones.Checked = false;
            //bones.Enabled = false;
        }

        private void browse_Button_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "FromSoftware Model|*.flver;*.chrbnd";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                flverPath.Text = file;
                if (cb_Save.Checked)
                {
                    asciiPath.Text = Path.GetDirectoryName(file);
                }
                string saveName = Path.GetFileNameWithoutExtension(file);

                //try
                {
                    FLVER2 test = new FLVER2();
                    if (Path.GetExtension(file).ToLower() == ".chrbnd")
                    {
                        BND4 chrbnd = BND4.Read(file);
                        for (int j = 0; j < chrbnd.Files.Count; j++)
                        {
                            if (Path.GetExtension(chrbnd.Files[j].Name).ToLower() == ".flver")
                            {
                                test = FLVER2.Read(chrbnd.Files[j].Bytes);
                                //filenames.Add(Path.GetFileNameWithoutExtension(chrbnds[i].Files[j].Name));
                            }
                            else if (Path.GetExtension(chrbnd.Files[j].Name).ToLower() == ".tpf")
                            {
                                //File.WriteAllBytes(outPath+"//"+Path.GetFileName(chrbnds[i].Files[j].Name).ToLower(), chrbnds[i].Files[j].Bytes);
                                //Directory.CreateDirectory(outPath + "//" + Path.GetFileName(chrbnds[i].Files[j].Name));
                                //texture(TPF.Read(chrbnds[i].Files[j].Bytes), outPath + "//" + Path.GetFileName(chrbnds[i].Files[j].Name));
                            }

                            //chrbnds[i].Files[j].Name;
                        }
                    }
                    else
                    {
                        test = FLVER2.Read(file);
                    }
                    Conversion c = new Conversion(test);
                    c.WriteFLVERtoASCII(asciiPath.Text, saveName, bones.Checked, root.Checked);
                }
                //catch (Exception)
                {
                    //throw;
                    //MessageBox.Show("Something went wrong, try with a different flver");
                }
            }
        }

        private void cb_Save_CheckedChanged(object sender, EventArgs e)
        {
            browseSave.Enabled = !cb_Save.Checked;
        }

        private void browseSave_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    asciiPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void merge_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Select a folder containing all armor pieces you want to merge -- be sure to have the flver files as well as the Bloodborne tool converted ascii-s in that one folder");
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Conversion c = new Conversion();
                    c.chrbndFolder(fbd.SelectedPath, fbd.SelectedPath + "//out");
                    //c.Merge(fbd.SelectedPath);
                }
            }


        }

        private void browseDCX_Click(object sender, EventArgs e) //texture
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "FromSoftware TPF|*.tpf";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                //flverPath.Text = file;
                string saveName = Path.GetFileNameWithoutExtension(file);
                //try
                {
                    //FLVER2 test = FLVER2.Read(file);
                    TPF texture = TPF.Read(file);
                    Conversion c = new Conversion();
                    c.texture(texture, Path.GetDirectoryName(file));
                    //c.WriteFLVERtoASCII(asciiPath.Text, saveName, bones.Checked, root.Checked);
                }
                //catch (Exception)
                {
                    //throw;
                    //MessageBox.Show("Something went wrong, try with a different flver");
                }
            }
        }

        private void bones_CheckedChanged(object sender, EventArgs e)
        {
            root.Enabled = bones.Checked;
        }

        private void Select_ER_workingDir_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    ER_working_dir = fbd.SelectedPath;
                }
            }

            string[] tempParts = Directory.GetFiles(ER_working_dir+"\\parts", "*.partsbnd");
            for (int i = 0; i < tempParts.Length; i++)
            {
                for (int j = 0; j < partsPrefix.Length; j++)
                {
                    if (Path.GetFileName(tempParts[i]).Contains(partsPrefix[j]))
                    {
                        if (!armorsets.Contains(Path.GetFileNameWithoutExtension(tempParts[i]).Substring(5)) && Path.GetFileNameWithoutExtension(tempParts[i]).Substring(8) == "0")
                        {
                            armorsets.Add(Path.GetFileNameWithoutExtension(tempParts[i]).Substring(5));
                        }
                        
                    }
                } 
            }
            armorsets.Sort();
            parts_list.DataSource = armorsets;
            mapBox.Enabled = true;


        }

        private void merge_armors_Click(object sender, EventArgs e)
        {
            Conversion c = new Conversion();
            c.armorset(ER_working_dir, armorsets[parts_list.SelectedIndex]);
        }

        private void map_Click(object sender, EventArgs e)
        {
            //string s = $"//map//mapstudio//{maps.FirstOrDefault(x => x.Value == mapBox.SelectedItem).Key}.msb";
            Conversion c = new Conversion();
            MSBE test = MSBE.Read(ER_working_dir+ $"//map//mapstudio//{ERmaps.FirstOrDefault(x => x.Value == mapBox.SelectedItem).Key}.msb");
            c.map((Games)cb_GameList.SelectedIndex,ER_working_dir, test, ERmaps.FirstOrDefault(x => x.Value == mapBox.SelectedItem).Key + "_" +mapBox.SelectedItem.ToString().Split(null,',')[0].ToLower());
        }
    }
}
