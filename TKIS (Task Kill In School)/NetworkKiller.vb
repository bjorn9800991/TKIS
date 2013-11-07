Imports System.Net.NetworkInformation

Public Class NetworkKiller

    Dim m_Networks As List(Of String)
    Dim m_Programs As List(Of String)

    ''' <summary>
    ''' The networks we are going to check for.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Networks As List(Of String)
        Get
            Return m_Networks
        End Get
    End Property

    ''' <summary>
    ''' The programs we are going to terminate.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Programs As List(Of String)
        Get
            Return m_Programs
        End Get
    End Property

    ''' <summary>
    ''' Creates a new instance of this class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        m_Networks = New List(Of String)
        m_Programs = New List(Of String)
    End Sub

    ''' <summary>
    ''' Add a network as a string to our list.
    ''' </summary>
    ''' <param name="network"></param>
    ''' <remarks></remarks>
    Friend Sub AddNetwork(ByVal network As String)
        If m_Networks.Contains(network) = False Then
            m_Networks.Add(network)
        End If
    End Sub

    ''' <summary>
    ''' Add a StringCollection to our networks list.
    ''' </summary>
    ''' <param name="networks"></param>
    ''' <remarks></remarks>
    Friend Sub AddNetwork(ByVal networks As Specialized.StringCollection)
        For Each netw As String In networks
            If m_Networks.Contains(netw) = False Then
                m_Networks.Add(netw)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Add a program as a string to our list.
    ''' </summary>
    ''' <param name="program"></param>
    ''' <remarks></remarks>
    Friend Sub AddProgram(ByVal program As String)

        Dim nameNoExtension As String

        If program.EndsWith(".exe") Then
            nameNoExtension = program.Substring(0, program.Length - 4)
        Else
            nameNoExtension = program
        End If

        If m_Programs.Contains(nameNoExtension) = False Then
            m_Programs.Add(nameNoExtension)
        End If

    End Sub

    ''' <summary>
    ''' Add a StringCollection to our programs list.
    ''' </summary>
    ''' <param name="programs"></param>
    ''' <remarks></remarks>
    Friend Sub AddProgram(ByVal programs As Specialized.StringCollection)
        For Each prog As String In programs
            If m_Programs.Contains(prog) = False Then
                m_Programs.Add(prog)
            End If
        Next
    End Sub

    ''' <summary>
    ''' If the net availability has changed, then run the killer.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub netChanged()

        'Get a list of all the DNS suffixes of all the networks the computer is connected to.
        Dim ListOfDnsSuffixes As List(Of String) = GetDnsSuffix()
        'Remove empty lines.
        ' ListOfDnsSuffixes.RemoveAll(Function(p) p = "")

        ListOfDnsSuffixes.Add("kth.se")

        'Loop through each suffix.
        For Each suffix As String In ListOfDnsSuffixes
            If Networks.Contains(suffix) Then
                KillTasks()
                Exit For
            End If
        Next

    End Sub

    ''' <summary>
    ''' Kills the processes that we should.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub KillTasks()

        'Look for each program and kill it.
        For Each program As String In Programs

            'Get all processes with this name.
            Dim proc = Process.GetProcessesByName(program)
            For i As Integer = 0 To proc.Count - 1

                'Tries to close the program by closing the main window.
                proc(i).CloseMainWindow()

                'Wait 5000 ms (=5s) for the program to exit.
                'If the process doesn't exit then kill it.
                If proc(i).WaitForExit(5000) = False Then
                    Try
                        proc(i).Kill()
                    Catch ex As Exception

                    End Try
                End If

            Next i

        Next

    End Sub

    ''' <summary>
    ''' Get the DNS suffixes of all the networks you are connected to.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDnsSuffix() As List(Of String)

        Dim DnsList As New List(Of String)

        'Get all network interfaces.
        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()

        'For each network adapter/interface, get the IP-properties.
        For Each adapter As NetworkInterface In adapters
            Dim properties As IPInterfaceProperties = adapter.GetIPProperties()

            'If the DNS-suffix isn't anything then add it to our list.
            If properties.DnsSuffix.Trim <> Nothing Then
                DnsList.Add(properties.DnsSuffix)
            End If

        Next adapter

        Return DnsList

    End Function

End Class
