using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace GiftShopView
{
    public partial class FormGiftMaterial : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
       
        public int Id
        {
            get { return Convert.ToInt32(comboBoxMaterial.SelectedValue); }
            set { comboBoxMaterial.SelectedValue = value; }
        }
       
        public string MaterialName { get { return comboBoxMaterial.Text; } }
        
        public int Count
        {
            get { return Convert.ToInt32(textBoxCount.Text); }
            set
            {
                textBoxCount.Text = value.ToString();
            }
        }

        public FormGiftMaterial(MaterialLogic logic)
        {
            InitializeComponent();
            List<MaterialViewModel> list = logic.Read(null);
            if (list != null)
            {
                comboBoxMaterial.DisplayMember = "MaterialName";
                comboBoxMaterial.ValueMember = "Id";
                comboBoxMaterial.DataSource = list;
                comboBoxMaterial.SelectedItem = null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxMaterial.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
