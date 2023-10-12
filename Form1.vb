Imports System.Data.Odbc

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim mysqlconODBC As New OdbcConnection("dsn=pruebaOdbc; uid= root; pwd=;")
        Dim dt As DataTable
        Dim da As OdbcDataAdapter
        Dim ds As New DataSet

        Dim consulta As String = "SELECT * FROM productos"
        Try

            da = New OdbcDataAdapter(consulta, mysqlconODBC)
            mysqlconODBC.Open()
            da.Fill(ds)
            mysqlconODBC.Close()
            dt = ds.Tables(0)
            DataGridView1.DataSource = dt
            Label2.Text = "Conexion fue exitosa"
        Catch ex As Exception
            MsgBox("Error : " & ex.Message)
        End Try


        Dim sqlserverconODBC As New OdbcConnection("dsn=SQLSERVER; uid=Admin; pwd=Admin;")
        Dim dt2 As DataTable
        Dim da2 As OdbcDataAdapter
        Dim ds2 As New DataSet

        Dim consulta2 As String = "SELECT * FROM Producto"
        Try

            da2 = New OdbcDataAdapter(consulta2, sqlserverconODBC)
            sqlserverconODBC.Open()
            da2.Fill(ds2)
            sqlserverconODBC.Close()
            dt2 = ds2.Tables(0)
            DataGridView2.DataSource = dt2
            Label2.Text = "Conexion fue exitosa"
        Catch ex As Exception
            MsgBox("Error : " & ex.Message)
        End Try



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        Try
            ' Consulta SQL para seleccionar datos de MySQL
            Dim selectQuery As String = "SELECT * FROM Productos"

            ' Conexión ODBC a MySQL
            Using connectionMySQL As New OdbcConnection("dsn=pruebaOdbc; uid= root; pwd=;")
                connectionMySQL.Open()

                ' Comando para ejecutar la consulta en MySQL
                Using cmdMySQL As New OdbcCommand(selectQuery, connectionMySQL)
                    Using reader As OdbcDataReader = cmdMySQL.ExecuteReader()
                        ' Conexión ODBC a SQL Server
                        Using connectionSQLServer As New OdbcConnection("dsn=SQLSERVER; uid=Admin; pwd=Admin;")
                            connectionSQLServer.Open()

                            ' Consulta SQL para insertar datos en SQL Server
                            Dim insertQuery As String = "INSERT INTO Producto (ID_Producto,Nombre_Producto,Descripcion,Precio_Unitario,Stock_Disponible,ID_Categoria) VALUES (?, ?, ?, ?, ?, ?)"

                            ' Comando para insertar datos en SQL Server
                            Using cmdSQLServer As New OdbcCommand(insertQuery, connectionSQLServer)
                                While reader.Read()
                                    ' Obtener valores de MySQL y asignarlos a parámetros en SQL Server
                                    cmdSQLServer.Parameters.Clear()
                                    cmdSQLServer.Parameters.AddWithValue("@param1", reader("ID_Producto"))
                                    cmdSQLServer.Parameters.AddWithValue("@param2", reader("Nombre_Producto"))
                                    cmdSQLServer.Parameters.AddWithValue("@param3", reader("Descripcion"))
                                    cmdSQLServer.Parameters.AddWithValue("@param4", reader("Precio_Unitario"))
                                    cmdSQLServer.Parameters.AddWithValue("@param5", reader("Stock_Disponible"))
                                    cmdSQLServer.Parameters.AddWithValue("@param6", reader("ID_Categoria"))
                                    ' Ejecutar la inserción en SQL Server
                                    cmdSQLServer.ExecuteNonQuery()
                                End While
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            MessageBox.Show("Datos migrados exitosamente de MySQL a SQL Server.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error al migrar datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
