// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

// Various tests for the:
// Microsoft.FSharp.Collections.Array4D module

namespace FSharp.Core.UnitTests.FSharp_Core.Microsoft_FSharp_Collections

open System
open FSharp.Core.UnitTests.LibraryTestFx
open NUnit.Framework
open Utils

(*
[Test Strategy]
Make sure each method works on:
* Integer Array4D (value type)
* String  Array4D (reference type)
* Empty   Array4D (0 elements)
* Null    Array4D (null)
*)


[<TestFixture>][<Category "Collections.Array">][<Category "FSharp.Core.Collections">]
type Array4Module() =

    let VerifyDimensions arr x y z u =
        if Array4D.length1 arr <> x then Assert.Fail("Array4D does not have expected dimensions.")
        if Array4D.length2 arr <> y then Assert.Fail("Array4D does not have expected dimensions.")
        if Array4D.length3 arr <> z then Assert.Fail("Array4D does not have expected dimensions.")
        if Array4D.length4 arr <> u then Assert.Fail("Array4D does not have expected dimensions.")
        ()

    let array4d (arrs: 'a array array array array) = Array4D.init arrs.Length arrs.[0].Length arrs.[0].[0].Length  arrs.[0].[0].[0].Length  (fun i j k m -> arrs.[i].[j].[k].[m])
    
    let shouldBeEmpty arr = 
        if Array4D.length4 arr <> 0 
        && Array4D.length3 arr <> 0
        && Array4D.length2 arr <> 0
        && Array4D.length1 arr <> 0 then 
            Assert.Fail("Array3D is not empty.")    

    let m1 = array4d 
               [|
                 [| 
                        [| [| 1.0;2.0;3.0;4.0;5.0;6.0 |];
                           [| 11.0;21.0;31.0;41.0;51.0;61.0 |]  |]
                        [| [| 10.0;20.0;30.0;40.0;50.0;60.0 |];
                           [| 100.0;200.0;300.0;400.0;500.0;600.0 |]  |] |]
                 [| 
                        [| [| 19.0;29.0;39.0;49.0;59.0;69.0 |];
                           [| 119.0;219.0;319.0;419.0;519.0;619.0 |]  |]
                        [| [| 109.0;209.0;309.0;409.0;509.0;609.0 |];
                           [| 1009.0;2009.0;3009.0;4009.0;5009.0;6009.0 |]  |] |]
                |]

    [<Test>]
    member this.Create() =
        // integer array  
        let intArr = Array4D.create 3 4 5 6 168
        if intArr.[1,2,1,1] <> 168 then Assert.Fail()
        VerifyDimensions intArr 3 4 5 6
        
        // string array 
        let strArr = Array4D.create 2 3 4 5 "foo"
        if strArr.[1,2,3,2] <> "foo" then Assert.Fail()
        VerifyDimensions strArr 2 3 4 5
        
        // empty array     
        let eptArr1 = Array4D.create 0 0 0 0 'a'
        let eptArr2 = Array4D.create 0 0 0 0 'b'
        if eptArr1 <> eptArr2 then Assert.Fail()
        () 
        
    [<Test>]
    member this.Init() =
            
        // integer array  
        let intArr = Array4D.init 3 3 3 3 (fun i j k l -> i*1000 + j*100 + k*10 + l)
        if intArr.[1,1,1,1] <> 1111 then Assert.Fail()
        if intArr.[2,2,2,2] <> 2222 then Assert.Fail()
        VerifyDimensions intArr 3 3 3 3
        
        // ref array 
        let strArr = Array4D.init 3 3 3 3 (fun i j k l -> (i, j, k, l))
        if strArr.[2,0,1,2] <> (2, 0, 1, 2) then Assert.Fail()
        if strArr.[0,1,2,1] <> (0, 1, 2,1) then Assert.Fail()
        VerifyDimensions intArr 3 3 3 3
        ()

    [<Test>]
    member this.Get() =
        
        // integer array  
        let intArr = Array4D.init 2 3 2 2 (fun i j k l -> i*1000 + j*100 + k*10 + l)
        let resultInt = Array4D.get intArr  0 1 1 0
        if resultInt <> 110 then Assert.Fail()
        
        // string array 
        let strArr = Array4D.init 2 3 2 2 (fun i j k l -> i.ToString() + "-" + j.ToString() + "-" + k.ToString() + "-" + l.ToString())
        let resultStr = Array4D.get strArr 0 2 1 0
        if resultStr <> "0-2-1-0" then Assert.Fail()
        
        CheckThrowsIndexOutRangException(fun () -> Array4D.get strArr 2 0 0 0 |> ignore)
        CheckThrowsIndexOutRangException(fun () -> Array4D.get strArr 0 3 0 0 |> ignore)
        CheckThrowsIndexOutRangException(fun () -> Array4D.get strArr 0 0 2 0 |> ignore)
        CheckThrowsIndexOutRangException(fun () -> Array4D.get strArr 0 0 0 2 |> ignore)
        
        // empty array  
        let emptyArray = Array4D.init 0 0 0 0 (fun i j k l -> Assert.Fail())
        CheckThrowsIndexOutRangException (fun () -> Array4D.get emptyArray 1 0 0 0 |> ignore)
        CheckThrowsIndexOutRangException (fun () -> Array4D.get emptyArray 0 1 0 0 |> ignore)
        CheckThrowsIndexOutRangException (fun () -> Array4D.get emptyArray 0 0 1 0 |> ignore)
        CheckThrowsIndexOutRangException (fun () -> Array4D.get emptyArray 0 0 0 1 |> ignore)

        // null array
        let nullArr : string[,,,] = null
        CheckThrowsNullRefException (fun () -> Array4D.get nullArr 1 1 1 1 |> ignore)  
        ()
    
    [<Test>]
    member this.Length1() =
    
        // integer array  
        let intArr = Array4D.create 2 3 2 2 168
        let resultInt = Array4D.length1 intArr
        if  resultInt <> 2 then Assert.Fail()

        
        // string array 
        let strArr = Array4D.create 2 3 2 2 "enmity"
        let resultStr = Array4D.length1 strArr
        if resultStr <> 2 then Assert.Fail()
        
        // empty array     
        let eptArr = Array4D.create 0 0 0 0 1
        let resultEpt = Array4D.length1 eptArr
        if resultEpt <> 0  then Assert.Fail()

        // null array
        let nullArr = null : string[,,,]    
        CheckThrowsNullRefException (fun () -> Array4D.length1 nullArr |> ignore)  
        () 

    [<Test>]
    member this.Length2() =
    
        // integer array  
        let intArr = Array4D.create 2 3 2 2 168
        let resultInt = Array4D.length2 intArr
        if  resultInt <> 3 then Assert.Fail()

        
        // string array 
        let strArr = Array4D.create 2 3 2 2 "enmity"
        let resultStr = Array4D.length2 strArr
        if resultStr <> 3 then Assert.Fail()
        
        // empty array     
        let eptArr = Array4D.create 0 0 0 0 1
        let resultEpt = Array4D.length2 eptArr
        if resultEpt <> 0  then Assert.Fail()

        // null array
        let nullArr = null : string[,,,]    
        CheckThrowsNullRefException (fun () -> Array4D.length2 nullArr |> ignore)  
        () 

    [<Test>]
    member this.Length3() = 
    
        // integer array  
        let intArr = Array4D.create 2 3 5 0 168
        let resultInt = Array4D.length3 intArr
        if  resultInt <> 5 then Assert.Fail()
        
        // string array 
        let strArr = Array4D.create 2 3 5 0 "enmity"
        let resultStr = Array4D.length3 strArr
        if resultStr <> 5 then Assert.Fail()
        
        // empty array     
        let eptArr = Array4D.create 0 0 0 0 1
        let resultEpt = Array4D.length3 eptArr
        if resultEpt <> 0  then Assert.Fail()

        // null array
        let nullArr = null : string[,,,]    
        CheckThrowsNullRefException (fun () -> Array4D.length3 nullArr |> ignore)  
        ()  
        
    [<Test>]
    member this.Length4() = 
    
        // integer array  
        let intArr = Array4D.create 2 3 5 5 168
        let resultInt = Array4D.length4 intArr
        if  resultInt <> 5 then Assert.Fail()
        
        // string array 
        let strArr = Array4D.create 2 3 5 5 "enmity"
        let resultStr = Array4D.length4 strArr
        if resultStr <> 5 then Assert.Fail()
        
        // empty array     
        let eptArr = Array4D.create 0 0 0 0 1
        let resultEpt = Array4D.length4 eptArr
        if resultEpt <> 0  then Assert.Fail()

        // null array
        let nullArr = null : string[,,,]    
        CheckThrowsNullRefException (fun () -> Array4D.length4 nullArr |> ignore)  
        ()          

    [<Test>]
    member this.Set() =

        // integer array  
        let intArr = Array4D.init 2 3 2 2 (fun i j k l -> i*1000 + j*100 + k*10 + l)
        
        Assert.IsFalse(intArr.[1,1,1,1] = -1)
        Array4D.set intArr 1 1 1 1 -1
        Assert.IsTrue(intArr.[1,1,1,1] = -1)

        // string array 
        let strArr = Array4D.init 2 3 2 2 (fun i j k l -> i.ToString() + "-" + j.ToString()+ "-" + k.ToString() + "-" + l.ToString())
        
        Assert.IsFalse(strArr.[1,1,1,1] = "custom")
        Array4D.set strArr 1 1 1 1 "custom"
        Assert.IsTrue(strArr.[1,1,1,1] = "custom")

        // Out of bounds checks
        CheckThrowsIndexOutRangException(fun () -> Array4D.set strArr 2 0 0 0 "out of bounds")
        CheckThrowsIndexOutRangException(fun () -> Array4D.set strArr 0 3 0 0 "out of bounds")
        CheckThrowsIndexOutRangException(fun () -> Array4D.set strArr 0 0 2 0 "out of bounds")
        CheckThrowsIndexOutRangException(fun () -> Array4D.set strArr 0 0 0 2 "out of bounds")
        
        // empty array  
        let emptArr = Array4D.create 0 0 0 0 'z'
        CheckThrowsIndexOutRangException(fun () -> Array4D.set emptArr 0 0 0 0 'a')

        // null array
        let nullArr = null : string[,,,]    
        CheckThrowsNullRefException (fun () -> Array4D.set  nullArr 0 0 0 0 "")  
        ()  

    [<Test>]
    member this.ZeroCreate() =
            
        let intArr : int[,,,] = Array4D.zeroCreate 2 3 2 2
        if Array4D.get intArr 1 1 1 1 <> 0 then 
            Assert.Fail()
            
        let structArray : DateTime[,,,] = Array4D.zeroCreate 1 1 1 1
        let defaultVal = new DateTime()
        Assert.IsTrue(Array4D.get structArray 0 0 0 0 = defaultVal)

        let strArr : string[,,,] = Array4D.zeroCreate 2 3 2 2
        for i in 0 .. 1 do
            for j in 0 .. 2 do
                for k in 0 .. 1 do
                    for l in 0 .. 1 do
                        Assert.AreEqual(null, strArr.[i, j, k, l])
                    
        // Test invalid values
        CheckThrowsArgumentException(fun () -> Array4D.zeroCreate -1 1 1 1 |> ignore)
        CheckThrowsArgumentException(fun () -> Array4D.zeroCreate 1 -1 1 1 |> ignore)
        CheckThrowsArgumentException(fun () -> Array4D.zeroCreate 1 1 -1 1 |> ignore)
        CheckThrowsArgumentException(fun () -> Array4D.zeroCreate 1 1 1 -1 |> ignore)
        
        ()

    [<Test>]
    member this.SlicingBoundedStartEnd() = 
        shouldEqual m1.[*,*,*,*]  m1
        shouldEqual m1.[0..,*,*,*]   m1
        shouldEqual m1.[0..0,*,*,*]  
           (array4d 
              [|
                [| 
                       [| [| 1.0;2.0;3.0;4.0;5.0;6.0 |];
                          [| 11.0;21.0;31.0;41.0;51.0;61.0 |]  |]
                       [| [| 10.0;20.0;30.0;40.0;50.0;60.0 |];
                          [| 100.0;200.0;300.0;400.0;500.0;600.0 |]  |] |]
               |])
        shouldEqual m1.[1..1,*,*,*]  
           (array4d 
              [|
                 [| 
                       [| [| 19.0;29.0;39.0;49.0;59.0;69.0 |];
                          [| 119.0;219.0;319.0;419.0;519.0;619.0 |]  |]
                       [| [| 109.0;209.0;309.0;409.0;509.0;609.0 |];
                          [| 1009.0;2009.0;3009.0;4009.0;5009.0;6009.0 |]  |] |]
               |])
    
        shouldEqual m1.[*,0..0,*,*]  
           (array4d 
              [|
                [| 
                       [| [| 1.0;2.0;3.0;4.0;5.0;6.0 |];
                          [| 11.0;21.0;31.0;41.0;51.0;61.0 |]  |]
                |];
                [| 
                       [| [| 19.0;29.0;39.0;49.0;59.0;69.0 |];
                          [| 119.0;219.0;319.0;419.0;519.0;619.0 |]  |]
                |]
               |])
        shouldEqual m1.[..1,*,*,*]   m1
        shouldEqual m1.[*,1..,*,*]  
           (array4d 
              [|
                [| 
                       [| [| 10.0;20.0;30.0;40.0;50.0;60.0 |];
                          [| 100.0;200.0;300.0;400.0;500.0;600.0 |]  |]
                |];
                [| 
                       [| [| 109.0;209.0;309.0;409.0;509.0;609.0 |];
                          [| 1009.0;2009.0;3009.0;4009.0;5009.0;6009.0 |]  |] 
                |]
               |])
        shouldEqual m1.[*,0..1,*,*]   m1
        shouldEqual m1.[*,*,0..0,*]  
           (array4d 
              [|
                [| 
                       [| [| 1.0;2.0;3.0;4.0;5.0;6.0 |];  |]
                       [| [| 10.0;20.0;30.0;40.0;50.0;60.0 |];  |]
                |];
                [| 
                       [| [| 19.0;29.0;39.0;49.0;59.0;69.0 |];  |]
                       [| [| 109.0;209.0;309.0;409.0;509.0;609.0 |];  |] |]
               |])
        shouldEqual m1.[*,*,*,0..5]  m1
    
        shouldEqual m1.[*,*,*,0..4]  
           (array4d 
              [|
                [| 
                       [| [| 1.0;2.0;3.0;4.0;5.0 |];
                          [| 11.0;21.0;31.0;41.0;51.0 |]  |]
                       [| [| 10.0;20.0;30.0;40.0;50.0 |];
                          [| 100.0;200.0;300.0;400.0;500.0 |]  |]
                |];
                [| 
                       [| [| 19.0;29.0;39.0;49.0;59.0 |];
                          [| 119.0;219.0;319.0;419.0;519.0 |]  |]
                       [| [| 109.0;209.0;309.0;409.0;509.0 |];
                          [| 1009.0;2009.0;3009.0;4009.0;5009.0 |]  |] 
                |]
               |])


    [<Test>]
    member this.SlicingOutOfBounds() = 
        shouldBeEmpty m1.[*,*,*,7..]  
        shouldBeEmpty m1.[*,*,*,.. -1]  
    
        shouldBeEmpty m1.[*,*,3..,*]  
        shouldBeEmpty m1.[*,*,.. -1,*]  
    
        shouldBeEmpty m1.[*,3..,*,*]  
        shouldBeEmpty m1.[*,.. -1,*,*]  
    
        shouldBeEmpty m1.[3..,*,*,*]  
        shouldBeEmpty m1.[.. -1,*,*,*]  
