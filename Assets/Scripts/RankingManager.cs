using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public void LoadRankings()
    {
        GameData[] rankings = SaveSystem.LoadRankings();
        
        // Mostrar los datos de rankings en la UI
        // Aquí puedes implementar la lógica para mostrar la lista de rankings
    }
}
