Public Class frmQLSV
    'khai bao bien de truy xuat tu DB tu lop DataBaseAcess
    Private _DBAccess As New DataBaseAccess

    'khai bao bien trang thai kiem tra du lieu dang load
    Private _isLoading As Boolean = False

    'Dinh nghia thu tuc load du lieu tu bang lop vao comBobox
    Private Sub LoadDataOnComBobox()
        Dim sqlQuery As String = "SELECT ClassID,ClassName From dbo.Class"
        Dim dTable As DataTable = _DBAccess.GetDataTable(sqlQuery)
        Me.cmbClass.DataSource = dTable
        Me.cmbClass.ValueMember = "ClassID"
        Me.cmbClass.DisplayMember = "ClassName"

    End Sub

    'Dinh nghia du lieu thu tuc load du lieu tu bang sinh vien theo tung lop vao Gridview
    Private Sub loadDataOnGridView(ClassID As String)
        Dim sqlQuery As String = _
            String.Format("SELECT StudentID, StudenName, Phone, Address From dbo.Students Where ClassID = '{0}'", ClassID)
        Dim dTable As DataTable = _DBAccess.GetDataTable(sqlQuery)
        Me.dgvStudents.DataSource = dTable
        With Me.dgvStudents
            .Columns(0).HeaderText = "StudentID"
            .Columns(1).HeaderText = "StudenName"
            .Columns(2).HeaderText = "Phone"
            .Columns(3).HeaderText = "Address"
            .Columns(3).Width = 200
        End With
    End Sub


    Private Sub frmQLSV_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _isLoading = True           ' true khi du kieu bat dau load

        LoadDataOnComBobox()
        loadDataOnGridView(Me.cmbClass.SelectedValue)

        _isLoading = False          ' false khi load du lieu xong
    End Sub

    Private Sub cmbClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClass.SelectedIndexChanged
        If Not _isLoading Then    ' Neu load du lieu xong
            loadDataOnGridView(Me.cmbClass.SelectedValue)
        End If
    End Sub

    'Dinh nghia hien thi ket qua Search: theo phuong phap tuong tu - tim kiem tuong tu

    Private Sub SearchStudent(ClassID As String, value As String)
        Dim sqlQuery As String = _
            String.Format("SELECT StudentID, StudenName, Phone, Address From dbo.Students Where ClassID = '{0}'", ClassID)
        If Me.cmbSearch.SelectedIndex = 0 Then    'tim theo Student ID
            sqlQuery += String.Format("AND StudentID LIKE '{0}%'", value)
        ElseIf Me.cmbSearch.SelectedIndex = 1 Then    'tim theo Student Name
            sqlQuery += String.Format("AND StudenName LIKE '{0}%'", value)
        End If

        Dim dTable As DataTable = _DBAccess.GetDataTable(sqlQuery)
        Me.dgvStudents.DataSource = dTable
        With Me.dgvStudents
            .Columns(0).HeaderText = "StudentID"
            .Columns(1).HeaderText = "StudenName"
            .Columns(2).HeaderText = "Phone"
            .Columns(3).HeaderText = "Address"
            .Columns(3).Width = 200
        End With
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        SearchStudent(Me.cmbClass.SelectedValue, Me.txtSearch.Text)
    End Sub



    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim frm As New frmStudent(False)
        frm.txtClassID.Text = Me.cmbClass.SelectedValue
        frm.ShowDialog()
        If frm.DialogResult = Windows.Forms.DialogResult.OK Then
            'load du lieu
            loadDataOnGridView(Me.cmbClass.SelectedValue)
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim frm As New frmStudent(True)
        frm.txtClassID.Text = Me.cmbClass.SelectedValue
        frm.txtStudentID.ReadOnly = True    ' chi cho phep doc, truong nay k cho phep sua
        With Me.dgvStudents
            frm.txtStudentID.Text = .Rows(.CurrentCell.RowIndex).Cells("StudentID").Value
            frm.txtStudentName.Text = .Rows(.CurrentCell.RowIndex).Cells("StudenName").Value
            frm.txtPhone.Text = .Rows(.CurrentCell.RowIndex).Cells("Phone").Value
            frm.txtAddress.Text = .Rows(.CurrentCell.RowIndex).Cells("Address").Value

        End With
        frm.ShowDialog()
        If frm.DialogResult = Windows.Forms.DialogResult.OK Then ' Sua du lieu thanh cong thi load du lieu len Girdview 
            loadDataOnGridView(Me.cmbClass.SelectedValue)

        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ' khai bao bien lay StudentID ma hong can da xoa duoc tren Girdview
        Dim StudentID As String = Me.dgvStudents.Rows(Me.dgvStudents.CurrentCell.RowIndex).Cells("StudentID").Value

        ' Khai bao cau lenh Query de xoa
        Dim sqlQuery As String = String.Format("DELETE Students WHERE StudentID = '{0}'", StudentID)

        ' Thuc hien Xoa
        If _DBAccess.ExecuteNoneQuery(sqlQuery) Then   ' xao thanh cong thi thong bao
            MessageBox.Show("Da Xoa Du Lieu Thanh Cong!")
            ' load lai du lieu len Girdview
            loadDataOnGridView(Me.cmbClass.SelectedValue)
        Else
            MessageBox.Show("Loi Du Lieu!")
        End If

    End Sub
End Class
