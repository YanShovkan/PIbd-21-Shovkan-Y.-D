using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.ViewModels;

namespace GiftShopView
{
    public partial class FormStorage : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly StorageLogic logic;

        private int? id;

        private Dictionary<int, (string, int)> storageMaterials;

        public FormStorage(StorageLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void LoadData()
        {
            try
            {
                if (storageMaterials != null)
                {
                    dataGridView.Rows.Clear();

                    foreach (var storageMaterial in storageMaterials)
                    {
                        dataGridView.Rows.Add(new object[]
                        {
                            storageMaterial.Key,
                            storageMaterial.Value.Item1,
                            storageMaterial.Value.Item2
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormStorage_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    StorageViewModel view = logic.Read(new StorageBindingModel { Id = id.Value })?[0];

                    if (view != null)
                    {
                        textBoxName.Text = view.StorageName;
                        textBoxManager.Text = view.StorageManager;
                        storageMaterials = view.StorageMaterials;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                storageMaterials = new Dictionary<int, (string, int)>();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(textBoxManager.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                StorageBindingModel storage = new StorageBindingModel
                {
                    Id = id,
                    StorageName = textBoxName.Text,
                    StorageManager = textBoxManager.Text,
                    StorageMaterials = storageMaterials,
                    DateCreate = DateTime.Now
                };

                logic.CreateOrUpdate(storage);                    

                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
