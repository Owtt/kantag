using System;

public class Card
{
    public string Name => _name;
    private string _name;

    public DateTime Created => _created;
    private DateTime _created;

    public string Description => _description;
    private string _description;

    public Card(string name, string description = "")
    {
        _name = name;
        _created = DateTime.Now;

        _description = description;
    }
}
