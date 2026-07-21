HOW TO MAKE AN NPC
1. In Assets/NPC, create new folder
2. Create class in folder extending NonPlayableCharacter, implement abstract void OnTalk() and (optionally) Init()
3. Create NonPlayableCharacterSO in folder, assign name and ID
4. Drag NPC Prefab into scene, attach script to prefab, attach SO to script

DIALOGUE
1. In Assets/Resources/DialogueFiles, create folder for NPC
2. Create JSON File
3. In OnTalk(): conversation.ExecuteDialogue(path relative to DialogueFiles)
JSON Example (i know json doesn't have comments but im using them anyway bc this is a text file):
{
    "startingPath": "example", 
    "data": [
        { // Path
            "name": "example",
            "dialogue": [
                { // One dialogue box
                    "dialogue": "Go to sleep and end the day early?", 
		    "signal": "ExampleSignal", // Optional, sends dialogue signal once user leaves dialoguebox
		    "wait": .3 // Optional, how much time to wait after dialogue
                    "buttons": [ // Optional
                        {
                            "id": "EndDayEarly", // will send this as signal when clicked
                            "text": "Yes"
                        },
                        {
                            "id": "EndDayEarlyNegative",
                            "text": "No"
			    "path": "example2" // dialogue path to execute when button pressed
                        }
                    ]
                }
            ]
        }, 
	{
            "name": "example2",
            "dialogue": [
                { 
                    "dialogue": "You suck" 
                }
            ]
        }

    ]
}


Processing dialogueSignals: DialogueRenderer will send out a DialogueSignal when a button is clicked or a dialogueBox with "signal" assigned is destroyed. An abstract class DialogueSignalHandler exists which allows the processing of DialogueSignals (implement protected abstract void HandleDialogueSignal(System.Object sender, EventArgs e)). SympathyPointsManager handles any dialogue or button with the signal "AddPoints" (adding either the DialogeBox's or button's "points").

SPECIAL CASES FOR NPC:
If you need to create an NPC that only has two conversations (one for the initial interaction and one for every interaction afterwards), attach the concrete class OneInteractNPC to the GameObject.