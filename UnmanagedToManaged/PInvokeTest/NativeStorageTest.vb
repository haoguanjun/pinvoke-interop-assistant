﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports PInvoke
Imports Xunit

'''<summary>
'''This is a test class for PInvoke.NativeStorage and is intended
'''to contain all PInvoke.NativeStorage Unit Tests
'''</summary>
Public Class NativeStorageTest

    <Fact>
    Public Sub LoadByName1()
        Dim s1 As New NativeStruct("s")
        s1.Members.Add(New NativeMember("m1", New NativeBuiltinType(BuiltinType.NativeInt32)))

        Dim s2 As NativeType = Nothing

        Dim ns As NativeStorage = NativeStorage.DefaultInstance
        ns.AddDefinedType(s1)

        Assert.True(ns.TryLoadByName(s1.Name, s2))
    End Sub

    <Fact>
    Public Sub LoadByName2()
        Dim s1 As New NativeStruct("s")
        s1.Members.Add(New NativeMember("m1", New NativeBuiltinType(BuiltinType.NativeInt32)))
        s1.Members.Add(New NativeMember("m2", New NativeBuiltinType(BuiltinType.NativeByte)))
        s1.Members.Add(New NativeMember("m3", New NativeBitVector(6)))
        s1.Members.Add(New NativeMember("m4", New NativePointer(New NativeBuiltinType(BuiltinType.NativeChar))))
        s1.Members.Add(New NativeMember("m5", New NativeArray(New NativeBuiltinType(BuiltinType.NativeFloat), 4)))
        s1.Members.Add(New NativeMember("m7", New NativeNamedType("bar", New NativeBuiltinType(BuiltinType.NativeDouble))))


        Dim s2 As NativeType = Nothing

        Dim ns As NativeStorage = NativeStorage.DefaultInstance
        ns.AddDefinedType(s1)

        Assert.True(ns.TryLoadByName(s1.Name, s2))
    End Sub

    ''' <summary>
    ''' Proc without parameters and a simple return type
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc1()
        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeByte)

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)

        Dim retp1 As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(p1.Name, retp1))
        Assert.Equal(p1.DisplayName, retp1.DisplayName)
    End Sub

    ''' <summary>
    ''' Proc with a non-trivial return type
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc2()
        Dim s1 As New NativeStruct("s1")
        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = s1

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)

        Dim retp1 As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(p1.Name, retp1))
        Assert.Equal(p1.DisplayName, retp1.DisplayName)
    End Sub

    ''' <summary>
    ''' Proc with a simple parameter
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc3()
        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeByte)
        p1.Signature.Parameters.Add(New NativeParameter("param1", New NativeBuiltinType(BuiltinType.NativeDouble)))

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)

        Dim retp1 As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(p1.Name, retp1))
        Assert.Equal(p1.DisplayName, retp1.DisplayName)
    End Sub

    ''' <summary>
    ''' Adding a procedure should not recursively add the type of it's parameters or return type
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc4()
        Dim s1 As New NativeStruct("s1")
        s1.Members.Add(New NativeMember("m1", New NativeBuiltinType(BuiltinType.NativeByte)))

        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = s1

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)

        Dim retp1 As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(p1.Name, retp1))
        Assert.Equal(p1.DisplayName, retp1.DisplayName)

        Dim rets1 As NativeDefinedType = Nothing
        Assert.False(ns.TryLoadDefined(s1.Name, rets1))
    End Sub

    ''' <summary>
    ''' Make sure that only a shollow save is done
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SaveAndLoad1()
        Dim s1 As New NativeStruct("s1")
        s1.Members.Add(New NativeMember("m1", New NativeBuiltinType(BuiltinType.NativeFloat)))

        Dim s2 As New NativeStruct("s2")
        s2.Members.Add(New NativeMember("m1", s1))

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddDefinedType(s2)

        Dim rets2 As NativeDefinedType = Nothing
        Assert.True(ns.TryLoadDefined(s2.Name, rets2))
        Assert.NotNull(rets2)
        Assert.False(NativeTypeEqualityComparer.AreEqualRecursive(s2, rets2))
        Assert.True(NativeTypeEqualityComparer.AreEqualTopLevel(s2, rets2))
    End Sub

    ''' <summary>
    ''' Save a type that has a reference to itself (via a pointer)
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SaveAndLoad2()
        Dim s1 As New NativeStruct("s1")
        s1.Members.Add(New NativeMember("m1", New NativePointer(s1)))

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddDefinedType(s1)

        Dim rets1 As NativeType = Nothing
        Assert.True(ns.TryLoadByName(s1.Name, rets1))
        Assert.NotNull(rets1)
        Assert.True(NativeTypeEqualityComparer.AreEqualTopLevel(s1, rets1))
    End Sub

    ''' <summary>
    ''' Save a struct that points to a named type.  This should succeed
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SaveAndLoad3()
        Dim s1 As New NativeStruct("s1")
        s1.Members.Add(New NativeMember("m1", New NativeNamedType("foo")))

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddDefinedType(s1)

        Dim rets1 As NativeType = Nothing
        Assert.True(ns.TryLoadByName(s1.Name, rets1))
        Assert.True(NativeTypeEqualityComparer.AreEqualTopLevel(s1, rets1))
    End Sub

    ''' <summary>
    ''' Load a typedef by name
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SaveAndLoad4()
        Dim t1 As New NativeTypeDef("t1")
        t1.RealType = New NativeBuiltinType(BuiltinType.NativeByte)

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddTypedef(t1)

        Dim rett1 As NativeType = Nothing
        Assert.True(ns.TryLoadByName(t1.Name, rett1))
        Assert.True(NativeTypeEqualityComparer.AreEqualRecursive(rett1, t1))
    End Sub

    ''' <summary>
    ''' Save and load a constant
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SaveAndLoad5()
        Dim c1 As New NativeConstant("c1", "v1")
        Dim c2 As New NativeConstant("c2", "v2", ConstantKind.MacroMethod)
        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddConstant(c1)
        ns.AddConstant(c2)

        Dim ret As NativeConstant = Nothing
        Assert.True(ns.TryLoadConstant("c1", ret))
        Assert.Equal("c1", ret.Name)
        Assert.Equal("v1", ret.Value.Expression)
        Assert.Equal(ConstantKind.Macro, ret.ConstantKind)

        Assert.True(ns.TryLoadConstant("c2", ret))
        Assert.Equal("c2", ret.Name)
        Assert.Equal("""v2""", ret.Value.Expression)
        Assert.Equal(ConstantKind.MacroMethod, ret.ConstantKind)
    End Sub

    ''' <summary>
    ''' Make sure calling conventions are properly saved
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SaveAndLoad6()
        Dim fptr As New NativeFunctionPointer("f1")
        Assert.Equal(NativeCallingConvention.WinApi, fptr.CallingConvention)
        fptr.CallingConvention = NativeCallingConvention.Pascal
        fptr.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeChar)

        Dim proc As New NativeProcedure("p1")
        Assert.Equal(NativeCallingConvention.WinApi, proc.CallingConvention)
        proc.CallingConvention = NativeCallingConvention.CDeclaration
        proc.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeChar)

        Dim ns As New NativeStorage()
        ns.AddProcedure(proc)
        ns.AddDefinedType(fptr)

        Dim temp As NativeDefinedType = Nothing
        Dim retPtr As NativeFunctionPointer = Nothing
        Assert.True(ns.TryLoadDefined(fptr.Name, temp))
        retPtr = DirectCast(temp, NativeFunctionPointer)
        Assert.Equal(NativeCallingConvention.Pascal, retPtr.CallingConvention)

        Dim retProc As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(proc.Name, retProc))
        Assert.Equal(NativeCallingConvention.CDeclaration, retProc.CallingConvention)


    End Sub

    <Fact>
    Public Sub BagSaveAndLoad1()
        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeBoolean)

        Dim td As NativeTypeDef =
                New NativeTypeDef(
                    "LPWSTR",
                    New NativePointer(BuiltinType.NativeWChar))
        p1.Signature.Parameters.Add(New NativeParameter(
            "param1",
            New NativeNamedType(
                "LPWSTR", td)))
        Assert.Equal("boolean p1(LPWSTR param1)", p1.DisplayName)
        Assert.Equal("p1(Sig(boolean)(Sal)(param1(LPWSTR(LPWSTR(*(wchar))))(Sal)))", SymbolPrinter.Convert(p1))

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)
        ns.AddTypedef(td)

        Dim bag As New NativeSymbolBag(ns)
        Dim ret1 As NativeProcedure = Nothing
        Assert.True(bag.TryFindOrLoadProcedure("p1", ret1))
        Assert.True(bag.TryResolveSymbolsAndValues())
        Assert.Equal(SymbolPrinter.Convert(p1), SymbolPrinter.Convert(ret1))
    End Sub

    <Fact>
    Public Sub Sal1()
        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeChar)
        p1.Signature.ReturnTypeSalAttribute = New NativeSalAttribute(SalEntryType.ReadOnly)

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)

        Dim retp1 As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(p1.Name, retp1))
        Assert.Equal("ReadOnly", retp1.Signature.ReturnTypeSalAttribute.DisplayName)
    End Sub

    <Fact>
    Public Sub Sal2()
        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeChar)
        p1.Signature.ReturnTypeSalAttribute = New NativeSalAttribute(
            New NativeSalEntry(SalEntryType.Deref, "foo"))

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)

        Dim retp1 As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(p1.Name, retp1))
        Assert.Equal("Deref(foo)", retp1.Signature.ReturnTypeSalAttribute.DisplayName)
    End Sub

    <Fact>
    Public Sub Sal3()
        Dim param As New NativeParameter("p")
        param.SalAttribute = New NativeSalAttribute(SalEntryType.Deref)
        param.NativeType = New NativeBuiltinType(BuiltinType.NativeChar)

        Dim p1 As New NativeProcedure("p1")
        p1.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeChar)
        p1.Signature.Parameters.Add(param)

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddProcedure(p1)

        Dim retp1 As NativeProcedure = Nothing
        Assert.True(ns.TryLoadProcedure(p1.Name, retp1))
        Assert.Equal("Deref", retp1.Signature.Parameters(0).SalAttribute.DisplayName)
    End Sub

    <Fact>
    Public Sub FuncPtr1()
        Dim fptr As New NativeFunctionPointer("f1")
        fptr.Signature.ReturnType = New NativeBuiltinType(BuiltinType.NativeChar)
        fptr.Signature.Parameters.Add(New NativeParameter("f", New NativeBuiltinType(BuiltinType.NativeFloat)))

        Dim ns As NativeStorage = NativeStorage.DefaultInstance()
        ns.AddDefinedType(fptr)

        Dim retFptr As NativeDefinedType = Nothing
        Assert.True(ns.TryLoadDefined(fptr.Name, retFptr))
        Assert.Equal("char (*f1)(float f)", DirectCast(retFptr, NativeFunctionPointer).DisplayName)
    End Sub

End Class