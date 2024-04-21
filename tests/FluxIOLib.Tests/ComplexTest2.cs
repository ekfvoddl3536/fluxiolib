#if !__FEATURES_MINI__
using fluxiolib;
using System.Diagnostics.CodeAnalysis;

namespace FluxIOLib.Test;

[TestClass]
public class ComplexTest2
{
    [TestMethod]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SeniorManager))]
    public void UseFilter()
    {
        var space = new FluxSearchSpace(1, 0);
        
        var list = FluxTool.GetInstanceFieldList<SeniorManager>(ProtFlags.All, space: space);

        var arr = list.ToArray();

        Assert.AreEqual(arr.Length, 5, "Fail apply filter");

        var names = arr.Select(x => x.GetNameWithoutCaching()).ToArray();

        Assert.IsTrue(names.Contains("age"), "'age' not found");
        Assert.IsTrue(names.Contains("teamSize"), "'teamSize' not found");
        Assert.IsTrue(names.Contains("region"), "'region' not found");
        Assert.AreEqual(names.Count(x => x == "department"), 2, "'department' wrong");
    }

    [TestMethod]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SeniorManager))]
    public void IndexExpress()
    {
        var space = new FluxSearchSpace(^3);

        var list = FluxTool.GetInstanceFieldList<SeniorManager>(ProtFlags.All, space: space);

        var arr = list.ToArray();

        Assert.AreEqual(arr.Length, 3, "Fail apply filter");

        var names = arr.Select(x => x.GetNameWithoutCaching()).ToArray();

        Assert.IsTrue(names.Contains("teamSize"), "'teamSize' not found");
        Assert.IsTrue(names.Contains("region"), "'region' not found");
        Assert.IsTrue(names.Contains("department"), "'region' not found");
    }
}
#endif