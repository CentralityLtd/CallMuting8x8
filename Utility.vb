Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class Utility

    Private Shared ReadOnly initVectorBytes As Byte() = Encoding.ASCII.GetBytes("sf353sdv4sdsd4df")
    Private Const keysize As Integer = 256

    Public Shared Function Decrypt(ByVal cipherText As String, ByVal passPhrase As String) As String

        If cipherText IsNot Nothing Then

            Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)

            Try
                Dim password As PasswordDeriveBytes = New PasswordDeriveBytes(passPhrase, Nothing)
                Dim keyBytes As Byte() = password.GetBytes(keysize / 8)

                Using symmetricKey As RijndaelManaged = New RijndaelManaged()
                    symmetricKey.Mode = CipherMode.CBC

                    Using decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

                        Using memoryStream As MemoryStream = New MemoryStream(cipherTextBytes)

                            Using cryptoStream As CryptoStream = New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)
                                Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}
                                Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
                                Return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
                            End Using
                        End Using
                    End Using
                End Using

            Catch

            End Try

        End If

        Return String.Empty

    End Function

End Class
