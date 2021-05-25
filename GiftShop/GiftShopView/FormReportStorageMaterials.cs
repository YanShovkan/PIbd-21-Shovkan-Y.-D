using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.ViewModels;
using Unity;

namespace GiftShopView
{
    public partial class FormReportStorageMaterials : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;
        public FormReportStorageMaterials(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormReportStorageMaterials_Load(object sender, EventArgs e)
        {
            try
            {
                MethodInfo method = logic.GetType().GetMethod("GetStorageMaterials");
                var materials = (List<ReportStorageMaterialsViewModel>)method.Invoke(logic, null);
                if (materials != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var material in materials)
                    {
                        dataGridView.Rows.Add(new object[] { material.StorageName, "", "" });
                        foreach (var listElem in material.Materials)
                        {
                            dataGridView.Rows.Add(new object[] { "", listElem.Item1, listElem.Item2 });
                        }
                        dataGridView.Rows.Add(new object[] { "Итого", "", material.TotalCount });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        MethodInfo method = logic.GetType().GetMethod("SaveStorageMaterialsToExcelFile");
                        method.Invoke(logic, new object[] { new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        }});
                       
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
