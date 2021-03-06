﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsCalculator
{
    public partial class CalculatorForm : Form
    {
        private readonly int RIGHT_INDENT_PIXELS = 8;
        private const PriorityMode DEFAULT_PRIORITY_MODE = PriorityMode.LeftToRight;

        private readonly Font HISTORY_FONT = new Font("Segoe UI Semibold", 13);
        private readonly Color HISTORY_COLOR = Color.FromArgb(0, 134, 134, 134);
        private readonly CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

        private PriorityMode priorityMode = DEFAULT_PRIORITY_MODE;
        
        private Calculator calculator;
        private List<string> history;
        private CalculatorHistoryForm historyForm;

        public CalculatorForm()
        {
            InitializeComponent();

            calculator = new Calculator(cultureInfo);
            history = new List<string>();

            AttachEventHandlers();
            // Initialize display at launch
            UpdateDisplay();

            InitializePriorityMenu();
            InitializeDecimalSeparator();

            // TODO: keyboard support
            // TODO: change memory button styling
        }

        private void InitializePriorityMenu()
        {
            // This is executed at startup to check the correct menuItem.
            SetPriorityMode(this.priorityMode);
        }

        private void InitializeDecimalSeparator()
        {
            this.buttonDecimalSeparator.Text = this.cultureInfo.NumberFormat.NumberDecimalSeparator;
        }

        private void AttachEventHandlers()
        {
            // All buttons are inside the tableLayoutPanel
            var controls = tableLayoutPanel.Controls;
            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    control.Click += (s, e) => UpdateDisplay();
                }
            }
        }

        private void UpdateDisplay()
        {
            // Prevent layout shifts when adjusting formatting
            richTextBoxResult.SuspendLayout();

            var equation = calculator.GetEquation();
            // Align first line properly. This is NBSP;
            if (equation.Length == 0) equation = "\u00A0";
            var currentOperand = calculator.GetCurrentOperand();
            richTextBoxResult.Text = $"{equation}{Environment.NewLine}{currentOperand}";

            // Alignment
            richTextBoxResult.SelectAll();
            richTextBoxResult.SelectionAlignment = HorizontalAlignment.Right;
            richTextBoxResult.SelectionRightIndent = RIGHT_INDENT_PIXELS;
            richTextBoxResult.DeselectAll();

            // Equation font size
            richTextBoxResult.Select(0, equation.Length);
            richTextBoxResult.SelectionFont = HISTORY_FONT;
            richTextBoxResult.SelectionColor = HISTORY_COLOR;
            richTextBoxResult.DeselectAll();

            richTextBoxResult.ResumeLayout();

            // TODO: these two can use data binding
            buttonMemoryClear.Enabled = calculator.HasMemory;
            buttonMemoryRead.Enabled = calculator.HasMemory;
            // TODO: change memory button styling
        }

        private void HandleNumberButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var value = button.Text[0];
            calculator.EnterKey(value);
        }

        private void HandleDecimalSeparatorClick(object sender, EventArgs e)
        {
            calculator.EnterDecimalSeparator();
        }

        private void HandleAddition(object sender, EventArgs e)
        {
            calculator.EnterOperator(Operator.Addition);
        }

        private void HandleSubtraction(object sender, EventArgs e)
        {
            calculator.EnterOperator(Operator.Subtraction);
        }

        private void HandleMultiplication(object sender, EventArgs e)
        {
            calculator.EnterOperator(Operator.Multiplication);
        }

        private void HandleDivision(object sender, EventArgs e)
        {
            calculator.EnterOperator(Operator.Division);
        }

        private void HandlePower(object sender, EventArgs e)
        {
            calculator.EnterOperator(Operator.Power);
        }

        private void HandleSquareRoot(object sender, EventArgs e)
        {
            calculator.EnterOperator(Operator.Sqrt);
        }

        private void HandleInvertSign(object sender, EventArgs e)
        {
            calculator.InvertSign();
        }

        private void HandleClearEntry(object sender, EventArgs e)
        {
            calculator.ClearEntry();
        }

        private void HandleClear(object sender, EventArgs e)
        {
            calculator.Clear();
        }

        private void HandleBackspace(object sender, EventArgs e)
        {
            calculator.Backspace();
        }

        private void HandleCalculate(object sender, EventArgs e)
        {
            calculator.Calculate();
            // Equation ends with " =" so we need a space between it and result
            var historyEntry = $"{calculator.GetEquation()} {calculator.GetCurrentOperand()}";
            history.Add(historyEntry);

            UpdateHistory();
        }

        private void HandleSaveAs(object sender, EventArgs e)
        {
            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var stream = saveFileDialog.OpenFile();
                var streamWriter = new StreamWriter(stream);
                using (streamWriter)
                {
                    string allHistory = string.Join(Environment.NewLine, history);
                    streamWriter.Write(allHistory);
                    MessageBox.Show($"Saved history in {saveFileDialog.FileName}.", "Success", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void HandleClearHistory(object sender, EventArgs e)
        {
            history.Clear();
            MessageBox.Show("History was cleared", "History", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateHistory();
        }

        private void HandleShowHistory(object sender, EventArgs e)
        {
            ShowHistory();
        }

        private void HandleOpenHistory(object sender, EventArgs e)
        {
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var stream = openFileDialog.OpenFile();
                var streamReader = new StreamReader(stream);

                using (streamReader)
                {
                    history.Clear();

                    string line = streamReader.ReadLine();
                    while (line != null)
                    {
                        history.Add(line);
                        line = streamReader.ReadLine();
                    }

                    var result2 = MessageBox.Show($"Loaded history from {openFileDialog.FileName}.{Environment.NewLine}Show history?", "LoadedHistory", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (result2 == DialogResult.Yes)
                    {
                        ShowHistory();
                    }
                }
            }
        }

        private void ShowHistory()
        {
            historyForm = new CalculatorHistoryForm();

            historyForm.Show();
            UpdateHistory();
        }

        private void UpdateHistory()
        {
            // TODO: what if form is closed?
            if (historyForm != null)
            {
                historyForm.UpdateHistory(history.ToArray());
            }
        }

        private void HandlePercent(object sender, EventArgs e)
        {
            calculator.Percentify();
        }

        private void SetAlgebraicPriority(object sender, EventArgs e)
        {
            SetPriorityMode(PriorityMode.Algebraic);
        }

        private void SetLeftToRightPriority(object sender, EventArgs e)
        {
            SetPriorityMode(PriorityMode.LeftToRight);
        }

        private void SetPriorityMode(PriorityMode priorityMode)
        {
            ResetPriorityMenuItems();

            switch (priorityMode)
            {
                case PriorityMode.Algebraic:
                    this.algebraicToolStripMenuItem.Checked = true;
                    break;
                case PriorityMode.LeftToRight:
                    this.lefttorightToolStripMenuItem.Checked = true;
                    break;
                default:
                    throw new Exception($"Invariant: Unexpected priority mode: {this.priorityMode}"); ;
            }

            this.calculator.SetPriorityMode(priorityMode);
            this.UpdateDisplay();
        }

        private void ResetPriorityMenuItems()
        {
            var priorityMenuItems = this.priorityToolStripMenuItem.DropDownItems;
            foreach (ToolStripMenuItem menuItem in priorityMenuItems)
            {
                menuItem.Checked = false;
            }
        }

        private void HandleMemoryClear(object sender, EventArgs e)
        {
            calculator.MemoryClear();
        }

        private void HandleMemoryRead(object sender, EventArgs e)
        {
            calculator.MemoryRead();
        }

        private void HandleMemoryAdd(object sender, EventArgs e)
        {
            calculator.MemoryAdd();
        }

        private void HandleMemorySubtract(object sender, EventArgs e)
        {
            calculator.MemorySubtract();
        }
    }
}
