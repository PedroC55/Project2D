using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEnergy : EnemyCondition
{
    [SerializeField]
    private float maxEnergy;
    private float currentEnergy;

    [SerializeField]
    private float energyRechargeTime = 5f;
	private float currentRechargeTime = 0f;

	private bool hasEnergy = true;

	#region Energy Bar Components
	[Header("Energy Bar Components")]
	[SerializeField]
	private Canvas canvas;
	private Vector3 canvasScale;
	[SerializeField]
	private Slider energySlider;
	[SerializeField]
	private Image fillSliderImage;

	private Color normalColor;
	private Color rechargeColor;

	#endregion

	private void Start()
	{
		canvas.enabled = true;

		currentEnergy = maxEnergy;
		
		energySlider.maxValue = maxEnergy;
		energySlider.value = currentEnergy;

		canvasScale = canvas.transform.localScale;

		normalColor = fillSliderImage.color;
		rechargeColor = new Color(normalColor.r, normalColor.g, normalColor.b, 0.5f);
	}

	private void Update()
	{
		if (enemyAI.IsDead())
			return;

		UpdateCanvas();
		
		//Fazer animação bonita de decrease e increse do slide bar de forma devagar
		if (!hasEnergy)
        {
			if (currentRechargeTime < energyRechargeTime)
			{
				float lerpvalue = currentRechargeTime / energyRechargeTime;
				currentEnergy = Mathf.Lerp(0, maxEnergy, lerpvalue);
				currentRechargeTime += Time.deltaTime;
			}

			if (currentRechargeTime >= energyRechargeTime)
			{
				ResetEnergy();
			}
		}
	}

	public void DecreaseEnergy(int amount)
    {
        currentEnergy -= amount;

		if(currentEnergy <= 0)
		{
			currentEnergy = 0;
			hasEnergy = false;
			fillSliderImage.color = rechargeColor;
			enemyAI.EnemyTired();
		}
    }

    public bool HasEnergy()
    {
		return hasEnergy;
	}

	public void ResetEnergy()
	{
		if (enemyAI.IsDead())
			return;

		canvas.enabled = true;
		currentEnergy = maxEnergy;
		currentRechargeTime = 0f;
		hasEnergy = true;
		enemyAI.EnemyRecovered();

		fillSliderImage.color = normalColor;
	}

	public void HideEnergy()
	{
		canvas.enabled = false;
	}

	private void UpdateCanvas()
	{
		var scale = canvasScale;
		if (transform.parent.localScale.x < 0)
		{
			scale.x = -canvasScale.x;
		}

		canvas.transform.localScale = scale;

		energySlider.value = currentEnergy;
	}
}