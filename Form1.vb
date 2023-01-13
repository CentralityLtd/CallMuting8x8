Imports System.DirectoryServices.AccountManagement
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Net
Imports System.Net.Http
Imports Microsoft.Win32
Imports System.Linq.Expressions

Public Class CallMuting8x8

    Shared ReadOnly client As HttpClient = New HttpClient()

    Public isMuted As Boolean = False
    Public CurrentTitle As String = ""
    Public ShownError As Boolean = False
    Public inStartup As Boolean = True
    Public ShownNoCall As Boolean = False

    Public LogFilename = "CallMuting8x8.log"

    Public MutingTitles As New List(Of String)
    Public LogFile As String = Path.Combine(Path.GetTempPath, LogFilename)
    Public TennantName As String = ""
    Public APIKey As String = ""
    Public ClusterURL As String = ""
    Public Debug As Boolean = False
    Public Manual As Boolean = False
    Public myEmail As String = ""
    Public FlashOnError As Boolean = True

    <DllImport("user32.dll", EntryPoint:="GetForegroundWindow")> Private Shared Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="GetWindowTextW")>
    Private Shared Function GetWindowTextW(<InAttribute()> ByVal hWnd As IntPtr, <OutAttribute(), MarshalAs(UnmanagedType.LPWStr)> ByVal lpString As StringBuilder, ByVal nMaxCount As Integer) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> Private Shared Function GetWindowTextLength(ByVal hwnd As IntPtr) As Integer
    End Function

    Private Sub CallMuting8x8_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Select Case e.CloseReason

            Case CloseReason.FormOwnerClosing, CloseReason.MdiFormClosing, CloseReason.UserClosing

                Me.WindowState = FormWindowState.Minimized
                Me.Hide()

                e.Cancel = True

            Case CloseReason.TaskManagerClosing

                NotifyIcon1.Visible = False
                NotifyIcon1.Dispose()

        End Select

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim _CallMutingTitles As String = System.Configuration.ConfigurationSettings.AppSettings("CallMutingTitles")
        Dim _logFile As String = System.Configuration.ConfigurationSettings.AppSettings("LogFilePath")
        Dim _TennantName As String = System.Configuration.ConfigurationSettings.AppSettings("TennantName")
        Dim _apiKey As String = Utility.Decrypt(System.Configuration.ConfigurationSettings.AppSettings("ApiKey"), "centrality@007")
        Dim _ClusterURL As String = System.Configuration.ConfigurationSettings.AppSettings("ClusterURL")
        Dim _debug As String = System.Configuration.ConfigurationSettings.AppSettings("Debug")
        Dim _manual As String = System.Configuration.ConfigurationSettings.AppSettings("Manual")
        Dim _overrideeMail As String = System.Configuration.ConfigurationSettings.AppSettings("OverrideEmail")
        Dim _overrideAccountName As String = System.Configuration.ConfigurationSettings.AppSettings("OverrideUsername")
        Dim _flashonerror As String = System.Configuration.ConfigurationSettings.AppSettings("FlashOnError")

        Dim ConfigErrors As New List(Of String)


        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12


        Dim InvalidAPI As Boolean = False

        If _logFile IsNot Nothing AndAlso _logFile <> "" Then

            ' _logfile allows us to override the path of the output log file

            LogFile = Path.Combine(_logFile, LogFilename)

        End If


        wlog("Initialising")


        If _TennantName IsNot Nothing AndAlso _TennantName <> "" Then

            TennantName = _TennantName
            tTennantName.Text = TennantName

        Else

            ConfigErrors.Add("'TennantName' has not been specified in the config file")

            InvalidAPI = True

        End If



        If _apiKey IsNot Nothing AndAlso _apiKey <> "" Then

            APIKey = _apiKey

        Else

            ConfigErrors.Add("'ApiKey' has not been specified in the config file")

            InvalidAPI = True

        End If


        If _ClusterURL IsNot Nothing AndAlso _ClusterURL <> "" Then

            If _ClusterURL.ToLower.StartsWith("https://") Then

                ClusterURL = _ClusterURL
                If Not ClusterURL.EndsWith("/") Then ClusterURL += "/"
                tURL.Text = ClusterURL

            Else

                ConfigErrors.Add("'CluserURL' setting is not valid")

                InvalidAPI = True

            End If

        Else

            ConfigErrors.Add("'CluserURL' has not been specified in the config file")

            InvalidAPI = True

        End If


        Dim eMailPattern As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Centrality\CallMuting8x8", "OverrideEmailPattern", "")



        If eMailPattern <> "" Then

            If Debug Then wlog("User specirfic email pattern override has been specified: " + eMailPattern)

            If _overrideAccountName IsNot Nothing AndAlso _overrideAccountName <> "" Then

                If Debug Then wlog("Using override account name: " + _overrideAccountName)

                eMailPattern = eMailPattern.ToLower.Replace("%username%", _overrideAccountName).ToLower

            Else

                eMailPattern = eMailPattern.ToLower.Replace("%username%", Environment.UserName).ToLower

            End If

            If Debug Then wlog("Determined eMail address for agent id: " + eMailPattern)

            myEmail = eMailPattern

        Else

            If _overrideeMail IsNot Nothing AndAlso _overrideeMail <> "" Then

                myEmail = _overrideeMail

            Else


                Try

                    Dim myPrincipalContext As New PrincipalContext(ContextType.Domain)

                    Dim myUserPrincipal As New UserPrincipal(myPrincipalContext)

                    myEmail = myUserPrincipal.Current.EmailAddress

                Catch ex As Exception

                    wlog("Error when retrieving user email address: " + ex.Message)

                End Try

            End If


        End If


        If myEmail = "" Then

            wlog("Looking for eMail address in Azure AD Tennant Info")

            Dim UserWPKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows NT\CurrentVersion\WorkplaceJoin", False)

            If UserWPKey IsNot Nothing Then

                Dim uAADNGC As RegistryKey = UserWPKey.OpenSubKey("AADNGC")

                If uAADNGC IsNot Nothing Then

                    For Each p In uAADNGC.GetSubKeyNames

                        Dim sk As RegistryKey = uAADNGC.OpenSubKey(p)

                        If sk IsNot Nothing Then

                            Dim tmpMail As String = sk.GetValue("UserID", "")

                            If tmpMail <> "" AndAlso tmpMail.Contains("@") Then

                                myEmail = tmpMail
                                Exit For

                            End If

                        End If

                    Next

                End If

            End If

            If myEmail = "" Then

                wlog("Looking for eMail address in ConnectEndpointPlugin Cache")

                Dim CachedUserRegKey As String = "Software\OneDeploy\ConnectEndpointPlugin"

                Dim LiveRegKey As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(CachedUserRegKey, False)

                myEmail = LiveRegKey.GetValue("LastUser", "")

                If myEmail = "" Then

                    Dim OfficeIdentityPath As String = "Software\Microsoft\Office\16.0\Common\Identity"

                    wlog("Looking for eMail address in Office Identity Key")

                    Dim OfficeIdentityKey As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(OfficeIdentityPath, True)

                    Dim tmpMail As String = OfficeIdentityKey.GetValue("ADUserName", "")

                    If tmpMail <> "" AndAlso tmpMail.Contains("@") Then myEmail = tmpMail

                    If myEmail = "" Then

                        wlog("Could not determine user's email address." + myEmail)

                        ConfigErrors.Add("Could not determine current user's email address")

                        InvalidAPI = True

                    Else

                        wlog("eMail address returned from Identity Provider: " + myEmail)

                    End If

                Else

                    wlog("eMail address returned from ConnectEndpointPlugin Cache: " + myEmail)

                End If

            Else

                wlog("eMail address returned from AzureAD: " + myEmail)

            End If

        End If


        teMail.Text = myEmail



        If _CallMutingTitles IsNot Nothing AndAlso _CallMutingTitles <> "" Then

            MutingTitles = _CallMutingTitles.ToLower.Split(";").ToList

        End If

        tTitles.Text = MutingTitles.Count

        If _manual.ToLower = "true" Then

            wlog("Manual mute / unmute is enabled")

            Manual = True

        End If


        FlashOnError = _flashonerror.ToLower = "true"


        If _debug.ToLower = "true" Then

            wlog("Running in Debug mode")

            Debug = True

        End If


        NotifyIcon1.Visible = True

        Dim UIDString As String = TennantName + ":" + APIKey

        Dim UID = Encoding.ASCII.GetBytes(UIDString)

        client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(UID))


        If ConfigErrors.Count > 0 Then

            SetNotifyIcon("Configuration Error", "Configuration issues within the Call Muting (8x8) application", 2, 30000)

            NotifyIcon1.ShowBalloonTip(999999)

            wlog("CALL MUTING IS NOT ACTIVE")

        Else

            If Not InvalidAPI Then

                wlog("Calling Resume API to ensure user exists and is the correct state")

                Select Case CallMute(False)

                    Case "notfound"

                        ConfigErrors.Add("Agent not found in 8x8 system: " + myEmail)

                        SetNotifyIcon("Agent not found", "Agent not found in 8x8 system: " + myEmail, 2, 30000)

                        wlog("ERROR: Agent not found in 8x8 system: " + myEmail)

                        wlog("CALL MUTING IS NOT ACTIVE")


                    Case "unauthorized"

                        ConfigErrors.Add("Cannot authenticate to the 8x8 system")

                        SetNotifyIcon("Could not authenticate ", "Cannot authenticate to the 8x8 system", 2, 30000)

                        wlog("ERROR: Could not authenticate to the 8x8 system")

                        wlog("CALL MUTING IS NOT ACTIVE")

                    Case "OK"

                        SetNotifyIcon("Call Muting OK", "8x8 Agent connected OK: " + myEmail, 1, 0)

                        wlog("Initialised OK")

                    Case Else

                        ConfigErrors.Add("Error communicating with the 8x8 service")

                        SetNotifyIcon("Connection Error", "Cannot communicate with the 8x8 service", 2, 30000)

                        wlog("ERROR: Could not connect to the 8x8 system")

                        wlog("CALL MUTING IS NOT ACTIVE")


                End Select

            End If

        End If

        If ConfigErrors.Count > 0 Then

            ConfigPanel.Visible = True

            configerrortext.Text = String.Join(vbCrLf, ConfigErrors)

        Else

            ScanTimer.Enabled = True

        End If

        inStartup = False

        'Me.Hide()

    End Sub

    Sub SetNotifyIcon(Title As String, Text As String, IconType As Integer, timeout As Integer)

        NotifyIcon1.BalloonTipTitle = Title
        NotifyIcon1.BalloonTipText = Text

        NotifyIcon1.Text = Text

        Select Case IconType

            Case 1

                Timer1.Enabled = False

                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
                NotifyIcon1.Icon = My.Resources.phone_ok

            Case 2

                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Error

                NotifyIcon1.Icon = My.Resources.phone_error

                If FlashOnError Then Timer1.Enabled = True

        End Select

        If timeout > 0 Then NotifyIcon1.ShowBalloonTip(timeout)

    End Sub

    Sub wlog(txt As String)

        Try

            File.AppendAllText(LogFile, Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + txt + vbCrLf)

        Catch ex As Exception

        End Try

    End Sub

    Private Sub NotifyIcon1_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseClick

        If Manual AndAlso e.Button = Windows.Forms.MouseButtons.Right Then

            If ManualMute.Visible = False Then

                ScanTimer.Enabled = False

                wlog("Manual mode started")

                ManualMute.ShowDialog()

                wlog("Manual mode ended")

                ScanTimer.Enabled = True

            End If

        End If

    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick

        If ManualMute.Visible = False Then

            Me.Show()
            Me.WindowState = FormWindowState.Normal


        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

        Me.WindowState = FormWindowState.Minimized
        Me.Visible = False

    End Sub

    Private Sub ScanTimer_Tick(sender As Object, e As EventArgs) Handles ScanTimer.Tick

        ScanTimer.Enabled = False

        Dim isTopWindow As Boolean = IsTopWindowByTitle()

        If isTopWindow Then

            'We have a valid foreground window matched to the list of Call Muting Titles

            If isMuted Then

                If Debug Then wlog("Application for muting has been found, but muting already active, so will not be called again")

            Else

                CallMute(True)

            End If

        Else

            If ShownNoCall Then

                wlog("App title no longer active, will stop attempts to mute")

                ShownNoCall = False

            End If

            If isMuted Then

                If Debug Then wlog("Application for muting no longer active, so will umute")

                CallMute(False)

            End If

        End If

        ScanTimer.Enabled = True

    End Sub

    Function CallMute(Mute As Boolean, Optional Manual As Boolean = False) As String

        Dim retval As String = "OK"

        Dim URL As String = ClusterURL + "api/tstats/rccontrol/"

        URL += IIf(Mute, "pause", "resume")

        URL += "/" + TennantName + "/email/" + myEmail

        If Debug Then wlog("Calling API: " + URL)


        Dim ReturnContent As HttpContent


        Try

            Dim response As HttpResponseMessage = client.PutAsync(URL, ReturnContent).Result

            If response.Content.ReadAsStringAsync.Status = TaskStatus.RanToCompletion Then

                If response.IsSuccessStatusCode Then

                    Dim resulttext As String = response.Content.ReadAsStringAsync.Result.ToLower

                    If ShownError Then

                        SetNotifyIcon("Call Muting OK", "8x8 Agent (re)connected OK: " + myEmail, 1, 500)
                        ShownError = False

                    End If

                    If Mute Then

                        If resulttext.Contains("active transaction does not exist") Then

                            If Not Manual Then

                                If (Not ShownNoCall) OrElse Debug Then wlog("Call mute attempted for app title '" + CurrentTitle + "', but 8x8 service reports no active call in progress.  Will keep re-trying.")

                                ShownNoCall = True

                            End If

                            retval = "No Call"

                        Else

                            ShownNoCall = False

                            isMuted = True

                            NotifyIcon1.Icon = My.Resources.phone_warn

                            If Not Manual Then

                                wlog("Call muted (Due to application: " + CurrentTitle + ")")

                            Else

                                wlog("Call muted (Manually invoked)")

                            End If

                        End If

                    Else

                        isMuted = False

                        NotifyIcon1.Icon = My.Resources.phone_ok

                        If Not inStartup Then

                            If Not Manual Then

                                wlog("Call unmuted")

                            Else

                                wlog("Call unmuted (Manually invoked)")

                            End If

                        End If


                    End If

                Else

                    NotifyIcon1.Icon = My.Resources.phone_error

                    If Not ShownError And Not inStartup Then

                        ShownError = True

                        SetNotifyIcon("Call Muting Error", "Cannot communicate with the 8x8 service", 2, 30000)

                    End If

                    wlog("ERROR: 8x8 API Call failure.  Return code: " + response.StatusCode.ToString)

                    If Not Mute Then isMuted = False

                    Return response.StatusCode.ToString.ToLower

                End If

            Else

                If Not Mute Then isMuted = False

            End If


        Catch ex As Exception

            wlog("ERROR: " + ex.Message)

        End Try

        Return retval

    End Function


    Private Function IsTopWindowByTitle() As Boolean

        Dim activeWindowHandle As IntPtr = GetForegroundWindow()

        If Not activeWindowHandle.Equals(IntPtr.Zero) Then

            Dim lgth As Integer = GetWindowTextLength(activeWindowHandle)
            Dim wTitle As New System.Text.StringBuilder("", lgth + 1)

            If lgth > 0 Then
                GetWindowTextW(activeWindowHandle, wTitle, wTitle.Capacity)
            End If

            If wTitle IsNot Nothing AndAlso wTitle.ToString <> "" Then

                If Debug AndAlso CurrentTitle <> wTitle.ToString Then wlog("Current application title: " + wTitle.ToString)

                CurrentTitle = wTitle.ToString

                If CurrentTitle <> "Call Muting Utility (8x8)" Then LastActive.Text = CurrentTitle

                Dim ctl As String = CurrentTitle.ToLower

                For Each p In MutingTitles

                    If ctl.Contains(p) Then

                        If Debug Then wlog("Application title matches to Call Muting Title:  " + p)

                        Return True

                    End If

                Next

            End If

        End If

        Return False

    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Static tick As Boolean = False

        If tick Then

            NotifyIcon1.Icon = My.Resources.phone_error

            tick = False

        Else

            NotifyIcon1.Icon = My.Resources.phone_cloud

            tick = True

        End If

    End Sub
End Class
