# FluxIO 사용자 가이드 - 고급 사용법
## 목차
- [FluxIO 사용자 가이드 - 고급 사용법](#fluxio-사용자-가이드---고급-사용법)
  - [목차](#목차)
  - [`TypedFieldAccessor` 사용하기](#typedfieldaccessor-사용하기)
  - [필드 필터링과 반복자](#필드-필터링과-반복자)
  - [제네릭 형식에서 탐색](#제네릭-형식에서-탐색)
  - [다른 문서](#다른-문서)

## `TypedFieldAccessor` 사용하기
`UnsafeFieldAccessor`는 유효성 검사를 수행하지 않기 때문에, 매우 빠르지만 그 만큼 위험합니다.  
만약, 접근하려는 형식(`Type`)에 대한 기본적인 검사가 포함된 `FieldAccessor`를 사용하고자 한다면, `TypedFieldAccessor` 사용을 고려해볼 수 있습니다.  
  
`TypedFieldAccessor`는 필드의 형식을 추적하고 확인하지는 않지만, 필드가 선언된 형식에 대한 검사는 수행합니다.  
이는 잘못된 형식으로 필드 정보를 읽을 수 있는 문제를 방지합니다.  
  
예를 들어서, 다음과 같이 서로 다른 형식으로 데이터를 읽으려고 시도한다고 할 때:
```csharp
// FILE: Foo.cs
class FooA 
{
    public int x;
}

class FooB
{
    public int y;
}
```
```csharp
// FILE: Program.cs
using fluxiolib;

FluxRuntimeFieldDesc fd_x;

fd_x = FluxTool.GetInstanceField(typeof(FooA), SStringUtf8.AnyAccept);

FooB fooB = new FooB();

fd_x.FieldAccessor.Value<int>(fooB) = 500;

Console.WriteLine("fooB.x = " + fooB.y);

// Print:
//  fooB.x = 500
```
  
이와 같이 필드를 검색할 때 사용한 형식과 사용할 때 값으로 입력한 형식이 서로 연관성이 없는 완전히 다른 형식이라도 필드의 오프셋이 입력한 형식에서도 유효하다면, 문제 없이 사용할 수 있습니다.  
  
때문에, 다음과 같이 더 안전한 `FieldAccessor`의 사용을 고려해볼 수 있습니다:  
```csharp
// FILE: Program.cs
using fluxiolib;

FluxRuntimeFieldDesc fd_x;

fd_x = FluxTool.GetInstanceField(typeof(FooA), SStringUtf8.AnyAccept);

FooB fooB = new FooB();

fd_x.GetSafeFieldAccessor().Value<int>(fooB) = 500; // 'System.ArgumentException' exception occurs

Console.WriteLine("fooB.x = " + fooB.y);

// Print:
//  Unhandled exception. System.ArgumentException: TypeMismatch/InvalidCast: current: 'FooA', other: 'FooB'
```
  
`FieldAccessor` 프로퍼티와 달리 `GetSafeFieldAccessor()` 메소드는 유효성 검사를 수행하고 필드가 선언된 형식을 얻기 위한 연산 비용이 존재하므로, 가능하다면 자주 호출하지 않는 것이 좋습니다.  
  
## 필드 필터링과 반복자
많은 양의 필드를 필터링하여 얻고자 할 때, `FluxTool.GetInstanceField(...)`를 많이 호출하는 것은 권장되지 않습니다.  
대신에, `FluxTool.GetInstanceFieldList(...)`을 사용하는 것이 좋습니다.  

```csharp
// FILE: ManyFields.cs
class ManyFields
{
    public int x, y, z, w;
    
    protected double dx, dy, dz, dw;
    
    private protected object? objName, objRegion, objComponent, objValue;
    
    protected short  f1, f2, f3, f4, f5, f6, f7, f8;
        
    internal ulong L1, L2, L3, L4, L5, L6, L7, L8;
    
    protected internal decimal value001, value002, value003, value004, value005, value006;
}
```  
```csharp
// FILE: Program.cs
using fluxiolib;

IEnumerable<FluxRuntimeFieldDesc> fieldList;

fieldList = FluxTool.GetInstanceFieldList(
    typeof(ManyFields),
    ProtFlags.Public | ProtFlags.FamilyANDAssembly,
    TypeFlags.ALL);

int num = 1;
foreach (FluxRuntimeFieldDesc field in fieldList)
{
    Console.WriteLine("# " + num);
    Console.WriteLine("  Name       : " + field.GetNameWithoutCaching());
    Console.WriteLine("  FieldOffset: " + field.FieldOffset);

    ++num;
}

// Print:
//  # 1
//    Name       : x
//    FieldOffset: 128
//  # 2
//    Name       : y
//    FieldOffset: 132
//  # 3
//    Name       : z
//    FieldOffset: 136
//  # 4
//    Name       : w
//    FieldOffset: 140
//  # 5
//    Name       : objName
//    FieldOffset: 0
//  # 6
//    Name       : objRegion
//    FieldOffset: 8
//  # 7
//    Name       : objComponent
//    FieldOffset: 16
//  # 8
//    Name       : objValue
//    FieldOffset: 24
```

## 제네릭 형식에서 탐색
기본적으로 제네릭 인수가 제공되지 않은 제네릭 선언 형태의 형식(예시: `List<>`)은 사용할 수 없습니다.  
그러나, `_Unsafe` 접미사가 있는 메소드를 사용한다면 매개변수 유효성을 건너뛰므로 강제로 작동 테스트를 수행할 수 있습니다.  
  
```csharp
// FILE: Generics.cs
class FooGeneric<T>
{
    public int x;
    public T? value1;
    public T? value2;
    public object? obj;
    public long y;
    public decimal z;
}
```
```csharp
var fd_g1 = FluxTool.GetInstanceField_Unsafe(typeof(FooGeneric<>), "value2"u8);

var fd_m2 = FluxTool.GetInstanceField<FooGeneric<int>>("value2"u8);

var fd_c3 = FluxTool.GetInstanceField<FooGeneric<string>>("value2"u8);

Console.WriteLine("FooGeneric<>.value2       = " + fd_g1.FieldOffset);
Console.WriteLine("                            " + fd_g1.FieldCorType);

Console.WriteLine("FooGeneric<int>.value2    = " + fd_m2.FieldOffset);
Console.WriteLine("                            " + fd_m2.FieldCorType);

Console.WriteLine("FooGeneric<string>.value2 = " + fd_c3.FieldOffset);
Console.WriteLine("                            " + fd_c3.FieldCorType);

// Print:
//  FooGeneric<>.value2       = 8
//                              CLASS
//  FooGeneric<int>.value2    = 24
//                              I4
//  FooGeneric<string>.value2 = 8
//                              CLASS
```

결과와 같이 제네릭 선언 형태의 형식(예시: `List<>`)에서 찾은 필드 오프셋을 그대로 사용하기에는 어려운 부분이 있습니다.  
그러나 만약, 제네릭 제약 조건으로 `where T : class`를 사용한다면 `FooGeneric<>`에서 찾은 필드 오프셋을 그대로 사용해도 됩니다. 
   
`T`에 전달되는 모든 형식이 참조 형식임을 보장할 수만 있다면, `FooGeneric<>`에서 미리 찾은 제네릭 형식으로 선언된 필드를 그대로 사용해도 괜찮습니다.  



## 다른 문서
- [API 문서](./API/fluxiolib.md)
- [빌드 결과물 얻기](./GetBuildArtifacts.md)  
- [벤치마크 결과](../Benchmark.Result.md)  
- [시스템 호환성](../Compatibility.md)  
