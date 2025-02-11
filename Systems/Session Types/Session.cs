using System.Collections.Generic;

public class Session
{
    public string Name => _name;
    private string _name;

    public List<Category> Categories => _categories;
    private List<Category> _categories = new List<Category>();
}
