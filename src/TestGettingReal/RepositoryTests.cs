using GettingReal.Model;

namespace TestGettingReal;

[TestClass]
public class RepositoryTests
{

   private ActivityRepository repository;
    Activity activity;

    [TestInitialize]
    public void Init()
    {
        repository = new ActivityRepository();
        activity = new Activity();
    }

    [TestCleanup]
    public void Cleanup()
    {
       repository.Clear();
    }

    [TestMethod]
    public void AddActivity_ShouldIncreaseItemCountByOne() //[MethodName]_[Condition/Scenario]_[ExpectedResult]
    {
        //Arrange: Opret repository og en ny aktivitet
      

        //Act: Tilføj (C) aktiviteten 
        repository.Add(activity);

        //Assert: Antallet af elementer skal være 1
        int number = repository.Items.Count;
        Console.WriteLine(repository.Items.Count);
        Assert.AreEqual(1, number);

    }

    [TestMethod]
    public void RemoveActivity_ShouldDecreaseItemCountToZero()
    {
        //Arrange: Opret repository og tilføj en aktivitet direkte til listen
      
        

        //Act: Fjern (D) aktiviteten
        repository.Items.Add(activity);
        repository.Remove(activity);

        //Assert: Listen bør nu være tom
        int number = repository.Items.Count;
        Assert.AreEqual(0, number);
      
    }

    [TestMethod]
    public void UpdateAvtivity_ShouldUpdate()
    {
        //Arrange
        Activity activity2 = new Activity("2D Print", TimeSpan.FromDays(2), "");

        //Act Update (U) aktiviteten
        repository.Items.Add(activity2);
        var act = repository.Get(activity2.GUID); //Read
        act.Name = "3D Print";
        repository.Update(act);
        var act2 = repository.Get(activity2.GUID); //Read

        //Assert
        Assert.IsTrue(repository.Items.Contains(activity2));
        Assert.AreEqual("3D Print", act2.Name);


    }

    [TestMethod]
    public void Get_ShouldGet()
    {
        //Arrange
       

        //Act Læs (R) aktiviteten
        repository.Items.Add(activity);
        var act = repository.Get(activity.GUID); //Read

        //Assert
        Assert.AreEqual(activity.GUID, act.GUID);

    }

    [TestMethod]
    public void RemoveGuid_ShouldRemoveItem()
    {
        //Arrange
        repository.Add(activity);

        //Act
        repository.Remove(activity.GUID);

        //Assert
        Assert.AreEqual(0, repository.Items.Count);

    }

    [TestMethod]
    public void GetExistingName_ShouldReturnItem()
    {
        //Arrange
        var activity2 = new Activity { Name = "MicroBit" };
        repository.Items.Add(activity2);

        //Act
        var result = repository.Get("MicroBit");

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("MicroBit", result.Name);

    }

    [TestMethod]
    public void GetAll_ShouldGetAllItems()
    {
        //Arrange
        repository.Add(activity);

        //Act
        var activities = repository.GetAll();

        //Assert
        Assert.AreEqual(1, activities.Count);
    }
}



