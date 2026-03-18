using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using income_and_expenses_tracker.models;
using income_and_expenses_tracker.BLL;
using income_and_expenses_tracker.DAL;

namespace income_and_expenses_tracker
{
    /// <summary>
    /// Main dashboard form - Primary user interface
    /// </summary>
    public partial class MainForm : Form
    {
        private DateTime currentMonth;
        private List<Income> allIncomes;
        private List<Expense> allExpenses;

        public MainForm()
        {
            InitializeComponent();

            currentMonth = DateTime.Now;
            allIncomes = new List<Income>();
            allExpenses = new List<Expense>();

            ApplyTheme();
            SetupFormLayout();
        }

        /// <summary>
        /// Apply dark theme styling
        /// </summary>
        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            this.Text = "💰 Income & Expenses Tracker";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Setup form layout and controls
        /// </summary>
        private void SetupFormLayout()
        {
            // This will be designed in the Windows Forms Designer
            // For now, setting up in code with comments

            // Main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 40)
            };

            // Title label
            Label titleLabel = new Label
            {
                Text = "Financial Dashboard",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            mainPanel.Controls.Add(titleLabel);
            this.Controls.Add(mainPanel);
        }

        /// <summary>
        /// Load data from database
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Load incomes and expenses
                allIncomes = IncomeService.GetAllIncomes();
                allExpenses = ExpenseService.GetAllExpenses();

                // Update dashboard
                UpdateDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading data: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update dashboard with current data
        /// </summary>
        private void UpdateDashboard()
        {
            try
            {
                int year = currentMonth.Year;
                int month = currentMonth.Month;

                decimal monthlyIncome = IncomeService.GetMonthlyIncome(year, month);
                decimal monthlyExpense = ExpenseService.GetMonthlyExpense(year, month);
                decimal balance = monthlyIncome - monthlyExpense;

                DisplayStatistics(monthlyIncome, monthlyExpense, balance);
                RefreshTransactionList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error updating dashboard: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Display financial statistics
        /// </summary>
        private void DisplayStatistics(decimal income, decimal expense, decimal balance)
        {
            try
            {
                // This would update UI labels in the designer
                Console.WriteLine($"===== Financial Summary =====");
                Console.WriteLine($"Month: {currentMonth:MMMM yyyy}");
                Console.WriteLine($"Total Income:  ${income:F2}");
                Console.WriteLine($"Total Expense: ${expense:F2}");
                Console.WriteLine($"Net Balance:   ${balance:F2}");
                Console.WriteLine($"=============================");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying statistics: {ex.Message}");
            }
        }

        /// <summary>
        /// Refresh transaction list
        /// </summary>
        private void RefreshTransactionList()
        {
            try
            {
                int year = currentMonth.Year;
                int month = currentMonth.Month;

                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var monthlyIncomes = IncomeService.GetIncomesByDateRange(startDate, endDate);
                var monthlyExpenses = ExpenseService.GetExpensesByDateRange(startDate, endDate);

                // Display transactions
                Console.WriteLine($"\n===== Transactions for {currentMonth:MMMM yyyy} =====");
                Console.WriteLine("\nINCOMES:");
                foreach (var income in monthlyIncomes)
                {
                    Console.WriteLine($"  {income.ToString()}");
                }

                Console.WriteLine("\nEXPENSES:");
                foreach (var expense in monthlyExpenses)
                {
                    Console.WriteLine($"  {expense.ToString()}");
                }
                Console.WriteLine("================================\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing transaction list: {ex.Message}");
            }
        }

        /// <summary>
        /// Navigate to previous month
        /// </summary>
        public void PreviousMonth()
        {
            currentMonth = currentMonth.AddMonths(-1);
            UpdateDashboard();
        }

        /// <summary>
        /// Navigate to next month
        /// </summary>
        public void NextMonth()
        {
            currentMonth = currentMonth.AddMonths(1);
            UpdateDashboard();
        }

        /// <summary>
        /// Navigate to current month
        /// </summary>
        public void CurrentMonth()
        {
            currentMonth = DateTime.Now;
            UpdateDashboard();
        }

        /// <summary>
        /// Open Add Income form
        /// </summary>
        public void OpenAddIncomeForm()
        {
            try
            {
                using (var form = new AddIncomeForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Add Income form: {ex.Message}");
            }
        }

        /// <summary>
        /// Open Add Expense form
        /// </summary>
        public void OpenAddExpenseForm()
        {
            try
            {
                using (var form = new AddExpenseForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Add Expense form: {ex.Message}");
            }
        }

        /// <summary>
        /// Form load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                // Initialize database
                DatabaseManager.Initialize();

                // Load data
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error initializing application: {ex.Message}",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Get current month display
        /// </summary>
        public string GetCurrentMonthDisplay()
        {
            return currentMonth.ToString("MMMM yyyy");
        }

        /// <summary>
        /// Get summary for display
        /// </summary>
        public dynamic GetDashboardSummary()
        {
            try
            {
                return ExpenseService.GetFinancialSummary();
            }
            catch
            {
                return null;
            }
        }
    }
}