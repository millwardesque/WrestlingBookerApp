
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Company : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Company data = (Company)obj;
		// Add your writer.Write calls here.
		writer.Write(data.companyName);
		writer.Write(data.money);
		writer.Write(data.maxRosterSize);
		writer.Write(data.phase);
		writer.Write(data.eventHistory);
		writer.Write(data.isInAlliance);

	}
	
	public override void Read(ES2Reader reader, Component c)
	{
		Company data = (Company)c;
		// Add your reader.Read calls here to read the data into the Component.
		data.companyName = reader.Read<System.String>();
		data.money = reader.Read<System.Single>();
		data.maxRosterSize = reader.Read<System.Int32>();
		data.phase = reader.Read<System.Int32>();
		data.eventHistory = reader.ReadList<HistoricalWrestlingEvent>();
		data.isInAlliance = reader.Read<System.Boolean>();

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
