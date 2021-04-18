using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;

namespace GiftShopView
{
    public partial class FormImplementer : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }
        private readonly ImplementerLogic logic;
        private int? id;


        public FormImplementer(ImplementerLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormImplementer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new ImplementerBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.Name;
                        textBoxWorkTime.Text = view.WorkingTime.ToString();
                        textBoxPauseTime.Text = view.PauseTime.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Вы не ввели имя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxWorkTime.Text))
            {
                MessageBox.Show("Вы не ввели время работы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPauseTime.Text))
            {
                MessageBox.Show("Вы не ввели время перерыва", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new ImplementerBindingModel
                {
                    Id = id,
                    Name = textBoxName.Text,
                    WorkingTime = Convert.ToInt32(textBoxWorkTime.Text),
                    PauseTime = Convert.ToInt32(textBoxPauseTime.Text)
                });
                MessageBox.Show("Сохранено", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
