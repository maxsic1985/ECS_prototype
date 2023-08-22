using MSuhininTestovoe.B2B;
using UnityEngine;



public class StartGame : MonoBehaviour
{
    public void StartGameClick()
   {
       Application.LoadLevelAsync((int) SceeneType.GAME);
   }
}
