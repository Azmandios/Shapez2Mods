using UnityEngine;

namespace Shapez2Mods.FastForwarder.UI
{
    public class FastForwarderUI : MonoBehaviour
    {
        private readonly float[] speeds = { 0f, 1f, 5f, 10f, 25f };
        private readonly string[] labels = { "II", ">", ">>", ">>>", ">|" };
        private int selectedIndex = 1;

        private void OnGUI()
        {
            if (FastForwarder.SimManager == null)
                return;

            GUI.BeginGroup(new Rect(Screen.width - 210, 120, 200, 40));

            GUIStyle customButtonStyle = new GUIStyle(GUI.skin.button);
            customButtonStyle.fontSize = 14;
            customButtonStyle.fontStyle = FontStyle.Bold;
            customButtonStyle.normal.textColor = Color.white;
            customButtonStyle.hover.textColor = Color.white;

            for (int i = 0; i < speeds.Length; i++)
            {
                if (i == selectedIndex)
                {
                    GUI.backgroundColor = new Color(1.0f, 0.55f, 0.1f);
                }
                else
                {
                    GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
                }

                if (GUI.Button(new Rect(i * 38, 0, 35, 30), labels[i], customButtonStyle))
                {
                    selectedIndex = i;
                    FastForwarder.SetSpeed(speeds[i]);
                }
            }

            GUI.EndGroup();
            
            GUI.backgroundColor = Color.white; 

            if (FastForwarder.CurrentGameCore == null)
            {
                FastForwarder.Log.LogInfo("[FastForwarder] Destroying UI GameObject as GameCore is null.");
                Destroy(this.gameObject);
            }
        }
    }
}