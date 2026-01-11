using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codigo.Scripts
{
    public class ScrollUpdater : MonoBehaviour, ISelectHandler
    {
        public ScrollRect scrollRect;
        public int padding = 5;
        
        IEnumerator ScrollUpdate()
        {
            yield return null;
            var scroll = GetComponentInParent<ScrollRect>();
            var target = gameObject.GetComponent<RectTransform>();
            var limiteSup = -scroll.viewport.rect.height;
            var limiteInf = 0;
            var current = target.localPosition.y + scroll.content.localPosition.y;
            if (scroll && !(current > limiteSup && current < limiteInf))
            {
                var vector3 = scroll.content.localPosition;

                if (current > limiteSup)
                    vector3.y += limiteInf - current - target.rect.height/2 - padding;

                if (current < limiteInf)
                    vector3.y += limiteSup - current + target.rect.height/2 + padding;
                
                scroll.content.localPosition = vector3;
            }
            yield break;
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            StopCoroutine(ScrollUpdate());
            StartCoroutine(ScrollUpdate());
        }
        
    }
}