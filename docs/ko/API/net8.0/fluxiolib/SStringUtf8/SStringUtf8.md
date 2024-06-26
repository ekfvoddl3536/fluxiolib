# SStringUtf8 constructor (1 of 3)

UTF8 인코딩 문자열과 [`StringMatchType`](../StringMatchType.md)으로부터 [`SStringUtf8`](../SStringUtf8.md)을 초기화합니다.

```csharp
[CLSCompliant(false)]
public SStringUtf8(ReadOnlySpan<byte> utf8String, 
    delegate*<ReadOnlySpan<byte>&, ReadOnlySpan<byte>&, bool> fpMatch)
```

| parameter | description |
| --- | --- |
| utf8String | UTF8 인코딩된 문자열 |
| fpMatch | 두 문자열의 비교에 사용할 함수 포인터 콜백입니다. |

## Examples

```csharp
private static void TestMethod()
{
    // Same as: left, input, other
    SStringUtf8 other = "hello, world!"u8;
     
    // Same as: right, compare, me
    SStringUtf8 me = new SStringUtf8("world!"u8, &MyCallback);
     
    bool r1 = me.Match(other); // TRUE
     
    bool r2 = other.Match(me); // FALSE
}

private static bool MyCallback(in ReadOnlySpan<byte> other, in ReadOnlySpan<byte> me)
{
    return other.EndsWith(me);
}
```

## See Also

* struct [SStringUtf8](../SStringUtf8.md)
* namespace [fluxiolib](../../fluxiolib.md)

---

# SStringUtf8 constructor (2 of 3)

UTF8 인코딩 문자열과 [`StringMatchType`](../StringMatchType.md)으로부터 [`SStringUtf8`](../SStringUtf8.md)을 초기화합니다.

```csharp
public SStringUtf8(ReadOnlySpan<byte> utf8String, StringMatchType type = StringMatchType.Default)
```

| parameter | description |
| --- | --- |
| utf8String | UTF8 인코딩된 문자열 |
| type | 다른 문자열과 *utf8String*을 비교할 때, 사용할 동작의 유형을 지정합니다. |

## Examples

```csharp
// Same as: left, input, other
SStringUtf8 other = "hello, world!"u8;
 
// Same as: right, compare, me
SStringUtf8 me = new SStringUtf8("hello"u8, StringMatchType.StartsWith);
 
bool r1 = me.Match(other); // TRUE
 
bool r2 = other.Match(me); // FALSE
```

## See Also

* enum [StringMatchType](../StringMatchType.md)
* struct [SStringUtf8](../SStringUtf8.md)
* namespace [fluxiolib](../../fluxiolib.md)

---

# SStringUtf8 constructor (3 of 3)

UTF16 인코딩 문자열과 [`StringMatchType`](../StringMatchType.md)으로부터 [`SStringUtf8`](../SStringUtf8.md)을 초기화합니다.

```csharp
public SStringUtf8(ReadOnlySpan<char> utf16String, StringMatchType type = StringMatchType.Default)
```

| parameter | description |
| --- | --- |
| utf16String | UTF8 인코딩된 문자열 |
| type | 다른 문자열과 *utf16String*을 비교할 때, 사용할 동작의 유형을 지정합니다. |

## Examples

```csharp
// Same as: left, input, other
SStringUtf8 other = "hello, world!";
 
// Same as: right, compare, me
SStringUtf8 me = new SStringUtf8("hello", StringMatchType.StartsWith);
 
bool r1 = me.Match(other); // TRUE
 
bool r2 = other.Match(me); // FALSE
```

## See Also

* enum [StringMatchType](../StringMatchType.md)
* struct [SStringUtf8](../SStringUtf8.md)
* namespace [fluxiolib](../../fluxiolib.md)

<!-- DO NOT EDIT: generated by xmldocmd for fluxiolib.dll -->
