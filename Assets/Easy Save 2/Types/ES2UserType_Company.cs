
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Company : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Company data = (Company)obj;
		// Add your writer.Write calls here.
		writer.Write(data.name);
		writer.Write(data.id);
		writer.Write(data.companyName);
		writer.Write(data.money);
		writer.Write(data.maxRosterSize);
		writer.Write(data.phase);
		writer.Write(data.eventHistory);
		writer.Write(data.isInAlliance);
		writer.Write(data.roster);

	}
	
	public override void Read(ES2Reader reader, Component c)
	{
		Company data = (Company)c;
		// Add your reader.Read calls here to read the data into the Component.
		data.name = reader.Read<System.String>();
		data.id = reader.Read<System.String>();
		data.companyName = reader.Read<System.String>();
		data.money = reader.Read<System.Single>();
		data.maxRosterSize = reader.Read<System.Int32>();
		data.phase = reader.Read<System.Int32>();
		data.eventHistory = reader.ReadList<HistoricalWrestlingEvent>();
		data.isInAlliance = reader.Read<System.Boolean>();
		data.roster = reader.ReadList<Wrestler>();

	}
	
	/* ! Don't modify anything below this line ! */
	
	public ES2UserType_Company():base(typeof(Company)){}
	
	public override object Read(ES2Reader reader)
	{
		Company param = GetOrCreate<Company>();
		Read(reader, param);
		return param;
	}
}
