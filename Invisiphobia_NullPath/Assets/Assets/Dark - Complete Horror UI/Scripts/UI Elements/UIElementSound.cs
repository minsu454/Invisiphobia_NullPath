using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.Dark
{
    public class UIElementSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        public AudioClip hoverSound;
        public AudioClip clickSound;

        // Settings
        public bool enableHoverSound = true;
        public bool enableClickSound = true;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableHoverSound == true)
                Managers.Sound.SFX2DPlay(hoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (enableClickSound == true)
                Managers.Sound.SFX2DPlay(clickSound);
        }
    }
}