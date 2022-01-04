using System;
using System.Threading;

namespace ParkingSystem.TimeServices
{
    public class DefaultTimeService : Interfaces.ITimeService, IDisposable
    {
        // FIELDS
        Timer partTimer;
        Timer fullTimer;

        // CONSTRUCTORS
        public DefaultTimeService(int partTimeFromSecond, int fullTimeFromMinute)
        {

            partTimer = new Timer((e) =>
            {
                OnPartTimeWent(EventArgs.Empty);
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(partTimeFromSecond));

            fullTimer = new Timer((e) =>
            {
                OnFullTimeWent(EventArgs.Empty);
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(fullTimeFromMinute));
        }

        // EVENTS
        public event EventHandler PartTimeWent;
        public event EventHandler FullTimeWent;

        // METHODS
        protected void OnPartTimeWent(EventArgs eventArgs)
        {
            PartTimeWent?.Invoke(this, eventArgs);
        }
        protected void OnFullTimeWent(EventArgs eventArgs)
        {
            FullTimeWent?.Invoke(this, eventArgs);
        }
        public void Dispose()
        {
            partTimer?.Dispose();
            fullTimer?.Dispose();
        }
    }
}
