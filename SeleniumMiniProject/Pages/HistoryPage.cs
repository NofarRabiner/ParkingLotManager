using NLog;
using OpenQA.Selenium;
using SeleniumTests.Models;

namespace SeleniumTests.Pages
{
    public class HistoryPage : BasePage, IHistoryPage
    {
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public HistoryPage(IWebDriver driver) : base(driver)
        {
            base._driver = driver;
        }

        public List<List<string>> GetHistoryTableData()
        {
            logger.Info("Retrieving history table data");
            var tableData = new List<List<string>>();
            var rows = _driver.FindElements(By.CssSelector("table tbody tr"));
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                var rowData = new List<string>();
                foreach (var cell in cells)
                {
                    rowData.Add(cell.Text);
                }
                tableData.Add(rowData);
            }
            return tableData;
        }
        public List<ParkingHistoryRecords> GetParkingHistoryRecords(List<List<string>> historyTableData)
        {
            var records = new List<ParkingHistoryRecords>();
            foreach (var row in historyTableData) 
            {
                var record = new ParkingHistoryRecords
                {
                    CarPlate = row[0],
                    Slot = row[1],
                    StartTime = DateTime.Parse(row[2]),
                    EndTime = DateTime.Parse(row[3]),
                    Fee = decimal.Parse(row[4]),
                    Image = row[5]
                };
                records.Add(record);
            }
            return records;
        }

        public void WaitForHistoryRow(string carPlate, int timeoutSeconds = 10)
        {
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(driver =>
            {
                var rows = driver.FindElements(By.CssSelector("table tbody tr"));
                return rows.Any(row =>
                {
                    var cells = row.FindElements(By.TagName("td"));
                    return cells.Count > 0 && cells[0].Text == carPlate;
                });
            });
        }

        public bool IsTheVehicleRegisteredInTheHistoryTable(List<ParkingHistoryRecords> historyRecordsData, ParkingHistoryRecords carNewRecord)
        {
            var isRegistered = historyRecordsData.Any(record =>
                  record.CarPlate.Trim() == carNewRecord.CarPlate.Trim() &&
                  record.Slot.Trim() == carNewRecord.Slot.Trim());
            return isRegistered;
        }
    }
}
