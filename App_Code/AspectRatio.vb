'Aptify e-Business 5.5.1, July 2013
Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web


Public Class AspectRatio
   
    Private d_Width As Integer = 0
    Private d_Height As Integer = 0
    Public Property Width() As Integer
        Get
            Return d_Width
        End Get
        Set(ByVal value As Integer)
            d_Width = value
        End Set
    End Property
    Public Property Height() As Integer
        Get
            Return d_Height
        End Get
        Set(ByVal value As Integer)
            d_Height = value
        End Set
    End Property
    ''' <summary>
    ''' Methord For Calculate Hight and Width
    ''' </summary>
    ''' <param name="aWidth"></param>
    ''' <param name="aHeight"></param>
    ''' <param name="dWidth"></param>
    ''' <param name="dHeight"></param>
    Public Sub WidthAndHeight(ByVal aWidth As Integer, ByVal aHeight As Integer, ByVal dWidth As Integer, ByVal dHeight As Integer)
        Dim height As Double = aHeight
        Dim width As Double = aWidth
        Dim rWidht As Double = Convert.ToDouble(dWidth)
        Dim rHeight As Double = Convert.ToDouble(dHeight)
        Dim fWidth As Integer = 0
        Dim fHeight As Integer = 0
        Dim hRatio As Double = 0.0
        Dim vRatio As Double = 0.0
        If width > rWidht Then
            hRatio = (rWidht / width)
            vRatio = (rHeight / height)

            If vRatio > hRatio Then
                fWidth = Convert.ToInt32(CDbl(width) * hRatio)
                fHeight = Convert.ToInt32(CDbl(height) * hRatio)
            Else
                fWidth = Convert.ToInt32(CDbl(width) * vRatio)
                fHeight = Convert.ToInt32(CDbl(height) * vRatio)

            End If
        ElseIf rWidht > width Then
            hRatio = (rWidht / width)
            vRatio = (rHeight / height)

            If vRatio > hRatio Then
                fWidth = Convert.ToInt32(CDbl(width) * hRatio)
                fHeight = Convert.ToInt32(CDbl(height) * hRatio)
            Else
                fWidth = Convert.ToInt32(CDbl(width) * vRatio)
                fHeight = Convert.ToInt32(CDbl(height) * vRatio)
            End If
        ElseIf height > rHeight Then
            hRatio = (rWidht / width)
            vRatio = (rHeight / height)

            If vRatio > hRatio Then
                fWidth = Convert.ToInt32(CDbl(width) * hRatio)
                fHeight = Convert.ToInt32(CDbl(height) * hRatio)
            Else
                fWidth = Convert.ToInt32(CDbl(width) * vRatio)
                fHeight = Convert.ToInt32(CDbl(height) * vRatio)
            End If
        ElseIf rHeight > height Then
            hRatio = (rWidht / width)
            vRatio = (rHeight / height)

            If vRatio > hRatio Then
                fWidth = Convert.ToInt32(CDbl(width) * hRatio)
                fHeight = Convert.ToInt32(CDbl(height) * hRatio)
            Else
                fWidth = Convert.ToInt32(CDbl(width) * vRatio)
                fHeight = Convert.ToInt32(CDbl(height) * vRatio)
            End If
        End If
        d_Width = fWidth
        d_Height = fHeight
    End Sub
End Class

