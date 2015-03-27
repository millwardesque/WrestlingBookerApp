
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Venue : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Venue data = (Venue)obj;
		// Add your writer.Write calls here.
		writer.Write(data.id);
		writer.Write(data.venueName);
		writer.Write(data.venueDescription);
		writer.Write(data.baseCost);
		writer.Write(data.gatePercentage);
		writer.Write(data.capacity);
		writer.Write(data.popularity);
		writer.Write(data.phase);
		writer.Write(data.unlockableMatchType);
		writer.Write(data.matchTypePreferences);
		writer.Write(data.matchFinishPreferences);
		writer.Write(data.seenMatchTypes);
		writer.Write(data.seenMatchFinishes);

	}
	
	public override void Read(ES2Reader reader, Component c)
	{
		Venue data = (Venue)c;
		// Add your reader.Read calls here to read the data into the Component.
		data.id = reader.Read<System.String>();
		data.venueName = reader.Read<System.String>();
		data.venueDescription = reader.Read<System.String>();
		data.baseCost = reader.Read<System.Single>();
		data.gatePercentage = reader.Read<System.Single>();
		data.capacity = reader.Read<System.Int32>();
		data.popularity = reader.Read<System.Single>();
		data.phase = reader.Read<System.Int32>();
		data.unlockableMatchType = reader.Read<System.String>();
		data.matchTypePreferences = reader.ReadDictionary<System.String,System.Single>();
		data.matchFinishPreferences = reader.ReadDictionary<System.String,System.Single>();
		data.seenMatchTypes = reader.ReadList<System.String>();
		data.seenMatchFinishes = reader.ReadList<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	
	public ES2UserType_Venue():base(typeof(Venue)){}
	
	public override object Read(ES2Reader reader)
	{
		Venue param = GetOrCreate<Venue>();
		Read(reader, param);
		return param;
	}
}
