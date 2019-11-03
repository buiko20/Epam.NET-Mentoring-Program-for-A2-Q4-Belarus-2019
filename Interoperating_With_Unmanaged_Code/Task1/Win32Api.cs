using System;
using System.Runtime.InteropServices;

namespace Task1
{
    public static class Win32Api
    {
        [DllImport("powrprof.dll", SetLastError = true)]
        internal static extern uint CallNtPowerInformation(
            POWER_INFORMATION_LEVEL informationLevel,
            [In] IntPtr lpInputBuffer,
            uint nInputBufferSize,
            [In, Out] IntPtr lpOutputBuffer,
            uint nOutputBufferSize
        );

        [DllImport("powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(
            bool bHibernate,
            bool bForce,
            bool bWakeupEventsDisabled
        );

        public enum POWER_INFORMATION_LEVEL
        {
            LastSleepTime = 15,
            LastWakeTime = 14,
            ProcessorInformation = 11,
            SystemBatteryState = 5,
            SystemExecutionState = 16,
            SystemPowerCapabilities = 4,
            SystemPowerInformation = 12,
            SystemPowerPolicyAc = 0,
            SystemPowerPolicyCurrent = 8,
            SystemPowerPolicyDc = 1,
            SystemReserveHiberFile = 10
        }

        public struct SYSTEM_BATTERY_STATE
        {
            public byte AcOnLine;
            public byte BatteryPresent;
            public byte Charging;
            public byte Discharging;
            public byte spare1;
            public byte spare2;
            public byte spare3;
            public byte spare4;
            public uint MaxCapacity;
            public uint RemainingCapacity;
            public int Rate;
            public uint EstimatedTime;
            public uint DefaultAlert1;
            public uint DefaultAlert2;
        }

        public struct SYSTEM_POWER_INFORMATION
        {
            public uint MaxIdlenessAllowed;
            public uint Idleness;
            public uint TimeRemaining;
            public byte CoolingMode;
        }
    }
}
