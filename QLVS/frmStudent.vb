Public Class frmStudent
    'khai bao bien truy xuat DB tu lop DBAccess
    Private _DBAccess As New DataBaseAccess

    'khai bao trang thai dang la Edu=it hay insert
    Private _isdit As Boolean = False

    Public Sub New(IsEdit As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _isdit = IsEdit

    End Sub


    'Dinh nghia Ham them ban ghi sinh vien vao database
    Private Function InsertStudent() As Boolean
        Dim sqlQuery As String = "INSERT INTO Students (StudentID, StudenName, Phone, Address, ClassID)"
        sqlQuery += String.Format("VALUES('{0}','{1}','{2}','{3}','{4}')", _
                                  txtStudentID.Text, txtStudentName.Text, txtPhone.Text, txtAddress.Text, txtClassID.Text)
        Return _DBAccess.ExecuteNoneQuery(sqlQuery)
    End Function

    'Dinh nghia ham Update
    Private Function UpdateStudent() As String
        Dim sqlQuery As String = String.Format("UPDATE Students SET StudenName = '{0}', Phone = '{1}', Address = '{2}' WHERE StudentID = '{3}'", _
                                               Me.txtStudentName.Text, Me.txtPhone.Text, Me.txtAddress.Text, Me.txtStudentID.Text)
        Return _DBAccess.ExecuteNoneQuery(sqlQuery)
    End Function




    'Dinh nghia ham kiem tra gia tri truoc khi insert du lieu vao database
    Private Function IsEmpty() As Boolean
        Return (String.IsNullOrEmpty(txtStudentID.Text) OrElse String.IsNullOrEmpty(txtStudentName.Text) OrElse _
               String.IsNullOrEmpty(txtPhone.Text) OrElse String.IsNullOrEmpty(txtAddress.Text) OrElse _
               String.IsNullOrEmpty(txtClassID.Text))

    End Function

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If IsEmpty() Then  ' kiem tra truong du lieu truoc khi thuc hien Them, Xoa
            MessageBox.Show("Hay nhap gia tri vao truoc khi vao database", "Error", MessageBoxButtons.OK)
        Else
            If _isdit Then  ' Neu Edit thi goi ham Update
                If UpdateStudent() Then ' Neu Update thanh cong thi hien thong bao
                    MessageBox.Show("Sua du lieu thanh cong!", "Information", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                Else         ' Neu co loi khi sua thi thong bao loi 
                    MessageBox.Show("Loi sua du lieu", "Error", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.No
                End If
            Else            ' neu khong phai thi goi ham Insert
                If InsertStudent() Then   'Neu insert thanh cong thi thong bao
                    MessageBox.Show("Them du lieu thanh cong!", "Information", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                Else
                    MessageBox.Show("Loi them du lieu!", "Error", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.No
                End If
            End If

            Me.Close()
        End If
    End Sub


    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


End Class