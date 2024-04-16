# FluxIO 사용자 가이드 - 기본 사용법
## 공통
문서 내에서 자주 언급될 클래스들을 여기에 모아두었습니다.  
  
```csharp
// FILE: 1Common.cs
// 공통 열거형 정의
public enum Department
{
    Engineering,
    Marketing,
    HumanResources
}

// 기본 클래스: 직원 정보
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

// 중간 클래스: 관리자 정보
public class Manager : Employee
{
    protected int teamSize;

    public Manager(string name, int age, Department department, int teamSize)
        : base(name, age, department)
    {
        this.teamSize = teamSize;
    }
}

// 최하위 클래스: 고급 관리자 정보
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

## `ProtFlags`와 `TypeFlags` 사용하기
이 값들은 열거형(enum) 타입이므로 사전에 정의된 값을 사용할 수 있습니다.

두 타입 모두 플래그를 허용하는 타입이므로, 여러 값들을 동시에 가질 수 있습니다.
만약, `public` 또는 `private` 필드만을 검색하려면 `ProtFlags.Public | ProtFlags.Private` 값을 사용할 수 있습니다.
`int` 또는 `double` 필드만을 검색하려면 `TypeFlags.I4 | TypeFlags.R8`을 사용할 수 있습니다.

이러한 열거형 값은 매우 느린 속도로 수행되는 필드 이름 문자열 비교 전에 평가되어 원하는 필드를 더 빠르게 찾을 수 있도록 돕습니다.  
  
만약 찾고자하는 필드의 형식(Type)이 열거형 타입의 경우, 해당 열거형 타입의 상수 값 형식을 사용해야 합니다.
예를 들어, `enum FooEnum : ushort`의 경우 `TypeFlags.U2`를 사용합니다.
    
그 외, **8바이트**를 초과하는 크기의 구조체나 **원시 타입(\*)**(primitive types)이 아닌 경우에는 `TypeFlags.VALUETYPE`로 표시됩니다.  
  
(\*) 원시 타입의 예: `int`, `uint`, `short`, `float` 등
### Example
```csharp
FluxRuntimeFieldDesc fd_age;

fd_age = FluxTool.GetInstanceField(
            typeof(SeniorManager), 
            "age"u8,
            ProtFlags.Protected,
            TypeFlags.I4);

// 2-times string comparsion:
//  0: VS. SeniorManager.region         - ProtFlags 다름: Protected != Private
//  1: VS. Manager.teamSize             - ProtFlags 와 TypeFlags 매치됨, 이름 비교 수행 (-> 이름 불일치)
//  1: VS. Employee.name                - TypeFlags 다름: I4 != CLASS
//  2: VS. Employee.age                 - ProtFlags 와 TypeFlags 매치됨, 이름 비교 수행. 찾았음!
```

## `FluxSearchSpace` 사용하기
`FluxSearchSpace` 구조체는 검색을 시작할 지점(`start`)과 검색할 필드의 수(`count`)를 지정하는 데 사용됩니다.  
  
`start`에는 인덱스 표현식을 사용할 수 있습니다.  
예를 들어 `^1` 같은 표현을 사용해 검색 시작 위치를 설정할 수 있습니다.  
  
`isConstrained` 옵션은 최상위 부모부터 지정된 타입에 이르기까지, 모든 중간 클래스를 포함한 상속 계층 내의 필드 총 수를 모를 때 유용합니다.  
  
`isConstrained`를 `true`로 설정하면 인덱싱이 현재 클래스에만 상대적으로 조정되어 상속된 필드는 무시됩니다. 
예를 들어:  
  
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
  
이 설정은 인덱스가 현재 타입 내에서 직접 선언된 필드만을 참조하도록 보장하며, `start`에 지정한 `0`은 상속된 필드를 무시하고 현재 타입의 첫 번째 필드를 가르킵니다.  
  
또한, `FluxSearchSpace` 구조체에서 `count`를 `0`으로 설정하면, 지정된 시작 인덱스부터 마지막 필드까지 검색이 이루어집니다.
  
**주의**: `start` 매개변수에 `^1`과 같은 `FromEnd` 표현식을 사용하면, `isConstrained` 옵션은 무시됩니다.
  
## `SStringUtf8` 사용하기
`SStringUtf8` 구조체는 `name` 매개변수의 타입으로 지정되어 있습니다.   
이전 예제에서 보여진 것처럼, `"text"u8`과 같은 UTF8 상수 문자열에서 암시적으로 변환될 수 있습니다.  
  
또한, UTF8 인코딩된 `ReadOnlySpan<byte>`에서 암시적으로 변환될 수 있습니다.  
이 구조체는 `string`이나 UTF16 인코딩된 `ReadOnlySpan<char>`에서 변환될 수 있지만, 이는 UTF16에서 UTF8로 재인코딩하는 과정을 포함하므로 작업 속도가 느려질 수 있습니다.  
    
이 구조체는 문자열의 비교 방법을 함께 지정할 수 있도록 설계돼있습니다. 문자열 비교 방법을 설정하기 위해 `StringMatchType` 열거형 값 중 하나를 사용할 수 있습니다.  
- 기본적으로, `StringMatchType.Default`가 사용되어 두 문자열에 대하여 같음 비교(\*)를 수행합니다. 
   
(\*) 일반적으로 `string` 타입끼리 `==` 비교를 하는 것과 동등한 연산을 수행합니다.
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
   
추가적으로, `SStringUtf8.AnyAccept` 값은 검색 중에 문자열 비교를 완전히 건너뛸 수 있게 합니다.   
이는 필드의 타입, 접근 제한자 수준, 검색 범위만으로도 원하는 필드를 식별하는 것이 충분히 가능한 시나리오에서 특히 유용할 수 있습니다.  
   
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
  
  
## 다른 문서
이러한 문서들도 살펴보십시오.  
- [고급 사용법 가이드](./AdvancedUsage.md)  
- [API 문서](./API/fluxiolib.md)  