using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using Cheermate.Domain.Entities;
using Cheermate.Domain.Enums;
using Cheermate.Application.Repositories;

namespace Cheermate.Presentation
{
    public partial class MainForm : Form
    {
        private DataGridView tasksGrid;
        private BindingSource tasksBindingSource = new();
        private List<TodoTask> _tasks = new();
        private readonly ITodoTaskRepository _todoTaskRepository;

        public MainForm(ITodoTaskRepository todoTaskRepository)
        {
            _todoTaskRepository = todoTaskRepository;
            InitializeComponent();
            LoadDataAsync();
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
            this.tasksGrid.AutoGenerateColumns = false; // We'll define columns manually
            this.tasksGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.tasksGrid.AllowUserToAddRows = false;
            this.tasksGrid.AllowUserToDeleteRows = false;

            // Setup columns manually for better control
            SetupGridColumns();

            // Handle cell value changes for checkboxes
            this.tasksGrid.CellValueChanged += TasksGrid_CellValueChanged;
            this.tasksGrid.CurrentCellDirtyStateChanged += TasksGrid_CurrentCellDirtyStateChanged;

            // Form
            this.ClientSize = new Size(784, 431);
            this.Controls.Add(this.tasksGrid);
            this.Text = "Cheermate Tasks";
            this.StartPosition = FormStartPosition.CenterScreen;

            ResumeLayout(false);
        }

        private async void LoadDataAsync()
        {
            try
            {
                _tasks = await _todoTaskRepository.GetAllAsync();
                tasksBindingSource.DataSource = _tasks;
                tasksGrid.DataSource = tasksBindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupGridColumns()
        {
            tasksGrid.Columns.Clear();

            // Title column
            tasksGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                Name = "Title",
                HeaderText = "Title",
                Width = 200,
                ReadOnly = true
            });

            // Description column
            tasksGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                Name = "Description",
                HeaderText = "Description",
                Width = 250,
                ReadOnly = true
            });

            // Priority column
            tasksGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Priority",
                Name = "Priority",
                HeaderText = "Priority",
                Width = 80,
                ReadOnly = true
            });

            // IsCompleted checkbox column
            tasksGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsCompleted",
                Name = "IsCompleted",
                HeaderText = "Completed",
                Width = 80,
                ReadOnly = false
            });

            // IsOverdue checkbox column (readonly - calculated property)
            tasksGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsOverdue",
                Name = "IsOverdue",
                HeaderText = "Overdue",
                Width = 80,
                ReadOnly = true
            });

            // IsUpcoming checkbox column (readonly - calculated property)
            tasksGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsUpcoming",
                Name = "IsUpcoming",
                HeaderText = "Upcoming",
                Width = 80,
                ReadOnly = true
            });

            // Due Date column
            tasksGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DueDate",
                Name = "DueDate",
                HeaderText = "Due Date",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "MM/dd/yyyy" }
            });
        }

        private async void TasksGrid_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = tasksGrid.Columns[e.ColumnIndex].Name;
                if (columnName == "IsCompleted")
                {
                    var task = _tasks[e.RowIndex];
                    if (task != null)
                    {
                        try
                        {
                            await _todoTaskRepository.UpdateAsync(task);
                            // Refresh the calculated properties by rebinding
                            tasksBindingSource.ResetCurrentItem();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error updating task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void TasksGrid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            // Commit the edit immediately for checkboxes
            if (tasksGrid.IsCurrentCellDirty && tasksGrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                tasksGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}