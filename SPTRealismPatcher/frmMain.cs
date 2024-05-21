
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;

namespace SPTRealismPatcher
{
    public partial class frmMain : Form
    {
        List<RealismTemplate> Templates;
        List<ItemToAdd> ItemsToAdd;
        List<RealismTemplate> PatchedItems;

        public frmMain()
        {
            InitializeComponent();
            txtExistingPatchPath.Text = Properties.Settings.Default["PatchedPath"].ToString();
            txtTemplatesPath.Text = Properties.Settings.Default["TemplatePath"].ToString();
            textBox1.Text = Properties.Settings.Default["ExportPath"].ToString();
        }

        public void cmdLoadFiles_Click(object sender, EventArgs e)
        {

            ItemsToAdd = new List<ItemToAdd>();
            foreach (var file in Directory.GetFiles(txtNewItemsPath.Text))
            {
                foreach (var item in JObject.Parse(File.ReadAllText(file)))
                {
                    try
                    {
                        ItemToAdd itemToAdd = (ItemToAdd)JsonConvert.DeserializeObject(item.Value.ToString(), typeof(ItemToAdd));
                        itemToAdd.ItemID = item.Key;

                        if (itemToAdd.Properties["item"]["_name"] != null) //moxopixel
                        {
                            if (itemToAdd.Properties["clone"] == null)
                            {
                                continue;
                            }

                            itemToAdd.ItemName = itemToAdd.Properties["item"]["_name"].ToString();
                            itemToAdd.ItemTplToClone = itemToAdd.Properties["clone"].ToString();
                        }
                        else
                        {
                            itemToAdd.ItemName = itemToAdd.Properties["overrideProperties"]["Prefab"]["path"].ToString();
                            
                            itemToAdd.ItemName = itemToAdd.ItemName.Split("/").Last();
                            itemToAdd.ItemName = itemToAdd.ItemName.Split(".").First();
                        }


                        ItemsToAdd.Add(itemToAdd);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"{file} couldn't be parsed. Are you sure it has items?");
                        break;
                    }
                }
            }

            Templates = new List<RealismTemplate>();
            string[] templateDirs = { "attachments", "weapons", "attatchments", "gear" };

            foreach (var directory in Directory.GetDirectories(txtTemplatesPath.Text).Where(x => templateDirs.Contains(x.Split(@"\").Last().ToLower())))
            {
                string templateDir = directory;
                if (Directory.Exists(Path.Combine(templateDir, "Realism")))
                {
                    templateDir = Path.Combine(templateDir, "Realism");
                }

                foreach (var file in Directory.GetFiles(templateDir))
                {
                    foreach (var item in JObject.Parse(File.ReadAllText(file)))
                    {
                        RealismTemplate template = (RealismTemplate)JsonConvert.DeserializeObject(item.Value.ToString(), typeof(RealismTemplate));
                        Templates.Add(template);
                    }
                }
            }

            PatchedItems = new List<RealismTemplate>();
            foreach (var directory in Directory.GetDirectories(txtExistingPatchPath.Text))
            {
                foreach (var file in Directory.GetFiles(directory))
                {
                    foreach (var item in JObject.Parse(File.ReadAllText(file)))
                    {
                        RealismTemplate template = (RealismTemplate)JsonConvert.DeserializeObject(item.Value.ToString(), typeof(RealismTemplate));
                        PatchedItems.Add(template);
                    }
                }
            }


            List<RealismTemplate> newItems = new List<RealismTemplate>();

            var query = from items in ItemsToAdd.Where(x => !PatchedItems.Any(z => z.ItemID == x.ItemID))
                        join templates in Templates
                            on items.ItemTplToClone equals templates.ItemID
                        where templates != null
                        select new RealismTemplate(items.ItemID, items.ItemName, templates.Properties);


            newItems.AddRange(query);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                foreach (var item in newItems)
                {
                    if (Templates.Any(x => x.Name == item.Name) || newItems.Any(x => item.Name == x.Name && item.ItemID != x.ItemID))
                    {
                        item.Name = Interaction.InputBox("Item Name is not unique." + Environment.NewLine + item.Properties["locales"]["en"]["name"], "Name Error", item.Name + item.Properties["locales"]["en"]["shortName"]);
                    }

                    writer.WritePropertyName(item.ItemID);
                    writer.WriteRawValue(JsonConvert.SerializeObject(item, Formatting.Indented));
                }
                writer.WriteEndObject();
            }
            File.WriteAllLines(textBox1.Text, sb.ToString().Split(@"\r\n"));    

            //lvToPatch.Items.AddRange(ItemsToAdd.Select(x => new ListViewItem { Text = x.ItemID }).ToArray());

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default["PatchedPath"] = txtExistingPatchPath.Text;
            Properties.Settings.Default["TemplatePath"] = txtTemplatesPath.Text;
            Properties.Settings.Default["ExportPath"] = textBox1.Text;
            Properties.Settings.Default.Save();
        }
    }
}