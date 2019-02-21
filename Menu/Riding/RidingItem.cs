using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingItem
{

	public string Name;
	public string ImageSrc;
	public float Price;
	public float RisingPricePercent;
	public float RisingTradePercent;

	public RidingItem(string name, string imageSrc, float price, float risingPricePercent,float startGoldPerSec, float risingGoldPerSec)
	{
		Name = name;
		ImageSrc = imageSrc;
		Price = price;
		RisingPricePercent = risingPricePercent;
	}
	
}
