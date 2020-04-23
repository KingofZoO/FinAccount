using System;
using System.Collections.Generic;
using System.Text;

namespace FinAccount.Models {
    public abstract class BaseFinRecords<T> {
        public IEnumerable<T> Records { get; protected set; }
        public string Name { get; protected set; }
        public decimal PositiveSum { get; protected set; }
        public decimal NegativeSum { get; protected set; }
        public decimal DiffSum { get; protected set; }

        public BaseFinRecords(IEnumerable<T> monthList, string name) {
            Records = monthList;
            Name = name;

            CalculateSums();
        }

        protected abstract void CalculateSums();
    }
}
