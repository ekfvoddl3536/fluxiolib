# FluxIO Usage Guide
## Table of Contents
- [FluxIO Usage Guide](#fluxio-usage-guide)
  - [Table of Contents](#table-of-contents)
  - [Common](#common)
  - [Using `ProtFlags` and `TypeFlags`](#using-protflags-and-typeflags)
    - [Example](#example)
  - [Using `FluxSearchSpace`](#using-fluxsearchspace)
  - [Using `SStringUtf8`](#using-sstringutf8)
  - [With `Reflection` (*recommend*)](#with-reflection-recommend)
  - [See Also](#see-also)

## Common
To prevent the page from becoming long, declarations commonly used in example code are placed here.  
  
```csharp
// FILE: 1Common.cs
// Common enumeration for departments
public enum Department
{
    Engineering,
    Marketing,
    HumanResources
}

// Base class: Information about employees
public class Employee
{
    private string name;
    protected int age;
    internal Department department;

    public Employee(string name, int age, Department department)
    {
        this.name = name;
        this.age = age;
        this.department = department;
    }
}

// Intermediate class: Information about managers
public class Manager : Employee
{
    protected int teamSize;

    public Manager(string name, int age, Department department, int teamSize)
        : base(name, age, department)
    {
        this.teamSize = teamSize;
    }
}

// Lowest class: Information about senior managers
public class SeniorManager : Manager
{
    private string region;
    public new string department;

    public SeniorManager(string name, int age, Department department, int teamSize, string region)
        : base(name, age, department, teamSize)
    {
        this.region = region;
        this.department = department.ToString();
    }
}
```

## Using `ProtFlags` and `TypeFlags`
These values are in enum type, allowing for the use of predefined values.  
  
Both are flag types, capable of holding multiple values simultaneously.  
To search exclusively for `public` or `private` fields, specify the value as `ProtFlags.Public | ProtFlags.Private`.  
To specifically search for `int` or `double` fields, use `TypeFlags.I4 | TypeFlags.R8`.  
  
Using these flags is evaluated before the very slow field name string comparison, making it quicker to locate the desired field.  
  
For enum types, adopt the format of the enum type's constant values.  
For example, for `enum FooEnum : ushort`, you would use `TypeFlags.U2`.  
  
Structures exceeding **8 bytes** in size or that are **not primitive types** (e.g., int, uint, short, etc.) are denoted as `TypeFlags.VALUETYPE`.  
  
### Example
```csharp
FluxRuntimeFieldDesc fd_age;

fd_age = FluxTool.GetInstanceField(
            typeof(SeniorManager), 
            "age"u8,
            ProtFlags.Protected,
            TypeFlags.I4);

// 2-times string comparsion:
//  0: VS. SeniorManager.region         - ProtFlags not match: Protected != Private
//  1: VS. Manager.teamSize             - ProtFlags and TypeFlags match, compare FieldName (-> Name not match)
//  1: VS. Employee.name                - TypeFlags not match: I4 != CLASS
//  2: VS. Employee.age                 - ProtFlags and TypeFlags match, compare FieldName. FOUND!
```

## Using `FluxSearchSpace`
The `FluxSearchSpace` struct is used to specify the starting point (`start`) and the number of fields (`count`) to search through.  
You can use an Index expression, such as `^1`, to set where the search begins.

The `isConstrained` option is useful when you do not know the total number of fields within an inheritance hierarchy—ranging from the highest parent to the specified type, including all intermediate classes.  

Setting `isConstrained` to `true` adjusts the indexing to be relative only to the current class, ignoring inherited fields. For example:
  
```csharp
class Foo1 { 
    public int x;
}

class Foo2_XY : Foo1 {
    public int y;
}

class Foo3_XYZ : Foo2_XY {
    public int z;
    public new int x;
}
```
```csharp
FluxRuntimeFieldDesc fd_m1;
FluxRuntimeFieldDesc fd_m2;

FluxSearchSpace space;

space = new FluxSearchSpace(start: 0, count: 2); // default 'isConstrained' is FALSE.

fd_m1 = FluxTool.GetInstanceField(
            typeof(Foo3_XYZ),
            "x"u8,
            ProtFlags.Public,
            TypeFlags.ALL,
            space);
            // RESULT: Foo1.x

space = new FluxSearchSpace(start: 0, count: 2, isConstrained: true);

fd_m2 = FluxTool.GetInstanceField(
            typeof(Foo3_XYZ),
            "x"u8,
            ProtFlags.Public,
            TypeFlags.ALL,
            space);
            // RESULT: Foo3_XYZ.x

Console.Write("isConstrained = false: FieldOffset: ");
Console.WriteLine(fd_m1.FieldOffset);

Console.Write("isConstrained = true: FieldOffset: ");
Console.WriteLine(fd_m2.FieldOffset);

Foo3_XYZ foo = new Foo3_XYZ();

fd_m1.FieldAccessor.Value<int>(foo) = -50;

fd_m2.FieldAccessor.Value<int>(foo) = 100;

Foo1 upcast = foo;

Console.Write("Foo1.x     = ");
Console.WriteLine(upcast.x);

Console.Write("Foo3_XYZ.x = ");
Console.WriteLine(foo.x);

// Print:
//  isConstrained = false: FieldOffset: 0
//  isConstrained = true: FieldOffset: 12
//  Foo1.x     = -50
//  Foo3_XYZ.x = 100
```
  
This setting ensures that the index refers to fields declared directly within the current type, with 0 indicating the first field of the current type only, disregarding any inherited fields.

Additionally, setting the `count` in the `FluxSearchSpace` struct to `0` enables the search to span from the specified starting index to the last field.

**Caution**: Using a `FromEnd` expression like `^1` for the `start` parameter generally bypasses the `isConstrained` option.
  
## Using `SStringUtf8`
The `SStringUtf8` struct is designated as the type for the `name` parameter. As demonstrated in previous examples, it can be implicitly converted from UTF8 constant strings such as `"text"u8`.

Furthermore, it supports implicit conversion from a UTF8-encoded `ReadOnlySpan<byte>`. It is also capable of being converted from a `string` or a UTF16-encoded `ReadOnlySpan<char>`, though this involves re-encoding from UTF16 to UTF8, which may slow down the operation.

This struct is also engineered to facilitate string comparison methods. It utilizes one of the `StringMatchType` enum values to determine how strings are compared:
- **By default**, `StringMatchType.Default` is employed, ensuring that two strings are compared for sequence equality.
  
```csharp
FluxRuntimeFieldDesc fd_k2;

SStringUtf8 name = new SStringUtf8("te"u8, StringMatchType.StartsWith);

fd_k2 = FluxTool.GetInstanceField(
            typeof(SeniorManager),
            name,
            ProtFlags.Family,
            TypeFlags.I4);
            // RESULT: Manager.teamSize

Console.Write("FieldOffset: ");
Console.WriteLine(fd_k2.FieldOffset);

Console.Write("FieldName: ");
Console.WriteLine(fd_k2.GetNameWithoutCaching());

// Print:
//  FieldOffset: 20
//  FieldName: teamSize
```
  
Additionally, the `SStringUtf8.AnyAccept` feature allows for bypassing string comparison entirely during searches. This can be especially useful in scenarios where it is adequate to identify the desired field based solely on the field’s type, access modifier level, and search scope.
     
```csharp
FluxRuntimeFieldDesc fd_k1;

fd_k1 = FluxTool.GetInstanceField(
            typeof(SeniorManager),
            SStringUtf8.AnyAccept, // Accepts any input string, without performing any string comparison
            ProtFlags.Assembly,
            TypeFlags.ALL);
            // RESULT: Employee.department

Console.Write("FieldOffset: ");
Console.WriteLine(fd_k1.FieldOffset);

Console.Write("FieldName: ");
Console.WriteLine(fd_k1.GetNameWithoutCaching());

// Print:
//  FieldOffset: 12
//  FieldName: department
```


## With `Reflection` (*recommend*)
Single field lookup performance is quite good with Reflection. (See the [benchmark results](../Benchmark.Result.md#table).)   
  
Therefore, unless there is a specific reason not to, it is advisable to use `Reflection` for field lookups and `FluxIO` for field access.  
  
```csharp
FieldInfo fdInfo;

// Search the field.
fdInfo = typeof(SeniorManager).GetField("teamSize", BindingFlags.NonPublic | BindingFlags.Instance)!;

FluxRuntimeFieldDesc fdDesc;

// To access the fields, convert to FluxRuntimeFieldDesc.
fdDesc = FluxTool.ToRuntimeFieldDesc(fdInfo);

SeniorManager senior = new SeniorManager();

// Access fields, read or modify data.
fdDesc.FieldAccessor.Value<int>(senior) = 250;
```



## See Also
- [Advanced Usage Guide (Korean)](./../ko/AdvancedUsage.md)  
- [API Document (Korean)](./../ko/API/fluxiolib.md)  
- [Obtaining Build Artifacts](./GetBuildArtifacts.md)
- [Benchmark Results](../Benchmark.Result.md)
- [System Compatibility](../Compatibility.md)
  
*Some documents are linked to the **Korean** versions as the **English** documents are not yet prepared.*  
