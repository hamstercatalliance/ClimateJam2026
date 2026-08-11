using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class DialogueButton {
    [JsonProperty("id")]
    public string id { get; set; }
    [JsonProperty("text")]
    public string text { get; set; }
    [JsonProperty("path")]
    public string path { get; set; }

    [JsonProperty("points")]
    public int? points = null;
}


[Serializable]
public class DialogueBox {

    // Start is called before the first frame update

    [JsonProperty("dialogue")]
    public string dialogue;
    [JsonProperty("characterID")]
    public string characterID;

    public static bool dialogueActive { get; set; } = false;
    public bool active { get; set; }

    [JsonProperty("signal", Required = Required.Default)]
    public string signal = "";

    public bool lastBox;
    [JsonProperty("wait",Required=Required.Default)]
    public float? wait = null;


    [JsonProperty("pointsAdded", Required = Required.Default)]
    public int? sympathyPointsChange = null;

    [JsonProperty("buttons", Required = Required.Default)]
    public List<DialogueButton> buttons = new List<DialogueButton>();

    [JsonProperty("link", Required = Required.Default)]
    public string link = "";

    public void setInactive()
    {
        this.active = false;
    }

    public string getContent()
    {
        return dialogue;
    }

    public string getCharacterID()
    {
        return characterID;
    }
}
