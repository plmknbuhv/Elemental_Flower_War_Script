using UnityEngine;

[CreateAssetMenu(menuName = "SO/Flower/FlowerDataListSO")]
public class FlowerElementDataListSO : ScriptableObject
{
    public SerializableDictionary<FlowerElementType, FlowerElementDataSO> elementDataDictionary;
}
