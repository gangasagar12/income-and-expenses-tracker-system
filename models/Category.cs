
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace income_and_expenses_tracker.Models
{
    public class Category
    {
        public int Categoryid { get; set; }

        [Column(TypeName ="nvchar(50")]
        public string Name { get; set; }

        public string Title { get; set; }

        public decimal Amount { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; } // Income or Expense
    }
}