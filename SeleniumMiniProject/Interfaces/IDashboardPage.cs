using SeleniumTests.Models;


namespace SeleniumTests.Interfaces
{

        public interface IDashboardPage
        {
            void StartParkingProcess(ParkingHistoryRecords record);
            bool IsDuplicateParkingAlertPresent(int timeoutSeconds = 5);
            void TryParkingWithTheSameCar(string carNumber);
            string GetHeaderText();
            string GetAlertText();
            void TypeCarNumber(string carNum);
            void TypeSlot(string slotStr);
            void ScrollDown();
            void ClickStartParking();
            void ClickHistoryTab();
            void EndParking(ParkingHistoryRecords record);
        }

}
