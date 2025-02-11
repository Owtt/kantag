using System.Collections.Generic;

public class Tag
{
    public string Name => _name;
    private string _name;

    public string Category => _category;
    private string _category;

    public List<Card> Cards => _cards;
    private List<Card> _cards = new List<Card>();

    public Tag(string name, string category)
    {
        _name = name;
        _category = category;
    }

    public void AddCard(Card card)
    {
        _cards.Add(card);
    }
}
