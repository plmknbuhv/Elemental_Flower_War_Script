using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "SO/Flower/FlowerDataSO")]
public class FlowerElementDataSO : ScriptableObject
{
    public SpriteLibraryAsset bulletSpriteLibrary;
    public FlowerElementType flowerType;
    public float knockBackPower;
    public Sprite sprite;
    public Color color;
    public int damage;
}
