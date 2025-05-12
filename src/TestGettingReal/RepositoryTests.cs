using GettingReal.Model;

namespace TestGettingReal;

[TestClass]
public sealed class RepositoryTests
{
    [TestMethod]
    public void AddActivity_ShouldIncreaseItemCountByOne()
    {
        //Arrange: Opret repository og en ny aktivitet
        Activity activity = new Activity();
        ActivityRepository repository = new ActivityRepository();

        //Act: Tilføj aktiviteten
        repository.Add(activity);

        //Assert: Antallet af elementer skal være 1
        int number = repository.Items.Count;
        Assert.AreEqual(1, number);


    }

    [TestMethod]
    public void RemoveActivity_ShouldDecreaseItemCountToZero()
    {
        //Arrange: Opret repository og tilføj en aktivitet direkte til listen
        Activity activity = new Activity();
        ActivityRepository repository = new ActivityRepository();

        //Act: Fjern aktiviteten
        repository.Items.Add(activity);
        repository.Remove(activity);

        //Assert: Listen bør nu være tom
        int number = repository.Items.Count;
        Assert.AreEqual(0, number);
    }

    [TestMethod]
    public void Update_ShouldUpdate()
    {
        //Arrange
        Activity activity = new Activity("2D Print", TimeSpan.FromDays(2));
        ActivityRepository repository = new ActivityRepository();

        //Act
        repository.Items.Add(activity);
        var act = repository.Get(activity.GUID); //Read
        act.Name = "3D Print";
        repository.Update(act);
        var act2 = repository.Get(activity.GUID); //Read

        //Assert
        Assert.IsTrue(repository.Items.Contains(activity));
        Assert.AreEqual("3D Print", act2.Name);

    }

    [TestMethod]
    public void Get_ShouldGet()
    {
        //Arrange
        Activity activity = new Activity();
        ActivityRepository repository = new ActivityRepository();

        //Act
        repository.Items.Add(activity);
        var act = repository.Get(activity.GUID); //Read

        //Assert
        Assert.AreEqual(activity.GUID, act.GUID);
    }
}



