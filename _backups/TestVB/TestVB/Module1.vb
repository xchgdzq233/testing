Imports log4net

Module Module1
    Private ReadOnly logger As ILog = LogManager.GetLogger(GetType(Module1))

    Sub Main()
        logger.Info("testing info")
        'logger.Error("testing ERROR!")
        Console.ReadLine()
    End Sub

End Module
