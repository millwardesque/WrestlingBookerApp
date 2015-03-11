
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Wrestler : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Wrestler data = (Wrestler)obj;
		// Add your writer.Write calls here.
		writer.Write(data.id);
		writer.Write(data.wrestlerName);
		writer.Write(data.description);
		writer.Write(data.perMatchCost);
		writer.Write(data.popularity);
		writer.Write(data.isHeel);
		writer.Write(data.hiringCost);
		writer.Write(data.phase);
		writer.Write(data.charisma);
		writer.Write(data.work);
		writer.Write(data.appearance);
		writer.Write(data.matchTypeAffinities);
		writer.Write(data.usedMatchTypes);

	}
	
	public override void Read(ES2Reader reader, Component c)
	{
		Wrestler data = (Wrestler)c;
		// Add your reader.Read calls here to read the data into the Component.
		data.id = reader.Read<System.String>();
		data.wrestlerName = reader.Read<System.String>();
		data.description = reader.Read<System.String>();
		data.perMatchCost = reader.Read<System.Single>();
		data.popularity = reader.Read<System.Single>();
		data.isHeel = reader.Read<System.Boolean>();
		data.hiringCost = reader.Read<System.Single>();
		data.phase = reader.Read<System.Int32>();
		data.charisma = reader.Read<System.Single>();
		data.work = reader.Read<System.Single>();
		data.appearance = reader.Read<System.Single>();
		data.matchTypeAffinities = reader.ReadDictionary<System.String,System.Single>();
		data.usedMatchTypes = reader.ReadList<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	
	public ES2UserType_Wrestler():base(typeof(Wrestler)){}
	
	public override object Read(ES2Reader reader)
	{
		Wrestler param = GetOrCreate<Wrestler>();
		Read(reader, param);
		return param;
	}
}
