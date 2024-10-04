using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public void ShowRanking()
    {
        List<GameData> gameDataList = SaveSystem.LoadGameData(); // Cargar el ranking
        gameDataList.Sort((x, y) => y.points.CompareTo(x.points)); // Ordenar por puntos de mayor a menor

        foreach (GameData gameData in gameDataList)
        {
            Debug.Log($"Puntos: {gameData.points}, Tiempo: {gameData.timePlayed} segundos, Fecha: {gameData.saveDate}");
        }
    }
}
