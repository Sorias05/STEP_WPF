using LibDatabase.Entities;
using LibDatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace WpfAppSimple.ProductAction
{
    public partial class ProductForm : Form
    {
        // Sorts ListViewItem objects by index.
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }

        private MyDataContext myData = new MyDataContext();
        public ProductForm()
        {
            InitializeComponent();
            LoadProductListView();
        }

        private void LoadProductListView()
        {
            try
            {
                lvProducts.Clear();
                lvProducts.LargeImageList = new ImageList();
                lvProducts.LargeImageList.ImageSize = new Size(64, 64);

                foreach (var p in myData.Products
                    .Include(x => x.ProductImages)
                    .ToList())
                {
                    var pImage = p.ProductImages.OrderBy(x => x.Priority).FirstOrDefault();
                    string id = "0";
                    string image = "no-image.jpg";
                    if (pImage != null)
                    {
                        image = pImage.Name;
                        id = pImage.Id.ToString();
                    }
                    MemoryStream ms = new MemoryStream();
                    using (FileStream file = new FileStream($"images/{image}", FileMode.Open, FileAccess.Read))
                        file.CopyTo(ms);

                    lvProducts.LargeImageList.Images.Add(id,
                        Image.FromStream(ms));
                    ListViewItem item = new ListViewItem();
                    item.Tag = p;
                    item.Text = $"{p.Name}\r\n{p.Price}";
                    item.ImageKey = $"{id}";

                    //item.Image
                    //item.
                    lvProducts.Items.Add(item);
                }
                //
                //var products = myData.Products.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error start app " + ex.Message);
            }
        }

        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            var listSelect = lvProducts.SelectedItems;
            if (listSelect.Count > 0)
            {
                var item = listSelect[0];
                var pImage = (ProductEntity)item.Tag;
                var p = myData.Products
                    .Include(x => x.ProductImages)
                    .SingleOrDefault(x => x.Id == pImage.Id);
                if (p != null)
                {
                    EditProductForm dlg = new EditProductForm();
                    dlg.Product_Name = p.Name;
                    dlg.Product_Price = p.Price.ToString();
                    dlg.Product_Description = p.Description;
                    dlg.Product_Images = new List<ImageItemListView>();
                    foreach (var image in p.ProductImages.OrderBy(i => i.Priority))
                    {
                        dlg.Product_Images.Add(new ImageItemListView
                        {
                            Id = image.Id,
                            Name = image.Name
                        });
                    }
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        p.Name = dlg.Product_Name;
                        p.Price = decimal.Parse(dlg.Product_Price);
                        p.Description = dlg.Product_Description;

                        myData.Products.Update(p);
                        myData.SaveChanges();

                        foreach (var pImgDel in p.ProductImages)
                        {
                            myData.ProductImages.Remove(pImgDel);
                            myData.SaveChanges();
                        }

                        foreach (var pImgDel in dlg.RemoveFiles)
                        {
                            File.Delete(@$"images\{pImgDel.Name}");
                        }

                        int i = 1;
                        foreach (var newImage in dlg.Product_Images)
                        {
                            string imageName = Path.GetRandomFileName() + ".jpg";
                            if (newImage.Id == 0)
                            {
                                string dir = "images";
                                if (!Directory.Exists(dir))
                                    Directory.CreateDirectory(dir);
                                Bitmap bitmap = new Bitmap(newImage.Name);
                                bitmap.Save(Path.Combine(dir, imageName), ImageFormat.Jpeg);
                            }
                            else
                            {
                                imageName = newImage.Name;
                            }

                            var pi = new ProductImage
                            {
                                Name = imageName,
                                ProductId = p.Id,
                                Priority = i
                            };
                            i++;
                            myData.ProductImages.Add(pi);
                            myData.SaveChanges();
                        }
                        LoadProductListView();
                    }
                }
                //MessageBox.Show("Product info: " + pImage.Id.ToString());
            }
            else
            {
                MessageBox.Show("Оберіть товар");
            }
        }

        private void lvProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var listSelect = lvProducts.SelectedItems;
            if (listSelect.Count > 0)
            {
                var item = listSelect[0];
                var pImage = (ProductEntity)item.Tag;
                MessageBox.Show("Product info: " + pImage.Id.ToString());
            }
        }

        #region Drog and Drop ListView

        // Starts the drag-and-drop operation when an item is dragged.
        private void lvProducts_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvProducts.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        // Sets the target drop effect.
        private void lvProducts_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        // Moves the insertion mark as the item is dragged.
        private void lvProducts_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse pointer.
            Point targetPoint =
                lvProducts.PointToClient(new Point(e.X, e.Y));

            // Retrieve the index of the item closest to the mouse pointer.
            int targetIndex = lvProducts.InsertionMark.NearestIndex(targetPoint);

            // Confirm that the mouse pointer is not over the dragged item.
            if (targetIndex > -1)
            {
                // Determine whether the mouse pointer is to the left or
                // the right of the midpoint of the closest item and set
                // the InsertionMark.AppearsAfterItem property accordingly.
                Rectangle itemBounds = lvProducts.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    lvProducts.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    lvProducts.InsertionMark.AppearsAfterItem = false;
                }
            }

            // Set the location of the insertion mark. If the mouse is
            // over the dragged item, the targetIndex value is -1 and
            // the insertion mark disappears.
            lvProducts.InsertionMark.Index = targetIndex;
        }

        // Removes the insertion mark when the mouse leaves the control.
        private void lvProducts_DragLeave(object sender, EventArgs e)
        {
            lvProducts.InsertionMark.Index = -1;
        }

        // Moves the item to the location of the insertion mark.
        private void lvProducts_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the index of the insertion mark;
            int targetIndex = lvProducts.InsertionMark.Index;

            // If the insertion mark is not visible, exit the method.
            if (targetIndex == -1)
            {
                return;
            }

            // If the insertion mark is to the right of the item with
            // the corresponding index, increment the target index.
            if (lvProducts.InsertionMark.AppearsAfterItem)
            {
                targetIndex++;
            }

            // Retrieve the dragged item.
            ListViewItem draggedItem =
                (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            // Insert a copy of the dragged item at the target index.
            // A copy must be inserted before the original item is removed
            // to preserve item index values. 
            lvProducts.Items.Insert(
                targetIndex, (ListViewItem)draggedItem.Clone());

            // Remove the original copy of the dragged item.
            lvProducts.Items.Remove(draggedItem);
        }

        #endregion

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddProductForm dlg = new AddProductForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProductEntity p = new ProductEntity();
                p.Name = dlg.Product_Name;
                p.Price = decimal.Parse(dlg.Product_Price);
                p.Description = dlg.Product_Description;

                myData.Products.Add(p);
                myData.SaveChanges();
                int i = 1;
                foreach (var item in dlg.Product_Images)
                {
                    string dir = "images";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    Bitmap bitmap = new Bitmap(item);
                    string imageName = Path.GetRandomFileName() + ".jpg";
                    bitmap.Save(Path.Combine(dir, imageName), ImageFormat.Jpeg);
                    var pi = new ProductImage
                    {
                        Name = imageName,
                        ProductId = p.Id,
                        Priority = i
                    };
                    i++;
                    myData.ProductImages.Add(pi);
                    myData.SaveChanges();
                }

                LoadProductListView();
            }
        }
    }
}
