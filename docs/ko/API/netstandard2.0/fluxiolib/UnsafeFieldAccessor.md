# UnsafeFieldAccessor structure

타입 제약을 포함하지 않는 고속 필드 접근자(FieldAccessor) 객체입니다.

```csharp
public struct UnsafeFieldAccessor : IEquatable<UnsafeFieldAccessor>
```

## Public Members

| name | description |
| --- | --- |
| [Offset](UnsafeFieldAccessor/Offset.md) { get; } | 이 필드 접근자가 가진 필드 오프셋입니다. |
| override [Equals](UnsafeFieldAccessor/Equals.md)(…) |  |
| [Equals](UnsafeFieldAccessor/Equals.md)(…) |  |
| override [GetHashCode](UnsafeFieldAccessor/GetHashCode.md)() |  |
| [Value&lt;TField&gt;](UnsafeFieldAccessor/Value.md)(…) | *reference* 참조에서 필드 데이터를 읽습니다. (2 methods) |
| [ValueDirect&lt;TStruct,TField&gt;](UnsafeFieldAccessor/ValueDirect.md)(…) | 구조체 형식의 참조에서 필드 데이터를 읽습니다. |
| [operator ==](UnsafeFieldAccessor/op_Equality.md) |  |
| [operator !=](UnsafeFieldAccessor/op_Inequality.md) |  |

## Remarks

[이 API는 매개변수나 개체 상태의 유효성을 검사하지 않습니다.](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe)

## See Also

* namespace [fluxiolib](../fluxiolib.md)

<!-- DO NOT EDIT: generated by xmldocmd for fluxiolib.dll -->
