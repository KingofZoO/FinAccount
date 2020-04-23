using System;
using SQLite;

namespace FinAccount.Models {
    [Table("FinRecords")]
    public class FinRecord {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        [Ignore]
        public string DateToString { get => Date.ToString("d"); }

        [Ignore]
        public int DayOfDate { get => Date.Day; }
    }
}
