using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveTextEffect : MonoBehaviour
{
	public float amplitude = 0.1f; // Altura da onda
	public float frequency = 0.1f; // Frequ�ncia da onda
	public float speed = 2f; // Velocidade da onda

	private TMP_Text textMeshPro;
	private Mesh textMesh;
	private Vector3[] vertices;

	void Start()
	{
		// Obt�m o componente TextMeshPro
		textMeshPro = GetComponent<TMP_Text>();
		if (textMeshPro == null)
		{
			Debug.LogError("Por favor, adicione um componente TextMeshPro ao GameObject.");
			return;
		}

		// Ativa o modo de malha din�mica
		textMeshPro.ForceMeshUpdate();
	}

	void Update()
	{
		if (textMeshPro == null) return;

		// Atualiza a malha
		textMesh = textMeshPro.mesh;
		vertices = textMesh.vertices;

		// Itera sobre cada caractere no texto
		for (int i = 0; i < textMeshPro.textInfo.characterCount; i++)
		{
			var charInfo = textMeshPro.textInfo.characterInfo[i];

			// Ignora caracteres invis�veis
			if (!charInfo.isVisible)
				continue;

			// Calcula os �ndices dos v�rtices do caractere atual
			int vertexIndex = charInfo.vertexIndex;

			// Aplica o efeito de onda em cada v�rtice
			for (int j = 0; j < 4; j++) // Cada caractere possui 4 v�rtices
			{
				Vector3 originalPosition = textMesh.vertices[vertexIndex + j];
				float wave = Mathf.Sin(Time.unscaledTime * speed + originalPosition.x * frequency) * amplitude;
				vertices[vertexIndex + j] = originalPosition + new Vector3(0, wave, 0);
			}
		}

		// Atualiza a malha com os v�rtices modificados
		textMesh.vertices = vertices;
		textMeshPro.canvasRenderer.SetMesh(textMesh);
	}
}