using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class UserData : Resource
{
    public List<string> dataPaths = new List<string>();
    public string ChosenPath = "";

    public Session lastSession;
    public Session defaultSession;
}
