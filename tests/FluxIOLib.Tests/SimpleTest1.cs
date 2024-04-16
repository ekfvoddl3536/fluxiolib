using fluxiolib;
using System.Diagnostics.CodeAnalysis;

namespace FluxIOLib.Test;

[TestClass]
public class SimpleTest1
{
    [TestMethod]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Employee))]
    public void QueryNumInstanceFields()
    {
        int count = FluxTool.GetNumInstanceFields<Employee>();
        Assert.AreEqual(3, count, $"Fail {nameof(FluxTool.GetNumInstanceFields)}");
    }

    [TestMethod]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Employee))]
    public void AccessInstanceField()
    {
        var field = FluxTool.GetInstanceField<Employee>(SStringUtf8.AnyAccept, ProtFlags.Family);

        Assert.AreEqual(field.IsNull, false, $"Fail lookup instance field.");

        Assert.IsTrue(field.FieldOffset >= 0, "Fail lookup offset.", field.FieldOffset);

        const int INPUT_AGE = 30;
        Employee test = new Employee("NAME", INPUT_AGE, Department.Engineering);

        int access_age = field.FieldAccessor.Value<int>(test);

        Assert.AreEqual(INPUT_AGE, access_age, "Wrong field offset.");
    }

    [TestMethod]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Employee))]
    public void GetFieldName()
    {
        var field = FluxTool.GetInstanceField<Employee>(SStringUtf8.AnyAccept, ProtFlags.Family);

        Assert.AreEqual(field.IsNull, false, $"Fail lookup instance field.");

        Assert.IsTrue(field.FieldOffset >= 0, "Fail lookup offset.", field.FieldOffset);

        SStringUtf8 getName = field.GetUtf8NameAsSpanWithoutCaching();

        Assert.IsTrue(getName.Length > 0, "Fail get utf8 field name.");

        Assert.IsTrue(getName.AsSpan().SequenceEqual("age"u8), "FieldName does not match.", getName.ToString(), "age");
    }
}
