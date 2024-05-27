
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

            string[] filesToPatch;
            if (File.Exists(txtNewItemsPath.Text))
            {
                filesToPatch = new string[1] { txtNewItemsPath.Text };
            }
            else
            {
                filesToPatch = Directory.GetFiles(txtNewItemsPath.Text);
            }

            foreach (var file in filesToPatch)
            {
                foreach (var item in JObject.Parse(File.ReadAllText(file)))
                {
                    try
                    {
                        ItemToAdd itemToAdd = (ItemToAdd)JsonConvert.DeserializeObject(item.Value.ToString(), typeof(ItemToAdd));
                        itemToAdd.ItemID = item.Key;

                        if (itemToAdd.Properties["item"] != null && itemToAdd.Properties["item"]["_name"] != null) //moxopixel
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
                            itemToAdd.ItemName = itemToAdd.Properties["locales"]["en"]["shortName"].ToString().ToLower().Replace(" ","_");
                            
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

            //make new realism compatible template from clone with itemid of mod
            var query = from items in ItemsToAdd.Where(x => !PatchedItems.Any(z => z.ItemID == x.ItemID))
                        join templates in Templates
                            on items.ItemTplToClone equals templates.ItemID
                        where templates != null
                        select new RealismTemplate(items.ItemID, items.ItemName, templates.Properties);


            newItems.AddRange(query);

            //If a mod references items from itself
            query = from items in ItemsToAdd.Where(x => !newItems.Any(z => z.ItemID == x.ItemID) && !PatchedItems.Any(z => z.ItemID == x.ItemID))
                        join clonedItems in newItems
                            on items.ItemTplToClone equals clonedItems.ItemID
                        where clonedItems != null
                        select new RealismTemplate(items.ItemID, items.ItemName, clonedItems.Properties);

            newItems.AddRange(query);

            //If a mod references items that have been patched already
            query = from items in ItemsToAdd.Where(x => !newItems.Any(z => z.ItemID == x.ItemID) && !PatchedItems.Any(z => z.ItemID == x.ItemID))
                    join patchedItems in PatchedItems
                        on items.ItemTplToClone equals patchedItems.ItemID
                    where patchedItems != null
                    select new RealismTemplate(items.ItemID, items.ItemName, patchedItems.Properties);

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
                        item.Name = Interaction.InputBox("Item Name is not unique." + Environment.NewLine + item.Name, "Name Error", item.Name);
                    }

                    writer.WritePropertyName(item.ItemID);
                    writer.WriteRawValue(JsonConvert.SerializeObject(item, Formatting.Indented));
                }
                writer.WriteEndObject();
            }
            File.WriteAllLines(textBox1.Text, sb.ToString().Split(@"\r\n"));
            MessageBox.Show($"File with {newItems.Count} Items created.");
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