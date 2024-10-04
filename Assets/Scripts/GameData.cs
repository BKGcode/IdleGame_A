using System;

[Serializable]
public class GameData
{
    public int points; // Puntos conseguidos en la partida
    public float timePlayed; // Tiempo en segundos jugado en la partida
    public string saveDate; // Fecha y hora cuando se guardó la partida

    // Constructor para inicializar los datos de la partida
    public GameData(int points, float timePlayed)
    {
        this.points = points;
        this.timePlayed = timePlayed;
        this.saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Formato para fecha y hora
    }
}
