using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsCalculator
{
    public partial class CalculatorHistoryForm : Form
    {
        public CalculatorHistoryForm()
        {
            InitializeComponent();
        }

        public void UpdateHistory(string[] history)
        {
            listBoxHistory.Items.Clear();
            listBoxHistory.Items.AddRange(history);
        }
    }
}
