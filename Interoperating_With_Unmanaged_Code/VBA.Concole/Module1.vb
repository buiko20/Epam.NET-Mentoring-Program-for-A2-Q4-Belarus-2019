Module Module1

    Delegate Function GetCurrentThreadDelegate() As IntPtr
    Delegate Function GetProcessIdOfThreadDelegate(thread As IntPtr) As UInteger

    Sub Main()

        Dim com As Object = CreateObject("Task1.SystemPowerCom")
        Console.WriteLine($"LastSleepTime: {com.GetLastSleepTime()}")
        Console.WriteLine($"LastWakeTime: {com.GetLastWakeTime()}")

        Dim comWrapper = CreateObject("ExtraTask.ComWrapper")
        Dim getCurrentThreadIdWrapper = comWrapper.WrapProcedure("kernel32.dll", "GetCurrentThread", GetType(GetCurrentThreadDelegate))
        Console.WriteLine(getCurrentThreadIdWrapper.Library)
        Console.WriteLine(getCurrentThreadIdWrapper.Procedure)
        Dim hThread = getCurrentThreadIdWrapper.Delegate.DynamicInvoke()
        Console.WriteLine(hThread)

        Dim wrapper = comWrapper.WrapProcedure("kernel32.dll", "GetProcessIdOfThread", GetType(GetProcessIdOfThreadDelegate))
        Console.WriteLine(wrapper.Library)
        Console.WriteLine(wrapper.Procedure)
        Console.WriteLine(wrapper.Delegate.DynamicInvoke(hThread))

        Console.ReadKey()
    End Sub

End Module
