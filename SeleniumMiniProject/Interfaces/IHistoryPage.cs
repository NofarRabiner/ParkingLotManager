using SeleniumTests.Models;
using System.Collections.Generic;

namespace SeleniumTests.Pages
{
    public interface IHistoryPage
    {
        List<List<string>> GetHistoryTableData();
        List<ParkingHistoryRecords> GetParkingHistoryRecords(List<List<string>> historyTableData);
        void WaitForHistoryRow(string carPlate, int timeoutSeconds = 10);
        bool IsTheVehicleRegisteredInTheHistoryTable(List<ParkingHistoryRecords> historyRecordsData, ParkingHistoryRecords carNewRecord);
    }
}