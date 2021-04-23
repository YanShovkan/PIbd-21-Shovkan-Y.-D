using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.ViewModels;
using System.Windows.Forms;
using Unity;

namespace GiftShopView
{
    public partial class FormGift : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly GiftLogic logic;
        private int? id;
        private Dictionary<int, (string, int)> giftMaterials;
        public FormGift(GiftLogic service)
        {
            InitializeComponent();
            this.logic = service;
        }
              
        private void LoadData()
        {
            try
            {
                if (giftMaterials != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var pc in giftMaterials)
                    {
                        dataGridView.Rows.Add(new object[] { pc.Key, pc.Value.Item1,
                            pc.Value.Item2 });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormGiftMaterial>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (giftMaterials.ContainsKey(form.Id))
                {
                    giftMaterials[form.Id] = (form.MaterialName, form.Count);
                }
                else
                {
                    giftMaterials.Add(form.Id, (form.MaterialName, form.Count));
                }
                LoadData();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormGiftMaterial>();
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = giftMaterials[id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    giftMaterials[form.Id] = (form.MaterialName, form.Count);
                    LoadData();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {

                        giftMaterials.Remove(Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (giftMaterials == null || giftMaterials.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new GiftBindingModel
                {
                    Id = id,
                    GiftName = textBoxName.Text,
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    GiftMaterials = giftMaterials
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormGift_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    GiftViewModel view = logic.Read(new GiftBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.GiftName;
                        textBoxPrice.Text = view.Price.ToString();
                        giftMaterials = view.GiftMaterials;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
            else
            {
                giftMaterials = new Dictionary<int, (string, int)>();
            }
        }
    }
}

