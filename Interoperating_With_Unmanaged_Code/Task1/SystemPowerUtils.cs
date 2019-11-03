using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Task1
{
    public static class SystemPowerUtils
    {
        public static long GetLastSleepTime()
        {
            return GetLastTime(Win32Api.POWER_INFORMATION_LEVEL.LastSleepTime);
        }

        public static long GetLastWakeTime()
        {
            return GetLastTime(Win32Api.POWER_INFORMATION_LEVEL.LastWakeTime);
        }

        public static Win32Api.SYSTEM_BATTERY_STATE GetSystemBatteryState()
        {
            IntPtr buffer = IntPtr.Zero;
            try
            {
                CallNtPowerInformation(out buffer, Win32Api.POWER_INFORMATION_LEVEL.SystemBatteryState, typeof(Win32Api.SYSTEM_BATTERY_STATE));
                var result = Marshal.PtrToStructure<Win32Api.SYSTEM_BATTERY_STATE>(buffer);
                return result;
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(buffer);
                }
            }
        }

        public static Win32Api.SYSTEM_POWER_INFORMATION GetSystemPowerInformation()
        {
            IntPtr buffer = IntPtr.Zero;
            try
            {
                CallNtPowerInformation(out buffer, Win32Api.POWER_INFORMATION_LEVEL.SystemPowerInformation, typeof(Win32Api.SYSTEM_POWER_INFORMATION));
                var result = Marshal.PtrToStructure<Win32Api.SYSTEM_POWER_INFORMATION>(buffer);
                return result;
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(buffer);
                }
            }
        }

        public static uint ReserveHibernationFile()
        {
            return ReserveHibernationFile(true);
        }

        public static uint DeleteHibernationFile()
        {
            return ReserveHibernationFile(false);
        }

        public static bool TurnOnSleepMode()
        {
            const bool bHibernate = true;
            const bool bForce = true;
            const bool bWakeupEventsDisabled = true;
            return Win32Api.SetSuspendState(bHibernate, bForce, bWakeupEventsDisabled);
        }

        private static uint ReserveHibernationFile(bool reserve)
        {
            IntPtr buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(uint)));
                if (buffer == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                Marshal.WriteInt32(buffer, Convert.ToInt32(reserve));

                uint ntStatus = Win32Api.CallNtPowerInformation(
                        Win32Api.POWER_INFORMATION_LEVEL.SystemReserveHiberFile,
                        buffer,
                        (uint)Marshal.SizeOf<uint>(),
                        IntPtr.Zero,
                        0
                );

                return ntStatus;
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(buffer);
                }
            }
        }

        private static long GetLastTime(Win32Api.POWER_INFORMATION_LEVEL level)
        {
            IntPtr buffer = IntPtr.Zero;
            try
            {
                CallNtPowerInformation(out buffer, level, typeof(long));

                // Receives a ULONGLONG that specifies the interrupt-time count, in 100-nanosecond units, at the last system sleep time
                // there are 1e9 nanoseconds in a second, so there are 1e7 100 - nanoseconds in a second.
                long lastSleepTime = Marshal.ReadInt64(buffer, ofs: 0);
                long lastSleepTimeInSeconds = lastSleepTime / 10000000;
                return lastSleepTimeInSeconds;
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(buffer);
                }
            }
        }

        private static void CallNtPowerInformation(out IntPtr buffer, Win32Api.POWER_INFORMATION_LEVEL level, Type sizeFor)
        {
            buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(sizeFor));
            if (buffer == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            uint ntStatus = Win32Api.CallNtPowerInformation(
                level,
                IntPtr.Zero,
                0,
                buffer,
                Convert.ToUInt32(Marshal.SizeOf(sizeFor))
            );

            if (ntStatus != 0)
            {
                throw new Win32Exception(Convert.ToInt32(ntStatus), $"CallNtPowerInformation returns status code {ntStatus}");
            }
        }
    }
}
