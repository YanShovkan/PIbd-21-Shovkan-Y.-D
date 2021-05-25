using System;
using System.Linq;
using System.Windows.Forms;
using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.BindingModels;
using Unity;

namespace GiftShopView
{
    public partial class FormMessages : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly MailLogic logic;
        private bool hasNext = false;
        private readonly int mailsOnPage = 2;
        private int currentPage = 0;

        public FormMessages(MailLogic logic)
        {
            InitializeComponent();
            if (mailsOnPage < 1) { mailsOnPage = 5; }
            this.logic = logic;
        }

        private void FormMessages_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var list = logic.Read(new MessageInfoBindingModel { ToSkip = currentPage * mailsOnPage, ToTake = mailsOnPage + 1 });
            hasNext = !(list.Count() <= mailsOnPage);
            if (hasNext)
            {
                buttonNext.Enabled = true;
            }
            else
            {
                buttonNext.Enabled = false;
            }
            if (list != null)
            {
                dataGridView.DataSource = list.Take(mailsOnPage).ToList();
                dataGridView.Columns[0].Visible = false;
                dataGridView.Columns[4].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (hasNext)
            {
                currentPage++;
                textBoxPage.Text = (currentPage + 1).ToString();
                buttonPrevious.Enabled = true;
                LoadData();
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if ((currentPage - 1) >= 0)
            {
                currentPage--;
                textBoxPage.Text = (currentPage + 1).ToString();
                buttonNext.Enabled = true;
                if (currentPage == 0)
                {
                    buttonPrevious.Enabled = false;
                }
                LoadData();
            }
        }
    }
}
