using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
using Tyuiu.ShelomentsevYA.Sprint7.V11.Lib;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11.Test
{
    [TestClass]
    public class DataServiceTest
    {
        private readonly DataService dataService = new DataService();

        [TestMethod]
        public void LoadCsv_ShouldReturnDataTable_WithCorrectRowsCount()
        {
            string path = "test.csv";
            File.WriteAllText(path,
                "Фамилия;Оклад\n" +
                "Иванов;50000\n" +
                "Петров;60000");

            DataTable table = dataService.LoadCsv(path);

            Assert.AreEqual(2, table.Rows.Count);

            File.Delete(path);
        }

        [TestMethod]
        public void SaveCsv_ShouldCreateFile()
        {
            string path = "save_test.csv";

            DataTable table = new DataTable();
            table.Columns.Add("Фамилия");
            table.Columns.Add("Оклад");

            table.Rows.Add("Иванов", "50000");

            dataService.SaveCsv(table, path);

            Assert.IsTrue(File.Exists(path));

            File.Delete(path);
        }

        [TestMethod]
        public void GetSalaryStatistics_ShouldReturnCorrectValues()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Оклад");

            table.Rows.Add("10000");
            table.Rows.Add("20000");
            table.Rows.Add("30000");

            var stats = dataService.GetSalaryStatistics(table);

            Assert.AreEqual(3, stats.count);
            Assert.AreEqual(10000, stats.min);
            Assert.AreEqual(30000, stats.max);
            Assert.AreEqual(20000, stats.avg);
        }
    }
}
