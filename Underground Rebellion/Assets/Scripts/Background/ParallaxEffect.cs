using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private GameObject playerObject;
    private Vector3 playerInitialPosition;
    private Vector3 roomPosAtual;


    [SerializeField]
    private float parallaxEffectMultiplier = 0.2f;

    [SerializeField]
    private float parallaxEffectChangingRoomLevel = 0.2f;
    private void OnEnable()
    {
        LevelEvent.OnPlayerEnter += UpdatePlayerPosition;
    }

    private void OnDisable()
    {
        LevelEvent.OnPlayerEnter -= UpdatePlayerPosition;
    }
    

    private void UpdatePlayerPosition(GameObject player, Vector3 roomPosition)
    {
        playerInitialPosition = player.transform.position;
        if (playerObject == null)
        {
            playerObject = player;
        }
        if (roomPosAtual.y < roomPosition.y)
        {
            float newoffset = this.gameObject.transform.localPosition.y - parallaxEffectChangingRoomLevel;
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, newoffset, 10);
        }
        else if (roomPosAtual.y > roomPosition.y)
        {
            float newoffset = this.gameObject.transform.localPosition.y + parallaxEffectChangingRoomLevel;
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, newoffset, 10);
        }

        roomPosAtual = roomPosition;
    }

    private void Update()
    {
        if (playerObject != null)
        {
            // Calcula a diferença de posição do jogador em relação ao ponto inicial
            Vector3 playerOffset = playerObject.transform.position - playerInitialPosition;

            // Aplica o efeito de parallax, mas com uma suavização controlada.
            // A multiplicação pelo "parallaxEffectMultiplier" vai controlar a intensidade do movimento.
            Vector3 parallaxPosition = roomPosAtual - playerOffset * parallaxEffectMultiplier;

            // Movimento suave do background usando Lerp
            
            this.gameObject.transform.position = new Vector3(parallaxPosition.x, this.gameObject.transform.position.y, 10);

        }
    }

}
