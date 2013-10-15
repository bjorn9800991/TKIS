Imports System.Windows.Forms

Public Class SettingsDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        My.Settings.MinimizeToTray = chkMinimizeToTray.Checked
        My.Settings.KillTimer = chkKillTimer.Checked
        My.Settings.SaveNetworksPrograms = chkSaveProgramsNetworks.Checked

        My.Settings.Programs.Clear()
        My.Settings.Networks.Clear()

        If chkSaveProgramsNetworks.Checked Then
            My.Settings.Programs.AddRange(MainForm.netKill.Programs.ToArray)
            My.Settings.Networks.AddRange(MainForm.netKill.Networks.ToArray)
        End If

        My.Settings.Save()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SettingsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If My.Settings.Programs Is Nothing Then
            My.Settings.Programs = New Specialized.StringCollection
        End If

        If My.Settings.Networks Is Nothing Then
            My.Settings.Networks = New Specialized.StringCollection
        End If

        UpdateGUI()
    End Sub

    Private Sub UpdateGUI()
        chkMinimizeToTray.Checked = My.Settings.MinimizeToTray
        chkSaveProgramsNetworks.Checked = My.Settings.SaveNetworksPrograms
        chkKillTimer.Checked = My.Settings.KillTimer
    End Sub
End Class
