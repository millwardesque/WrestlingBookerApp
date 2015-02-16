
public class WrestlingMatchType {
	public string typeName;
	public string description;
	public int phase;
	
	public void Initialize(string typeName, string description, int phase) {
		this.typeName = typeName;
		this.description = description;
		this.phase = phase;
	}
}
