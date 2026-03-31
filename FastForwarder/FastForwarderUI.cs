using UnityEngine;

namespace Shapez2Mods.FastForwarder.UI
{
    public class FastForwarderUI : MonoBehaviour
    {
        private readonly float[] speeds = { 0f, 1f, 5f, 10f, 25f };
        private readonly string[] labels = { "II", ">", ">>", ">>>", ">|" };
        private int selectedIndex = 1;
        private GameObject raycastBlocker = null!;

        private void Start()
        {
            raycastBlocker = new GameObject("FastForwarder_RaycastBlocker");
            raycastBlocker.layer = LayerMask.NameToLayer("UI");
            var canvas = raycastBlocker.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 30000;

            raycastBlocker.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            var imageObj = new GameObject("BlockerImage");
            imageObj.transform.SetParent(raycastBlocker.transform, false);
            imageObj.layer = LayerMask.NameToLayer("UI");
            var image = imageObj.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0, 0, 0, 0.001f); 

            var rectTransform = image.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            
            rectTransform.sizeDelta = new Vector2(200, 40);
            rectTransform.anchoredPosition = new Vector2(-10, -120);
        }

        private void OnDestroy()
        {
            if (raycastBlocker != null)
            {
                Destroy(raycastBlocker);
            }
        }

        private void OnGUI()
        {
            if (FastForwarder.SimManager == null)
                return;

            Rect uiRect = new Rect(Screen.width - 210, 120, 200, 40);

            GUI.BeginGroup(uiRect);

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
            
            if (uiRect.Contains(Event.current.mousePosition))
            {
                var currentEvent = Event.current;
                if (currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseUp || currentEvent.type == EventType.ScrollWheel || currentEvent.type == EventType.MouseDrag)
                {
                    currentEvent.Use();
                }
            }

            GUI.backgroundColor = Color.white; 

            if (FastForwarder.CurrentGameCore == null)
            {
                FastForwarder.Log.LogInfo("[FastForwarder] Destroying UI GameObject as GameCore is null.");
                Destroy(this.gameObject);
            }
        }
    }
}