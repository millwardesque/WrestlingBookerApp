
using System; 
using UnityEngine; 
using System.Collections; 
using System.Collections.Generic;

[ExecuteInEditMode]				
public class ES2Init : MonoBehaviour
{
	public void Awake()
	{
		Init();
	}
	
	public void Start()
	{
		if(Application.isEditor)
			GameObject.DestroyImmediate(gameObject);
		else
			GameObject.Destroy(gameObject);
	}

	public static void Init()
	{
		ES2TypeManager.types = new Dictionary<Type, ES2Type>();
				ES2TypeManager.types[typeof(UnityEngine.Vector2)] = new ES2_Vector2();
		ES2TypeManager.types[typeof(UnityEngine.Vector3)] = new ES2_Vector3();
		ES2TypeManager.types[typeof(UnityEngine.Vector4)] = new ES2_Vector4();
		ES2TypeManager.types[typeof(UnityEngine.Texture2D)] = new ES2_Texture2D();
		ES2TypeManager.types[typeof(UnityEngine.Quaternion)] = new ES2_Quaternion();
		ES2TypeManager.types[typeof(UnityEngine.Mesh)] = new ES2_Mesh();
		ES2TypeManager.types[typeof(UnityEngine.Color)] = new ES2_Color();
		ES2TypeManager.types[typeof(UnityEngine.Color32)] = new ES2_Color32();
		ES2TypeManager.types[typeof(UnityEngine.Material)] = new ES2_Material();
		ES2TypeManager.types[typeof(UnityEngine.Rect)] = new ES2_Rect();
		ES2TypeManager.types[typeof(UnityEngine.Bounds)] = new ES2_Bounds();
		ES2TypeManager.types[typeof(UnityEngine.Transform)] = new ES2_Transform();
		ES2TypeManager.types[typeof(UnityEngine.BoxCollider)] = new ES2_BoxCollider();
		ES2TypeManager.types[typeof(UnityEngine.CapsuleCollider)] = new ES2_CapsuleCollider();
		ES2TypeManager.types[typeof(UnityEngine.SphereCollider)] = new ES2_SphereCollider();
		ES2TypeManager.types[typeof(UnityEngine.MeshCollider)] = new ES2_MeshCollider();
		ES2TypeManager.types[typeof(System.Boolean)] = new ES2_bool();
		ES2TypeManager.types[typeof(System.Byte)] = new ES2_byte();
		ES2TypeManager.types[typeof(System.Char)] = new ES2_char();
		ES2TypeManager.types[typeof(System.Decimal)] = new ES2_decimal();
		ES2TypeManager.types[typeof(System.Double)] = new ES2_double();
		ES2TypeManager.types[typeof(System.Single)] = new ES2_float();
		ES2TypeManager.types[typeof(System.Int32)] = new ES2_int();
		ES2TypeManager.types[typeof(System.Int64)] = new ES2_long();
		ES2TypeManager.types[typeof(System.Int16)] = new ES2_short();
		ES2TypeManager.types[typeof(System.String)] = new ES2_string();
		ES2TypeManager.types[typeof(System.UInt32)] = new ES2_uint();
		ES2TypeManager.types[typeof(System.UInt64)] = new ES2_ulong();
		ES2TypeManager.types[typeof(System.UInt16)] = new ES2_ushort();
		ES2TypeManager.types[typeof(System.Enum)] = new ES2_Enum();
		ES2TypeManager.types[typeof(UnityEngine.Matrix4x4)] = new ES2_Matrix4x4();
		ES2TypeManager.types[typeof(UnityEngine.BoneWeight)] = new ES2_BoneWeight();
		ES2TypeManager.types[typeof(UnityEngine.SkinnedMeshRenderer)] = new ES2_SkinnedMeshRenderer();
		ES2TypeManager.types[typeof(System.SByte)] = new ES2_sbyte();
		ES2TypeManager.types[typeof(Company)] = new ES2UserType_Company();
		ES2TypeManager.types[typeof(HistoricalWrestlingEvent)] = new ES2UserType_HistoricalWrestlingEvent();
		ES2TypeManager.types[typeof(Venue)] = new ES2UserType_Venue();
		ES2TypeManager.types[typeof(Wrestler)] = new ES2UserType_Wrestler();
		ES2TypeManager.types[typeof(WrestlingMatchFinish)] = new ES2UserType_WrestlingMatchFinish();
		ES2TypeManager.types[typeof(WrestlingMatchType)] = new ES2UserType_WrestlingMatchType();
		ES2TypeManager.types[typeof(UnityEngine.Sprite)] = new ES2_Sprite();
		ES2TypeManager.types[typeof(SavedGame)] = new ES2UserType_SavedGame();

	}
}
