using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//!!!!!! ZAPISYWA ZMIANY W FOLDERZE NA PULPICIE 
//v: 1.3
public static class M2D {
	static public Vector2 AngleToVector(float angle) {
		return new Vector2(Mathf.Cos(angle*2F*Mathf.PI/360F), Mathf.Sin(angle*2F*Mathf.PI/360F));
	}
	/// <summary>
	/// Wykonuje 2 wymiarowy hitscan w oparciu o kąt
	/// </summary>
	/// <param name="start">Transform do wykonywania testu</param>
	/// <param name="angle">kąt</param>
	/// <param name="mask">maska kolizji dla testu</param>
	/// <param name="range">zasięg</param>
	/// <returns></returns>
	static public RaycastHit2D AngleScan2D(Transform start, float angle, int mask, float range) {
		RaycastHit2D wynik = Physics2D.Raycast((Vector2)start.position, (Vector2)start.TransformDirection((Vector3) AngleToVector(angle)), range, mask);

		if(wynik.collider == null) {
			wynik.point = start.position + start.TransformDirection((Vector3) AngleToVector(angle))* range;
		}
		return wynik;
	}

	/// <summary>
	/// Obraca obiekt w kierunku celu osią y lub kierunkiem wskazanym przez direction
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="Target"></param>
	static public void LookAt2D(this Transform transform, Vector2 Target, float extra = 0f) {
		transform.LookAt(transform.position + new Vector3(0, 0, 1), (Vector3)Target) ;
		Quaternion Q_temp = Quaternion.LookRotation(new Vector3(0, 0, 1), transform.position - (Vector3) Target);
		transform.rotation = Q_temp;
		transform.Rotate(new Vector3(0, 0, extra));

	}

} 
