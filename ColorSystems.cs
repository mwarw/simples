using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorSystems {

	
	/// <summary>
	/// Konwertuje kolor z HSV do RGB
	/// </summary>
	/// <param name="color">Kolor w skali HSV</param>
	/// <returns> Kolor w skali RGB</returns>
	static public Color HSVtoRGB(HSV color) {
		float h = color.h;
		float s = color.s;
		float v = color.v;
		Color wynik = new Color(1, 1, 1);
		h = h%360f;
		switch(Mathf.FloorToInt(h/60)) {
			case 0:
				wynik.r = 1;
				wynik.g = (h%60f)/60f;
				wynik.b = 0;
				break;
			case 1:
				wynik.r = 1 - (h%60f)/60f;
				wynik.g = 1;
				wynik.b = 0;
				break;
			case 2:
				wynik.r = 0;
				wynik.g = 1;
				wynik.b = (h%60f)/60f;
				break;
			case 3:
				wynik.r = 0;
				wynik.g = 1 - (h%60f)/60f;
				wynik.b = 1;
				break;
			case 4:
				wynik.r = (h%60f)/60f;
				wynik.g = 0f;
				wynik.b = 1;
				break;
			case 5:
				wynik.r = 1;
				wynik.g = 0;
				wynik.b = 1 - (h%60f)/60f;
				break;
		}

		wynik.r = 1-(1-wynik.r)*s;
		wynik.g = 1-(1-wynik.g)*s;
		wynik.b = 1-(1-wynik.b)*s;

		wynik.r *= v;
		wynik.g *= v;
		wynik.b *= v;
		return wynik;
	}

	/// <summary>
	/// Konwertuje kolor z RGB do HSV
	/// </summary>
	/// <param name="color">Kolor w skali RGB</param>
	/// <returns>Kolor w skali HSV</returns>
	static public HSV RGBtoHSV( Color color){ 
		HSV result = new HSV(0,0,0);
		result.v = Mathf.Max(color.r, color.g, color.b);
		if(result.v == 0f){ 
			return result;	
		}
		color = color/result.v;

		result.s = Mathf.Max(color.r, color.g, color.b) - Mathf.Min(color.r, color.g, color.b);
		if(result.s == 0f){ 
			return result;	
		}

		float mid = color.r  + color.g+color.b - Mathf.Max(color.r, color.g, color.b) - Mathf.Min(color.r, color.g, color.b);
		bool[] parts = { true, true, true, true, true, true };

		if(color.r == 1f)
			parts[1] = parts[2] = parts[3] = parts[4] = false;
		if(color.r == 0f)
			parts[1] = parts[4] = parts[0] = parts[5] = false;
		if(color.g==1f)
			parts[0] = parts[5] = parts[3] = parts[4] = false;
		if(color.g==0f)
			parts[1] = parts[2] = parts[3] = parts[4] = false;
		if(color.b==1f)
			parts[1] = parts[2] = parts[0] = parts[5] = false;
		if(color.b==0f)
			parts[5] = parts[2] = parts[3] = parts[4] = false;

		int t = 0;
		while(!parts[t] && t< 6)
			t++;

		result.h = (t*60)%360;
		if(t%2 == 0)
			result.h += mid*60;
		else
			result.h += 60*(1 - mid);

		return result;

	}
}

[System.Serializable]
public struct HSV{ 
	[Range(0,360)] public float h;
	[Range(0,1)] public float s;
	[Range(0,1)] public float v;

	public HSV(float H, float S, float V){ 
		h = H % 360;
		s = Mathf.Max(0, Mathf.Min(1, S));
		v = Mathf.Max(0, Mathf.Min(1, V));
	}
}