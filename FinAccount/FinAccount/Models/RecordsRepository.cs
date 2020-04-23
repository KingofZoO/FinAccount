using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinAccount.Models {
    public class RecordsRepository {
        private SQLiteConnection database;

        public RecordsRepository(string path) {
            database = new SQLiteConnection(path);
            database.CreateTable<FinRecord>();
        }

        public IEnumerable<FinRecord> GetRecords() {
            return database.Table<FinRecord>().ToList();
        }

        public IEnumerable<FinRecord> MonthRecords() {
            return database.Query<FinRecord>("select * from FinRecords where Date >= ?", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        }

        public IEnumerable<FinRecord> RecordsByNote(string note) {
            return database.Query<FinRecord>("select * from FinRecords where Note = ?", note);
        }

        public void SaveRecord(FinRecord record) {
            if (record.Id != 0)
                database.Update(record);
            else
                database.Insert(record);
        }

        public void DropHistory() {
            database.DropTable<FinRecord>();
            database.CreateTable<FinRecord>();
        }
    }
}
