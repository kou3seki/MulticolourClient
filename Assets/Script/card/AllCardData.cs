using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AllCardData
{
    const string DATA_PATH = "/Resources/CardInfo/CardData.txt";

    public static List<CardData> allCardDatas_list = new List<CardData>();
    public static Dictionary<string, CardData> allCardDatas = AllCardImport();
    public static Sprite transparent = Resources.Load<Sprite>("CardImage/Transparent");
    public static Sprite filledSqure = Resources.Load<Sprite>("CardImage/Filled");
    public static Sprite rect = Resources.Load<Sprite>("CardImage/Rect");
    public static Sprite back = Resources.Load<Sprite>("CardImage/BackOfCard");
    public static CardData sweetpotato = new CardData(Resources.Load<Sprite>("CardImage/Tool/SweetPotato"),"[秋穰子的红薯]", "Sweetpotato", 
        new List<string>(), "将此牌永久翻面，视为支付了一点任意颜色值", new int[] {0, 0, 0, 0, 0}, null, -1);
    public static CardData derivant = new CardData(filledSqure, "衍生物", "Derivant",
        new List<string>(), "此牌为衍生物，可在其上输入标识", new int[] { 0, 0, 0, 0, 0 }, null, -2);

    public static Dictionary<string, CardData> AllCardImport()
    {
        Dictionary<string, CardData> allCardDatas = new Dictionary<string, CardData>();
        int cardOrder = 0;
        string currentPath = "";
        string rootPath ="CardImage/";
        string[] datas = File.ReadAllLines(Application.dataPath + DATA_PATH);

        foreach (string data in datas)
        {
            if (data[0] == '#') currentPath = data.Substring(1);
            else
            {
                CardData cardData = Deserialization(data, rootPath + currentPath + "/", cardOrder);
                allCardDatas.Add(cardData.NameC, cardData);
                allCardDatas_list.Add(cardData);
                cardOrder++;
            }
        }

        return allCardDatas;
    }

    public static CardData Deserialization(string input, string path, int cardOrder)
    {
        string[] data = input.Split('^');
        
        string[] tags = data[4].Split('·');
        List<string> allTags = new List<string>();
        foreach (string tag in tags)
        {
            allTags.Add(tag);
        }

        string[] baseCost_ = data[2].Split('/');
        int[] baseCost = {int.Parse(baseCost_[0]), int.Parse(baseCost_[1]), int.Parse(baseCost_[2]), int.Parse(baseCost_[3]), int.Parse(baseCost_[4]), int.Parse(baseCost_[5]) };

        if(data.Length == 5)
        {
            return new CardData(Resources.Load<Sprite>(path + data[0]), data[1], data[0], allTags, data[4] + "\n\n" + data[3], baseCost, null, cardOrder);
        }
        else
        {
            string[] baseValue_ = data[5].Split('/');
            int[] baseValue = { int.Parse(baseValue_[0]), int.Parse(baseValue_[1]), int.Parse(baseValue_[2]) };
            if(data.Length == 6)
            {
                return new CardData(Resources.Load<Sprite>(path + data[0]), data[1], data[0], allTags, data[4] + "\n\n" + data[3], baseCost, baseValue, cardOrder);
            }
            else
            {
                return new CardData(Resources.Load<Sprite>(path + data[0]), data[1], data[0], allTags, data[4] + 
                    "\n\n自：" + data[6] + "\n\n主：" + data[3], baseCost, baseValue, cardOrder);
            }
        }
    }
}

public readonly struct CardData
{
    public Sprite Image {get;}
    public string NameC { get; }
    public string NameE { get; }
    public List<string> Tags { get; }
    public string Describe { get; }
    public int MaxNum { get; }
    public int[] BaseCost { get; }
    public int[] BaseValue { get; }
    public int CardOrder { get; }

    public CardData(Sprite sprite, string nameC, string nameE, List<string> tags, string describe, int[] baseCost,  int[] baseValue, int cardOrder)
    {
        Tags = new List<string>();
        Image = sprite;
        NameC = nameC;
        NameE = nameE;
        Tags = tags;
        Describe = describe;
        BaseCost = baseCost;
        BaseValue = baseValue;
        CardOrder = cardOrder;

        if (describe.Contains("终言")) MaxNum = 1;
        else MaxNum = 4;
    }
}
