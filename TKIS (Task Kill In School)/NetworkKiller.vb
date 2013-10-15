Imports System.Net.NetworkInformation

Public Class NetworkKiller

    Dim m_Networks As List(Of String)
    Dim m_Programs As List(Of String)

    Public ReadOnly Property Networks As List(Of String)
        Get
            Return m_Networks
        End Get
    End Property

    Public ReadOnly Property Programs As List(Of String)
        Get
            Return m_Programs
        End Get
    End Property

    Public Sub New()
        m_Networks = New List(Of String)
        m_Programs = New List(Of String)
    End Sub

    Friend Sub AddNetwork(ByVal network As String)
        m_Networks.Add(network)
    End Sub

    Friend Sub AddNetwork(ByVal networks As Specialized.StringCollection)
        For Each netw As String In networks
            m_Networks.Add(netw)
        Next
    End Sub

    Friend Sub AddProgram(ByVal program As String)

        If program.EndsWith(".exe") Then
            m_Programs.Add(program.Substring(0, program.Length - 4))
        Else
            m_Programs.Add(program)
        End If

    End Sub

    Friend Sub AddProgram(ByVal programs As Specialized.StringCollection)
        For Each prog As String In programs
            m_Programs.Add(prog)
        Next
    End Sub

    Public Sub netChanged()

        Dim ListOfDnsSuffixes As List(Of String) = GetDnsSuffix()
        ListOfDnsSuffixes.RemoveAll(Function(p) p = "")


        For Each network As String In Networks
            For Each suffix As String In ListOfDnsSuffixes
                If suffix.Contains(network) Then
                    KillTasks()
                    Exit Sub
                End If
            Next
        Next

    End Sub

    Private Sub KillTasks()

        For Each program As String In Programs

            Dim proc = Process.GetProcessesByName(program)
            For i As Integer = 0 To proc.Count - 1
                proc(i).Kill()
            Next i

        Next

    End Sub

    Private Function GetDnsSuffix() As List(Of String)

        Dim DnsList As New List(Of String)

        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        Dim adapter As NetworkInterface
        For Each adapter In adapters
            Dim properties As IPInterfaceProperties = adapter.GetIPProperties()

            DnsList.Add(properties.DnsSuffix)

        Next adapter

        Return DnsList

    End Function

End Class
