Public Class ControladorPermisos

    Public Shared Sub CargarPermisos(ByRef Tree As TreeView, Optional Permisos As Entidades.PermisoCompuestoEntidad = Nothing)
        Try
            If Tree.Nodes.Count = 0 Then
                Dim gestorpermisos As Negocio.GestorPermisosBLL = New Negocio.GestorPermisosBLL
                If IsNothing(Permisos) Then

                    Dim lista = gestorpermisos.ListarPermisos()
                    ArmarArbol(lista, Nothing, Tree)
                    Tree.CollapseAll()
                Else
                    ArmarArbol(Permisos.Hijos, Nothing, Tree)
                    Tree.CollapseAll()
                End If

            Else
                If IsNothing(Permisos) Then
                    Tree.Nodes.Clear()
                    CargarPermisos(Tree)
                Else
                    Tree.Nodes.Clear()
                    CargarPermisos(Tree, Permisos)
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Public Shared Sub ArmarArbol(lista As List(Of Entidades.PermisoBaseEntidad), ByRef arbol As TreeNode, ByRef Tree As TreeView)
        If IsNothing(arbol) Then
            For Each Permiso In lista
                Dim nodo As TreeNode = New TreeNode With {.Value = Permiso.ID_Permiso, .Text = Permiso.Nombre, .SelectAction = TreeNodeSelectAction.None}
                Tree.Nodes.Add(nodo)
                If Permiso.tieneHijos Then
                    Dim GrupoPermiso = DirectCast(Permiso, Entidades.PermisoCompuestoEntidad)
                    ArmarArbol(GrupoPermiso.Hijos, nodo, Tree)
                End If
            Next
        Else
            For Each Permiso2 In lista
                Dim nodosegundo = New TreeNode With {.Value = Permiso2.ID_Permiso, .Text = Permiso2.Nombre, .SelectAction = TreeNodeSelectAction.None}
                arbol.ChildNodes.Add(nodosegundo)
                If Permiso2.tieneHijos Then
                    Dim GrupoPermiso = DirectCast(Permiso2, Entidades.PermisoCompuestoEntidad)
                    ArmarArbol(GrupoPermiso.Hijos, nodosegundo, Tree)
                End If
            Next
        End If
    End Sub


    Public Shared Sub CheckChildNodes(ByVal iNode As TreeNode)
        Try
            UnCheckParentNodes(iNode)
            For Each sNode As TreeNode In iNode.ChildNodes
                sNode.Checked = iNode.Checked
                CheckChildNodes(sNode)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub UnCheckParentNodes(ByVal iNode As TreeNode)
        Try
            If iNode.Checked = False AndAlso iNode.Parent IsNot Nothing Then
                iNode.Parent.Checked = False
                UnCheckParentNodes(iNode.Parent)
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Function RecorrerArbol(ByRef arbol As TreeNode, ByRef Compuesto As Entidades.PermisoCompuestoEntidad, ByRef Tree As TreeView) As Entidades.PermisoCompuestoEntidad
        If IsNothing(arbol) Then
            For Each nodo As TreeNode In Tree.Nodes
                If nodo.Checked = True Then
                    If nodo.ChildNodes.Count > 0 Then
                        Dim Permiso As New Entidades.PermisoCompuestoEntidad With {.ID_Permiso = nodo.Value, .Nombre = nodo.Text}
                        RecorrerArbol(nodo, Permiso, Tree)
                        If Not Permiso.esValido(Compuesto.Nombre) Then
                            Compuesto.agregarHijo(Permiso)
                        Else
                            Permiso.Hijos.Clear()
                        End If

                    Else
                        Dim Permiso As New Entidades.PermisoEntidad With {.ID_Permiso = nodo.Value, .Nombre = nodo.Text}
                        If Not Permiso.esValido(Compuesto.Nombre) Then
                            Compuesto.agregarHijo(Permiso)
                        End If
                    End If
                Else
                    RecorrerArbol(nodo, Compuesto, Tree)
                End If
            Next
        Else
            For Each nodo2 As TreeNode In arbol.ChildNodes
                If nodo2.Checked = True Then
                    If nodo2.ChildNodes.Count <> 0 Then
                        Dim Permiso As New Entidades.PermisoCompuestoEntidad With {.ID_Permiso = nodo2.Value, .Nombre = nodo2.Text}
                        If Not Permiso.esValido(Compuesto.Nombre) Then
                            RecorrerArbol(nodo2, Permiso, Tree)
                            Compuesto.agregarHijo(Permiso)
                        Else
                            RecorrerArbol(nodo2, Permiso, Tree)
                            Permiso.Hijos.Clear()
                        End If
                    Else
                        Dim Permiso As New Entidades.PermisoEntidad With {.ID_Permiso = nodo2.Value, .Nombre = nodo2.Text}
                        If Not Permiso.esValido(Compuesto.Nombre) Then
                            Compuesto.agregarHijo(Permiso)
                        End If
                    End If
                Else
                    If nodo2.ChildNodes.Count <> 0 Then
                        RecorrerArbol(nodo2, Compuesto, Tree)
                    End If
                End If
            Next

        End If
        Return Compuesto
    End Function

End Class
