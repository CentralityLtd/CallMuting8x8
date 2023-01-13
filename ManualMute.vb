Public Class ManualMute

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Button1.Text = "Mute" Then

            CallMuting8x8.wlog("User manually initiated Mute")

            Dim MyResult As String = CallMuting8x8.CallMute(True, True)

            Select Case MyResult

                Case "OK"

                    Button1.Text = "Unmute"

                    Label1.Visible = False

                Case "No Call"

                    Label1.Visible = True

            End Select


        Else

            If Button1.Text = "Unmute" Then

                CallMuting8x8.wlog("User manually initiated Unmute")

                Label1.Visible = False

                Dim MyResult As String = CallMuting8x8.CallMute(False, True)

                Select Case MyResult

                    Case "OK"

                        Button1.Text = "Mute"

                End Select

            End If

        End If

    End Sub

End Class