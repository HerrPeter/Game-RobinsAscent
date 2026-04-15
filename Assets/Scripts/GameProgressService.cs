using System;
using UnityEngine;

[Serializable]
public class LevelProgressData
{
    public bool unlocked;
    public bool completed;
    public int bestCoinsCollected;
    public float bestHeightReached;
}

[Serializable]
public class GameProgressData
{
    public int currentLevel = 1;
    public int highestUnlockedLevel = 1;
    public LevelProgressData[] levels = new LevelProgressData[0];
}

public static class GameProgressService
{
    private const string SaveKey = "GameProgress";

    public static GameProgressData Load(int totalLevels)
    {
        GameProgressData data;

        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            data = JsonUtility.FromJson<GameProgressData>(json);
        }
        else
        {
            data = new GameProgressData();
        }

        if (data == null)
        {
            data = new GameProgressData();
        }

        EnsureLevelData(data, totalLevels);
        Save(data);

        return data;
    }

    public static void Save(GameProgressData data)
    {
        if (data == null)
        {
            return;
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void SetCurrentLevel(GameProgressData data, int levelNumber)
    {
        if (data == null)
        {
            return;
        }

        data.currentLevel = Mathf.Clamp(levelNumber, 1, data.levels.Length);
        Save(data);
    }

    public static void RecordLevelAttempt(GameProgressData data, int levelNumber, int coinsCollected, float heightReached)
    {
        if (!TryGetLevel(data, levelNumber, out LevelProgressData level))
        {
            return;
        }

        level.bestCoinsCollected = Mathf.Max(level.bestCoinsCollected, coinsCollected);
        level.bestHeightReached = Mathf.Max(level.bestHeightReached, heightReached);
        data.currentLevel = levelNumber;

        Save(data);
    }

    public static void RecordLevelComplete(GameProgressData data, int levelNumber, int coinsCollected, float heightReached)
    {
        if (!TryGetLevel(data, levelNumber, out LevelProgressData level))
        {
            return;
        }

        level.completed = true;
        level.bestCoinsCollected = Mathf.Max(level.bestCoinsCollected, coinsCollected);
        level.bestHeightReached = Mathf.Max(level.bestHeightReached, heightReached);

        int nextLevel = Mathf.Clamp(levelNumber + 1, 1, data.levels.Length);
        data.highestUnlockedLevel = Mathf.Max(data.highestUnlockedLevel, nextLevel);

        if (levelNumber < data.levels.Length)
        {
            data.levels[levelNumber].unlocked = true;
            data.currentLevel = levelNumber + 1;
        }
        else
        {
            data.currentLevel = levelNumber;
        }

        Save(data);
    }

    public static bool IsLevelUnlocked(GameProgressData data, int levelNumber)
    {
        return TryGetLevel(data, levelNumber, out LevelProgressData level) && level.unlocked;
    }

    public static void Reset(int totalLevels)
    {
        GameProgressData data = new GameProgressData();
        EnsureLevelData(data, totalLevels);
        Save(data);
    }

    private static void EnsureLevelData(GameProgressData data, int totalLevels)
    {
        if (totalLevels <= 0)
        {
            totalLevels = 1;
        }

        if (data.levels == null || data.levels.Length != totalLevels)
        {
            LevelProgressData[] oldLevels = data.levels ?? new LevelProgressData[0];
            LevelProgressData[] newLevels = new LevelProgressData[totalLevels];

            for (int i = 0; i < totalLevels; i++)
            {
                if (i < oldLevels.Length && oldLevels[i] != null)
                {
                    newLevels[i] = oldLevels[i];
                }
                else
                {
                    newLevels[i] = new LevelProgressData();
                }
            }

            data.levels = newLevels;
        }

        if (data.levels.Length > 0)
        {
            data.levels[0].unlocked = true;
        }

        data.highestUnlockedLevel = Mathf.Clamp(data.highestUnlockedLevel, 1, totalLevels);
        data.currentLevel = Mathf.Clamp(data.currentLevel, 1, totalLevels);
    }

    private static bool TryGetLevel(GameProgressData data, int levelNumber, out LevelProgressData level)
    {
        level = null;

        if (data == null || data.levels == null)
        {
            return false;
        }

        int index = levelNumber - 1;

        if (index < 0 || index >= data.levels.Length)
        {
            return false;
        }

        level = data.levels[index];
        return level != null;
    }
}
