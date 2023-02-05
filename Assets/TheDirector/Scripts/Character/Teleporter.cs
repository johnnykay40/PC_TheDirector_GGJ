using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Teleporter : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Teleporter> destinations;
    private PlayerTransportation playerTransportation;

    private void Awake()
    {
        playerTransportation = FindObjectOfType<PlayerTransportation>();
    }

    public bool IsDestination(Teleporter teleporter) =>
        destinations.Contains(teleporter);

    public void OnPointerClick(PointerEventData eventData)
    {
        playerTransportation.MoveTo(this);
    }
}
