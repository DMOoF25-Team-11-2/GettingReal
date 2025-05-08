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
        repository.Items.Add(activity);

        //Act: Fjern aktiviteten
        repository.Remove(activity);

        //Assert: Listen bør nu være tom
        int number = repository.Items.Count;
        Assert.AreEqual(0, number);
    }
}


