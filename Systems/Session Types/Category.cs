using System.Collections.Generic;

public class Category
{
    public string Name => _name;
    private string _name;

    public Dictionary<string, Tag> Tags => _tags;
    private Dictionary<string, Tag> _tags = new Dictionary<string, Tag>();

    public Category(string name)
    {
        _name = name;
    }

    public void AddTag(Tag tag)
    {
        _tags.Add(tag.Name, tag);
    }
}
