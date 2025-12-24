using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
using Tyuiu.ShelomentsevYA.Sprint7.V11.Lib;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11.Test
{
    [TestClass]
    public class DataServiceTest
    {
        private DataService dataService;

        [TestInitialize]
        public void Init()
        {
            dataService = new DataService();
        }

        // ================= LOAD CSV =================

        [TestMethod]
        public void LoadCsv_FileNotExists_ReturnsEmptyTable()
        {
            var table = dataService.LoadCsv("not_exists.csv");

            Assert.IsNotNull(table);
            Assert.AreEqual(0, table.Columns.Count);
            Assert.AreEqual(0, table.Rows.Count);
        }

        [TestMethod]
        public void LoadCsv_ValidCsv_LoadsCorrectly()
        {
            string path = "test.csv";

            File.WriteAllText(path,
                "Фамилия;Оклад\n" +
                "Иванов;50000\n" +
                "Петров;60000");

            var table = dataService.LoadCsv(path);

            Assert.AreEqual(2, table.Columns.Count);
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual("Иванов", table.Rows[0]["Фамилия"]);
            Assert.AreEqual("60000", table.Rows[1]["Оклад"]);

            File.Delete(path);
        }

        // ================= SAVE CSV =================

        [TestMethod]
        public void SaveCsv_CreatesFile()
        {
            var table = new DataTable();
            table.Columns.Add("Имя");
            table.Columns.Add("Оклад");

            table.Rows.Add("Анна", "70000");

            string path = "save_test.csv";

            dataService.SaveCsv(table, path);

            Assert.IsTrue(File.Exists(path));

            var content = File.ReadAllText(path);
            Assert.IsTrue(content.Contains("Анна"));

            File.Delete(path);
        }

        // ================= VALIDATION =================

        [TestMethod]
        public void IsNumericColumn_ReturnsTrue_ForNumeric()
        {
            var table = new DataTable();
            table.Columns.Add("Оклад");

            table.Rows.Add("50000");
            table.Rows.Add("60000");

            bool result = DataService.IsNumericColumn(table.Columns["Оклад"]);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNumericColumn_ReturnsFalse_ForText()
        {
            var table = new DataTable();
            table.Columns.Add("Оклад");

            table.Rows.Add("abc");

            bool result = DataService.IsNumericColumn(table.Columns["Оклад"]);

            Assert.IsFalse(result);
        }

        // ================= STATISTICS =================

        [TestMethod]
        public void GetSalaryStatistics_ReturnsCorrectValues()
        {
            var table = new DataTable();
            table.Columns.Add("Оклад");

            table.Rows.Add("50000");
            table.Rows.Add("70000");
            table.Rows.Add("60000");

            var stats = dataService.GetSalaryStatistics(table);

            Assert.AreEqual(3, stats.Count);
            Assert.AreEqual(50000, stats.Min);
            Assert.AreEqual(70000, stats.Max);
            Assert.AreEqual(60000, stats.Average);
        }

        [TestMethod]
        public void GetSalaryStatistics_EmptyTable_ReturnsZeros()
        {
            var table = new DataTable();
            table.Columns.Add("Оклад");

            var stats = dataService.GetSalaryStatistics(table);

            Assert.AreEqual(0, stats.Count);
            Assert.AreEqual(0, stats.Min);
            Assert.AreEqual(0, stats.Max);
            Assert.AreEqual(0, stats.Average);
        }
    }
}
