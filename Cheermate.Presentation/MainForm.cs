using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cheermate.Domain.Entities;
using Cheermate.Domain.Enums;
using Cheermate.Application.Repositories;

namespace Cheermate.Presentation
{
    public partial class MainForm : Form
    {
        private DataGridView tasksGrid = null!;
        private readonly BindingSource tasksBindingSource = new();
        private readonly ITodoTaskRepository _repo;

        public MainForm(ITodoTaskRepository repo)
        {
            _repo = repo;
            InitializeComponent();
            // Fire and forget; alternatively await in Shown event
            _ = LoadTasksAsync();
        }

        private void InitializeComponent()
        {
            tasksGrid = new DataGridView();
            SuspendLayout();

            tasksGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tasksGrid.Location = new Point(12, 12);
            tasksGrid.Name = "tasksGrid";
            tasksGrid.Size = new Size(760, 400);
            tasksGrid.AutoGenerateColumns = true;
            tasksGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tasksGrid.ReadOnly = true;
            tasksGrid.AllowUserToAddRows = false;

            ClientSize = new Size(784, 431);
            Controls.Add(tasksGrid);
            Text = "Cheermate Tasks";
            StartPosition = FormStartPosition.CenterScreen;

            tasksGrid.DataSource = tasksBindingSource;

            ResumeLayout(false);
        }

        private async Task LoadTasksAsync()
        {
            try
            {
                var tasks = await _repo.GetRecentAsync();
                tasksBindingSource.DataSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to load tasks: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
