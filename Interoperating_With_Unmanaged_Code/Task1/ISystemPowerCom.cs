using System.Runtime.InteropServices;

namespace Task1
{
    [ComVisible(true)]
    [Guid("18918FEF-0743-4720-A596-F0E5B1965C09")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ISystemPowerCom
    {
        long GetLastSleepTime();

        long GetLastWakeTime();

        Win32Api.SYSTEM_BATTERY_STATE GetSystemBatteryState();

        Win32Api.SYSTEM_POWER_INFORMATION GetSystemPowerInformation();

        uint ReserveHibernationFile();

        uint DeleteHibernationFile();

        bool TurnOnSleepMode();
    }
}
