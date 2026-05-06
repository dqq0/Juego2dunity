using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Configuración de Escala")]
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 clickScale = new Vector3(0.95f, 0.95f, 0.95f);
    public float animationSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private Coroutine scaleCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale;
        targetScale.x *= hoverScale.x;
        targetScale.y *= hoverScale.y;
        targetScale.z *= hoverScale.z;

        AnimateScale(targetScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        AnimateScale(targetScale);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = originalScale;
        targetScale.x *= clickScale.x;
        targetScale.y *= clickScale.y;
        targetScale.z *= clickScale.z;

        AnimateScale(targetScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Al soltar, verificamos si el cursor sigue encima del elemento
        if (eventData.pointerEnter == gameObject)
        {
            targetScale = originalScale;
            targetScale.x *= hoverScale.x;
            targetScale.y *= hoverScale.y;
            targetScale.z *= hoverScale.z;
        }
        else
        {
            targetScale = originalScale;
        }

        AnimateScale(targetScale);
    }

    private void AnimateScale(Vector3 target)
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleRoutine(target));
    }

    private IEnumerator ScaleRoutine(Vector3 target)
    {
        while (Vector3.Distance(transform.localScale, target) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, Time.unscaledDeltaTime * animationSpeed);
            yield return null;
        }
        transform.localScale = target;
    }
}
