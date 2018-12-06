using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

public class Hoge : MonoBehaviour {

	// Use this for initialization
	void Start () {

 		// //string input = "";
        // //System.Text.StringBuilder text = new System.Text.StringBuilder();
        // Debug.Log("Input the chat.");
        
        // // to type the EOF character and end the input: use CTRL+D, then press <enter>
        // // while ((input = Console.ReadLine()) != "\u0004")
        // // {
        // //     text.AppendLine(input);
        // // }
        
        // AntlrInputStream inputStream = new AntlrInputStream("Masaki says hello\n Moge says yay\n");
        // SpeakLexer speakLexer = new SpeakLexer(inputStream);
        // CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
        // SpeakParser speakParser = new SpeakParser(commonTokenStream);

		// SpeakVisitor visitor = new SpeakVisitor();        
		// speakParser.AddParseListener(visitor);
		
 
        // SpeakParser.ChatContext chatContext = speakParser.chat();
        
		// Debug.LogError("end");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
