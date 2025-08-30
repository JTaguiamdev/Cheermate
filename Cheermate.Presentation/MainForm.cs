using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cheermate.Domain.Entities;
using Cheermate.Domain.Enums;

namespace Cheermate.Presentation
{
    public partial class MainForm : Form
    {
        private DataGridView tasksGrid;
        private BindingSource tasksBindingSource = new();
        private List<TodoTask> _tasks = new();

        public MainForm()
        {
            InitializeComponent();
            LoadSampleData();
        }

        private void InitializeComponent()
        {
            this.tasksGrid = new DataGridView();
            SuspendLayout();

            // tasksGrid
            this.tasksGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tasksGrid.Location = new Point(12, 12);
            this.tasksGrid.Name = "tasksGrid";
            this.tasksGrid.Size = new Size(760, 400);
            this.tasksGrid.AutoGenerateColumns = true;
            this.tasksGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.tasksGrid.ReadOnly = true;
            this.tasksGrid.AllowUserToAddRows = false;

            // Form
            this.ClientSize = new Size(784, 431);
            this.Controls.Add(this.tasksGrid);
            this.Text = "Cheermate Tasks";
            this.StartPosition = FormStartPosition.CenterScreen;

            ResumeLayout(false);
        }

        private void LoadSampleData()
        {
            _tasks.Add(new TodoTask
            {
                Title = "Demo Todo",
                Description = "Sample task",
                UserId = 1,
                Priority = TaskPriority.Medium,
                SubTasks = new List<SubTask>
                {
                    new SubTask { Title = "Sub 1", IsCompleted = true,  TaskId = 0 },
                    new SubTask { Title = "Sub 2", IsCompleted = false, TaskId = 0 }
                }
            });

            tasksBindingSource.DataSource = _tasks;
            tasksGrid.DataSource = tasksBindingSource;
        }
    }
}