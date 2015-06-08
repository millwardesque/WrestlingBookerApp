
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_SavedGame : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		SavedGame data = (SavedGame)obj;
		// Add your writer.Write calls here.
		writer.Write(data.gameID);

	}
	
	public override object Read(ES2Reader reader)
	{
		SavedGame data = new SavedGame();
		// Add your reader.Read calls here and return your object.
		data.gameID = reader.Read<System.String>();

		return data;
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_SavedGame():base(typeof(SavedGame)){}
}
