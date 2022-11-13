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

namespace WpfAppSimple.ProductAction
{
    public partial class EditProductForm : Form
    {
        public string Product_Name { get; set; }
        public string Product_Price { get; set; }
        public string Product_Description { get; set; }
        public List<ImageItemListView> RemoveFiles { get; set; } = new List<ImageItemListView>();
        public List<ImageItemListView> Product_Images { get; set; } = new List<ImageItemListView>();

        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }
        public EditProductForm()
        {
            InitializeComponent();
            lvImages.LargeImageList = new ImageList();
            lvImages.LargeImageList.ImageSize = new Size(64, 64);
            lvImages.MultiSelect = false;
            lvImages.ListViewItemSorter = new ListViewIndexComparer();
            // Initialize the insertion mark.
            lvImages.InsertionMark.Color = Color.Green;
            lvImages.AllowDrop = true;

        }

        private void InitInputForm()
        {
            txtName.Text = Product_Name;
            txtDescription.Text = Product_Description;
            txtPrice.Text = Product_Price;
            foreach (var image in Product_Images)
            {
                string key = image.Name;
                ListViewItem item = new ListViewItem();
                item.Tag = image;
                item.Text = Path.GetFileName($"{image.Name}");
                item.ImageKey = key;
                MemoryStream ms = new MemoryStream();
                using (FileStream file = new FileStream($"images/{image.Name}", FileMode.Open, FileAccess.Read))
                    file.CopyTo(ms);
                lvImages.LargeImageList.Images.Add(key, Image.FromStream(ms));
                lvImages.Items.Add(item);
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string key = Guid.NewGuid().ToString();
                ListViewItem item = new ListViewItem();
                item.Tag = new ImageItemListView
                {
                    Id = 0,
                    Name = dlg.FileName
                };
                item.Text = Path.GetFileName(dlg.FileName);
                item.ImageKey = key;
                lvImages.LargeImageList.Images.Add(key, Image.FromFile(dlg.FileName));
                lvImages.Items.Add(item);

                //MessageBox.Show("Select image " + dlg.FileName);
            }
        }

        private void lvImages_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvImages.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void lvImages_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void lvImages_DragLeave(object sender, EventArgs e)
        {
            lvImages.InsertionMark.Index = -1;
        }

        private void lvImages_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse pointer.
            Point targetPoint =
                lvImages.PointToClient(new Point(e.X, e.Y));

            // Retrieve the index of the item closest to the mouse pointer.
            int targetIndex = lvImages.InsertionMark.NearestIndex(targetPoint);

            // Confirm that the mouse pointer is not over the dragged item.
            if (targetIndex > -1)
            {
                // Determine whether the mouse pointer is to the left or
                // the right of the midpoint of the closest item and set
                // the InsertionMark.AppearsAfterItem property accordingly.
                Rectangle itemBounds = lvImages.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    lvImages.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    lvImages.InsertionMark.AppearsAfterItem = false;
                }
            }

            // Set the location of the insertion mark. If the mouse is
            // over the dragged item, the targetIndex value is -1 and
            // the insertion mark disappears.
            lvImages.InsertionMark.Index = targetIndex;
        }

        private void lvImages_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the index of the insertion mark;
            int targetIndex = lvImages.InsertionMark.Index;

            // If the insertion mark is not visible, exit the method.
            if (targetIndex == -1)
            {
                return;
            }

            // If the insertion mark is to the right of the item with
            // the corresponding index, increment the target index.
            if (lvImages.InsertionMark.AppearsAfterItem)
            {
                targetIndex++;
            }

            // Retrieve the dragged item.
            ListViewItem draggedItem =
                (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            // Insert a copy of the dragged item at the target index.
            // A copy must be inserted before the original item is removed
            // to preserve item index values. 
            lvImages.Items.Insert(
                targetIndex, (ListViewItem)draggedItem.Clone());

            // Remove the original copy of the dragged item.
            lvImages.Items.Remove(draggedItem);
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            Product_Name = txtName.Text;
            Product_Description = txtDescription.Text;
            Product_Price = txtPrice.Text;
            Product_Images.Clear();
            foreach (ListViewItem item in lvImages.Items)
            {
                Product_Images.Add((ImageItemListView)item.Tag);
            }

            this.DialogResult = DialogResult.OK;
        }

        private void EditProductForm_Load(object sender, EventArgs e)
        {
            InitInputForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var listSelect = lvImages.SelectedItems;
            if (listSelect.Count > 0)
            {
                var item = listSelect[0];
                var pImage = (ImageItemListView)item.Tag;
                if (pImage.Id > 0)
                {
                    RemoveFiles.Add(pImage);
                }
                lvImages.Items.Remove(item);
            }
        }
    }
}
