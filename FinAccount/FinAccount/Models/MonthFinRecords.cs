using System;
using System.Collections.Generic;
using System.Text;

namespace FinAccount.Models {
    public class MonthFinRecords : BaseFinRecords<FinRecord> {
        public MonthFinRecords(IEnumerable<FinRecord> monthRecords, string month) : base(monthRecords, month) { }

        protected override void CalculateSums() {
            foreach (var record in Records) {
                decimal sum = record.Sum;
                if (sum > 0)
                    PositiveSum += sum;
                else
                    NegativeSum += sum;
            }

            DiffSum = PositiveSum + NegativeSum;
        }
    }
}
