using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankSystem : MonoBehaviour
{
    [SerializeField]
    private int maxRankCount = 1;
    [SerializeField]
    private GameObject textPrefab;
    [SerializeField]
    private Transform panelRankInfo;

    private RankData[] rankDataArray;
    private int currentIndex = 0;

    private void Awake()
    {
        rankDataArray = new RankData[maxRankCount];

        LoadRankData();
        CompareRank();
        PrintRankData();
        SaveRankData();
    }

    private void LoadRankData()
    {
        for (int i = 0; i < maxRankCount; ++i)
        {
            rankDataArray[i].score = PlayerPrefs.GetInt("RankScore" + i);
        }
    }

    private void SpawnText(string print, Color color)
    {
        GameObject clone = Instantiate(textPrefab);

        TextMeshProUGUI text = clone.GetComponent<TextMeshProUGUI>();

        clone.transform.SetParent(panelRankInfo);
        clone.transform.localScale = Vector3.one;
        text.text = print;
        text.color = color;
    }

    private void CompareRank()
    {
        RankData currentData = new RankData();
        currentData.score = PlayerPrefs.GetInt("CurrentScore");

        for (int i = 0;i < maxRankCount; ++i)
        {
            if ( currentData.score > rankDataArray[i].score )
            {
                currentIndex = i;
                break;
            }
        }

        for (int i = maxRankCount-1; i > 0; --i)
        {
            rankDataArray[i] = rankDataArray[i-1];

            if ( currentIndex == i-1)
            {
                break;
            }
        }

        rankDataArray[currentIndex] = currentData;
    }

    private void PrintRankData()
    {
        Color color = Color.white;

        for ( int i = 0; i < maxRankCount; ++i )
        {
            color = currentIndex != i ? Color.white : Color.yellow;

            SpawnText((i+1).ToString(), color);
            SpawnText(rankDataArray[i].score.ToString(), color);
        }
    }

    private void SaveRankData()
    {
        for ( int i = 0; i < maxRankCount ; ++i )
        {
            PlayerPrefs.SetInt("RankScore"+1, rankDataArray[i].score);
        }
    }
}

[System.Serializable]
public struct RankData
{
    public int score;
}