using UnityEngine;

public class SkinPriceFetcher : MonoBehaviour
{
    [System.Serializable]
    public class SkinPrice
    {
        public ModelType SkinType;
        public int Price;
    }

    public int DefaultSkinPrice = 30;
    public SkinPrice[] Prices;


    public int GetPrice()
    {
        return DefaultSkinPrice;
    }

    public int GetPrice(ModelType SkinType)
    {
        switch (SkinType)
        {
            default:
                return DefaultSkinPrice;
        }
    }
}
