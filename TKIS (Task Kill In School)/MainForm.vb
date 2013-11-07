Public Class MainForm

    'Our class that handles the killing.
    Private netKill As NetworkKiller

    ''' <summary>
    ''' Clears the GUI and repoulates it from our killer.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshGUI()
        lstNetworks.Items.Clear()
        lstPrograms.Items.Clear()

        txtNetwork.Text = Nothing
        txtProgram.Text = Nothing

        'Get the networks and the programs from our killer and populate our lists.
        For Each network In netKill.Networks
            lstNetworks.Items.Add(network)
        Next

        For Each program In netKill.Programs
            lstPrograms.Items.Add(program)
        Next

    End Sub

    ''' <summary>
    ''' Update the stored settings from our killer.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateSettings()

        My.Settings.Programs.Clear()
        My.Settings.Networks.Clear()

        If My.Settings.SaveNetworksPrograms Then
            My.Settings.Programs.AddRange(netKill.Programs.ToArray)
            My.Settings.Networks.AddRange(netKill.Networks.ToArray)
        End If

        My.Settings.Save()

    End Sub

    ''' <summary>
    ''' On load create a new killer and load stored settings to the killer.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        netKill = New NetworkKiller

        LoadSettings()
        
        RefreshGUI()
    End Sub

    ''' <summary>
    ''' Load settings to our killer.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadSettings()

        'Initialize our setting variables.
        If My.Settings.Programs Is Nothing Then
            My.Settings.Programs = New Specialized.StringCollection
        End If

        If My.Settings.Networks Is Nothing Then
            My.Settings.Networks = New Specialized.StringCollection
        End If

        'Load the programs and networks if we should.
        If My.Settings.SaveNetworksPrograms Then
            netKill.AddProgram(My.Settings.Programs)
            netKill.AddNetwork(My.Settings.Networks)
        End If

        'Enable our timer if we should.
        EmergencyTimer.Enabled = My.Settings.KillTimer
    End Sub

#Region "Event Stubs"

    ''' <summary>
    ''' Adds a network to the killer, refresh the GUI and update settings.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddNetwork_Click(sender As Object, e As EventArgs) Handles btnAddNetwork.Click
        If txtNetwork.Text <> Nothing Then
            netKill.AddNetwork(txtNetwork.Text)
        End If

        RefreshGUI()
        UpdateSettings()
    End Sub

    ''' <summary>
    ''' Adds a program to the killer, refresh the GUI and update settings.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddProgram_Click(sender As Object, e As EventArgs) Handles btnAddProgram.Click
        If txtProgram.Text <> Nothing Then
            netKill.AddProgram(txtProgram.Text)
        End If

        RefreshGUI()
        UpdateSettings()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click, ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim ab As New AboutBox

        ab.ShowDialog()
    End Sub

    ''' <summary>
    ''' Handles the Enter key-press on the Network textbox.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtNetwork_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNetwork.KeyUp
        If e.KeyCode = Keys.Enter Then
            btnAddNetwork.PerformClick()
        End If
    End Sub

    ''' <summary>
    ''' Handles the Enter key-press on the Program textbox.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtProgram_KeyUp(sender As Object, e As KeyEventArgs) Handles txtProgram.KeyUp
        If e.KeyCode = Keys.Enter Then
            btnAddProgram.PerformClick()
        End If
    End Sub

    ''' <summary>
    ''' Handles the resize event, and more specifically the minimize "event".
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized And My.Settings.MinimizeToTray Then
            'Show the tray icon, and hide the main window.
            NotifyIcon1.Visible = True
            Me.Hide()
        End If
    End Sub

    ''' <summary>
    ''' Handles the Open menu item in the tray menu.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        'Makes the icon invisible and shows the MainForm.
        NotifyIcon1.Visible = False
        Me.Show()
    End Sub

    ''' <summary>
    ''' Handles the click event of the tray icon.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub NotifyIcon1_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            OpenToolStripMenuItem.PerformClick()
        End If
    End Sub


#End Region

    ''' <summary>
    ''' If our network availability has changed and we are connected to a network then run our killer.
    ''' </summary>
    ''' <param name="Available">Network availability status</param>
    ''' <remarks></remarks>
    Public Sub SetConnectionStatus(ByVal Available As Boolean)
        If Available Then
            netKill.netChanged()
        End If
    End Sub

    ''' <summary>
    ''' Opens the settings dialog, and if OK was pressed the settings should be updated.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Dim setting As New SettingsDialog
        If setting.ShowDialog() = Windows.Forms.DialogResult.OK Then
            UpdateSettings()
            EmergencyTimer.Enabled = My.Settings.KillTimer
        End If
    End Sub

    ''' <summary>
    ''' The Emergency timer's tick event will simulate a network change.
    ''' By default the the tick will occur once every 60 seconds.
    ''' </summary>p
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EmergencyTimer_Tick(sender As Object, e As EventArgs) Handles EmergencyTimer.Tick
        netKill.netChanged()
    End Sub

    Private Sub RemoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveToolStripMenuItem.Click

        'Convert our sender (the menuitem) to a menu item.
        Dim menuItem As ToolStripMenuItem = Convert.ChangeType(sender, GetType(ToolStripMenuItem))

        'Get the Owner of the MenuItem (i.e. the MenuStrip).
        Dim ctxmnu As ContextMenuStrip = menuItem.Owner

        'Get the listbox by getting the source control of the menustrip.
        Dim lstBox As ListBox = Convert.ChangeType(ctxmnu.SourceControl, GetType(ListBox))

        'We are removing directly from the killer object, so we have to know what lstBox that opened the menu.
        If lstBox.Name = "lstPrograms" Then
            'Remove the program from the killer.
            For i As Integer = 0 To lstBox.SelectedItems.Count - 1
                netKill.Programs.Remove(lstBox.SelectedItems(i).ToString())
            Next
        ElseIf lstBox.Name = "lstNetworks" Then
            'Remove the network from the killer.
            For i As Integer = 0 To lstBox.SelectedItems.Count - 1
                netKill.Networks.Remove(lstBox.SelectedItems(i).ToString())
            Next
        End If

        
        UpdateSettings()
        RefreshGUI()

    End Sub
End Class
