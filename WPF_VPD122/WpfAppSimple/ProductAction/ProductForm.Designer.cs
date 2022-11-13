using System.Windows.Forms;

namespace WpfAppSimple.ProductAction
{
    partial class ProductForm
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
            this.lvProducts = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProductInfo = new System.Windows.Forms.Button();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvProducts
            // 
            this.lvProducts.AllowDrop = true;
            this.lvProducts.Location = new System.Drawing.Point(10, 62);
            this.lvProducts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvProducts.MultiSelect = false;
            this.lvProducts.Name = "lvProducts";
            this.lvProducts.Size = new System.Drawing.Size(806, 338);
            this.lvProducts.TabIndex = 0;
            this.lvProducts.UseCompatibleStateImageBehavior = false;
            this.lvProducts.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvProducts_ItemDrag);
            this.lvProducts.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvProducts_DragDrop);
            this.lvProducts.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvProducts_DragEnter);
            this.lvProducts.DragOver += new System.Windows.Forms.DragEventHandler(this.lvProducts_DragOver);
            this.lvProducts.DragLeave += new System.EventHandler(this.lvProducts_DragLeave);
            this.lvProducts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvProducts_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Список продуктів";
            // 
            // btnProductInfo
            // 
            this.btnProductInfo.Location = new System.Drawing.Point(680, 16);
            this.btnProductInfo.Name = "btnProductInfo";
            this.btnProductInfo.Size = new System.Drawing.Size(134, 36);
            this.btnProductInfo.TabIndex = 2;
            this.btnProductInfo.Text = "Змінити товар";
            this.btnProductInfo.UseVisualStyleBackColor = true;
            this.btnProductInfo.Click += new System.EventHandler(this.btnProductInfo_Click);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Location = new System.Drawing.Point(485, 16);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(101, 36);
            this.btnAddProduct.TabIndex = 3;
            this.btnAddProduct.Text = "Додати товар";
            this.btnAddProduct.UseVisualStyleBackColor = true;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // ProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 418);
            this.Controls.Add(this.btnAddProduct);
            this.Controls.Add(this.btnProductInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvProducts);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ProductForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LvProducts_DragEnter(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private ListView lvProducts;
        private Label label1;
        private Button btnProductInfo;
        private Button btnAddProduct;
    }
}