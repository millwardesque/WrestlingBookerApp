
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_WrestlingMatchFinish : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		WrestlingMatchFinish data = (WrestlingMatchFinish)obj;
		// Add your writer.Write calls here.
		writer.Write(data.finishName);
		writer.Write(data.description);
		writer.Write(data.phase);

	}
	
	public override object Read(ES2Reader reader)
	{
		WrestlingMatchFinish data = new WrestlingMatchFinish();
		// Add your reader.Read calls here and return your object.
		data.finishName = reader.Read<System.String>();
		data.description = reader.Read<System.String>();
		data.phase = reader.Read<System.Int32>();

		return data;
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_WrestlingMatchFinish():base(typeof(WrestlingMatchFinish)){}
}
