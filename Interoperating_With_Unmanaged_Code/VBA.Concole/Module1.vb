Module Module1

    Sub Main()
        Dim com As Object = CreateObject("Task1.SystemPowerCom")
        Console.WriteLine($"LastSleepTime: {com.GetLastSleepTime()}")
        Console.WriteLine($"LastWakeTime: {com.GetLastWakeTime()}")
        Console.ReadKey()
    End Sub

End Module
