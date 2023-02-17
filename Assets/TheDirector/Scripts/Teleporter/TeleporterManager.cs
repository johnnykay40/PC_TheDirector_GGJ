using UnityEngine;
using System.Collections.Generic;

public class TeleporterManager : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private Sprite spriteTeleporter;

    [Header("List of Teleporter")]
    [SerializeField] private List<SpriteRenderer> listSpriteTeleporter;

    private void Awake() => AddSpriteToTeleporters();

    private void AddSpriteToTeleporters()
    {
        foreach (var item in listSpriteTeleporter)
        {
            item.sprite = spriteTeleporter;
        }
    }
}
