#pragma warning disable

namespace FluxIOLib.Test;

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