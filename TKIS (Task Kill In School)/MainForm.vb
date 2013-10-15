Public Class MainForm

    Friend netKill As NetworkKiller

    Private Sub RefreshGUI()
        lstNetworks.Items.Clear()
        lstPrograms.Items.Clear()

        For Each network In netKill.Networks
            lstNetworks.Items.Add(network)
        Next

        For Each program In netKill.Programs
            lstPrograms.Items.Add(program)
        Next

        txtNetwork.Text = Nothing
        txtProgram.Text = Nothing

    End Sub

    Private Sub UpdateSettings()

        If My.Settings.SaveNetworksPrograms Then
            My.Settings.Programs.Clear()
            My.Settings.Networks.Clear()

            My.Settings.Programs.AddRange(netKill.Programs.ToArray)
            My.Settings.Networks.AddRange(netKill.Networks.ToArray)
        End If

        My.Settings.Save()

    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        netKill = New NetworkKiller

        If My.Settings.SaveNetworksPrograms Then
            netKill.AddProgram(My.Settings.Programs)
            netKill.AddNetwork(My.Settings.Networks)
        End If

        EmergencyTimer.Enabled = My.Settings.KillTimer

        EmergencyTimer.Start()

        RefreshGUI()
    End Sub

    Private Sub btnAddNetwork_Click(sender As Object, e As EventArgs) Handles btnAddNetwork.Click
        If txtNetwork.Text <> Nothing Then
            netKill.AddNetwork(txtNetwork.Text)
        End If

        RefreshGUI()
        UpdateSettings()
    End Sub

    Private Sub btnAddProgram_Click(sender As Object, e As EventArgs) Handles btnAddProgram.Click

        If txtProgram.Text <> Nothing Then
            netKill.AddProgram(txtProgram.Text)
        End If

        RefreshGUI()
        UpdateSettings()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim ab As New AboutBox

        ab.ShowDialog()
    End Sub

    Private Sub txtNetwork_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNetwork.KeyUp
        If e.KeyCode = Keys.Enter Then
            btnAddNetwork.PerformClick()
        End If
    End Sub

    Private Sub txtProgram_KeyUp(sender As Object, e As KeyEventArgs) Handles txtProgram.KeyUp
        If e.KeyCode = Keys.Enter Then
            btnAddProgram.PerformClick()
        End If
    End Sub

    Public Sub SetConnectionStatus(ByVal Available As Boolean)
        If Available Then
            netKill.netChanged()
        End If
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Dim setting As New SettingsDialog
        If setting.ShowDialog() = Windows.Forms.DialogResult.OK Then
            EmergencyTimer.Enabled = My.Settings.KillTimer
        End If
    End Sub

    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized And My.Settings.MinimizeToTray Then
            NotifyIcon1.Visible = True
            Me.Hide()
        End If
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        NotifyIcon1.Visible = False
        Me.Show()
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub NotifyIcon1_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            OpenToolStripMenuItem.PerformClick()
        End If
    End Sub

    Private Sub EmergencyTimer_Tick(sender As Object, e As EventArgs) Handles EmergencyTimer.Tick
        netKill.netChanged()
    End Sub

End Class
