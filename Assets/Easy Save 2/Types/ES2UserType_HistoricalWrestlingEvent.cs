
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_HistoricalWrestlingEvent : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		HistoricalWrestlingEvent data = (HistoricalWrestlingEvent)obj;
		// Add your writer.Write calls here.
		writer.Write(data.name);
		writer.Write(data.revenue);
		writer.Write(data.type);
		writer.Write(data.venue);
		writer.Write(data.interest);
		writer.Write(data.rating);
		writer.Write(data.ticketsSold);

	}
	
	public override object Read(ES2Reader reader)
	{
		HistoricalWrestlingEvent data = new HistoricalWrestlingEvent();
		// Add your reader.Read calls here and return your object.
		data.name = reader.Read<System.String>();
		data.revenue = reader.Read<System.Single>();
		data.type = reader.Read<System.String>();
		data.venue = reader.Read<System.String>();
		data.interest = reader.Read<System.Single>();
		data.rating = reader.Read<System.Single>();
		data.ticketsSold = reader.Read<System.Int32>();

		return data;
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_HistoricalWrestlingEvent():base(typeof(HistoricalWrestlingEvent)){}
}
