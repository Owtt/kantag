using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

[GlobalClass]
public partial class UserCardData : Resource
{
    public Dictionary<string, Session> Sessions = new Dictionary<string, Session>();

    public Dictionary<string, Category> Categories = new Dictionary<string, Category>();

    public Dictionary<string, Tag> Tags = new Dictionary<string, Tag>();

    public List<Card> Cards = new List<Card>();

    public Session CurrentSession;

    private string _jsonFile = "user-card-data.json";
    private string _jsonPath;

    public void Initialize()
    {
        _jsonPath = OS.GetUserDataDir() + "/" + _jsonFile;

        if (!File.Exists(_jsonPath))
        {
            // start new user protocol
            GenerateFirstTimeCards();
        }
        else
        {
            Load();
        }
    }

    private void GenerateFirstTimeCards()
    {
        Category project = new("Project");

        Tag now = new("Now", project.Name);
        Tag notes = new("Notes", project.Name);
        Tags.Add("Now", now);
        Tags.Add("Notes", notes);

        Card c1 = new("Getting Started", "Kantag uses card, at a glance the title");
        Card c2 = new(
            "Category, Tags, Cards",
            "Cards fit nicely into Tags. Tags fit nicely into Categories.\nThis gives us a nice way to categorize like Cards and like Tags into Tags and Categories respectively."
        );
        Card c3 = new("Drag!", "Drag Categories, Tags, and Cards to reorganize them.");

        Cards.Add(c1);
        Cards.Add(c2);
        Cards.Add(c3);

        now.AddCard(c1);
        now.AddCard(c3);
        notes.AddCard(c2);

        project.AddTag(now);
        project.AddTag(notes);

        Categories.Add(project.Name, project);

        Session startingSession = new Session("Starting Session", project);

        Sessions.Add(startingSession.Name, startingSession);

        CurrentSession = startingSession;
    }

    private void Load()
    {
        // find _jsonFile
        // parse json into data structures above
    }

    private void Save()
    {
        // write to json
        // create/find json _jsonFile
        // sum up data structures above
        // Save

        string json = JsonSerializer.Serialize(this);

        FileStream cardDataStream = new FileStream(_jsonPath, FileMode.OpenOrCreate);
    }
}
