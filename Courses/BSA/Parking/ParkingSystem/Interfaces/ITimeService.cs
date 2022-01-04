namespace ParkingSystem.Interfaces
{
    public interface ITimeService
    {
        event System.EventHandler PartTimeWent;
        event System.EventHandler FullTimeWent;
    }
}
