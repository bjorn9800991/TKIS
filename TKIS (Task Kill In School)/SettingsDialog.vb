Imports System.Windows.Forms

Public Class SettingsDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        My.Settings.MinimizeToTray = chkMinimizeToTray.Checked
        My.Settings.KillTimer = chkKillTimer.Checked
        My.Settings.SaveNetworksPrograms = chkSaveProgramsNetworks.Checked

        My.Settings.Save()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SettingsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateGUI()
    End Sub

    ''' <summary>
    ''' Update the GUI so the controls reflect the current settings.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateGUI()
        chkMinimizeToTray.Checked = My.Settings.MinimizeToTray
        chkSaveProgramsNetworks.Checked = My.Settings.SaveNetworksPrograms
        chkKillTimer.Checked = My.Settings.KillTimer
    End Sub
End Class
