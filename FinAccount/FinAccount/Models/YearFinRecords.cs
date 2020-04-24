using System;
using System.Collections.Generic;
using System.Text;

namespace FinAccount.Models {
    public class YearFinRecords : BaseFinRecords<MonthFinRecords> {
        public YearFinRecords(IEnumerable<MonthFinRecords> yearRecords, string year) : base(yearRecords, year) { }

        protected override void CalculateSums() {
            foreach (var record in Records) {
                PositiveSum += record.PositiveSum;
                NegativeSum += record.NegativeSum;
            }

            PositiveSum = decimal.Round(PositiveSum);
            NegativeSum = decimal.Round(NegativeSum);
            DiffSum = PositiveSum + NegativeSum;
        }
    }
}
