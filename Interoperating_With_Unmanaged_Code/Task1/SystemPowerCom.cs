using System.Runtime.InteropServices;

namespace Task1
{
    [ComVisible(true)]
    [Guid("4C374E4B-299D-423F-A8A0-B819BE621671")]
    [ClassInterface(ClassInterfaceType.None)]
    public class SystemPowerCom : ISystemPowerCom
    {
        public long GetLastSleepTime()
        {
            return SystemPowerUtils.GetLastSleepTime();
        }

        public long GetLastWakeTime()
        {
            return SystemPowerUtils.GetLastWakeTime();
        }

        public Win32Api.SYSTEM_BATTERY_STATE GetSystemBatteryState()
        {
            return SystemPowerUtils.GetSystemBatteryState();
        }

        public Win32Api.SYSTEM_POWER_INFORMATION GetSystemPowerInformation()
        {
            return SystemPowerUtils.GetSystemPowerInformation();
        }

        public uint ReserveHibernationFile()
        {
            return SystemPowerUtils.ReserveHibernationFile();
        }

        public uint DeleteHibernationFile()
        {
            return SystemPowerUtils.DeleteHibernationFile();
        }

        public bool TurnOnSleepMode()
        {
            return SystemPowerUtils.TurnOnSleepMode();
        }
    }
}
