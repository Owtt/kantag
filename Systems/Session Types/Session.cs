using System.Collections.Generic;

public class Session
{
    public string Name => _name;
    private string _name;

    public Dictionary<string, Category> Categories => _categories;
    private Dictionary<string, Category> _categories = new Dictionary<string, Category>();

    public Session(string name, Category category)
    {
        _name = name;
        _categories.Add(category.Name, category);
    }
}
