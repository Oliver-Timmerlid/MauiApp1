namespace MauiApp1.Model;

public class Device : IDevice
{
    public string Name { get; set; }
    //public string Id { get; set; }

    public override string ToString()
    {
        return $"{Name}";
    }
}
