using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class M {

	/// <summary>
	/// zamienia miejscami zmienne
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a"></param>
	/// <param name="b"></param>
	static public void Swap<T>(ref T a, ref T b) {
		T temp = a;
		a = b;
		b = temp;
	}

	/// <summary>
	/// zwraca ilość elementów równych b w tablicy a
	/// </summary>
	/// <param name="b"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	static public int Equals(float b, params float[] a) {
		int output = 0;
		for(int i = 0; i<a.Length; i++)
			if(b == a[i])
				output++;

		return output;
	}
	static public int Count<T>(T[] b, System.Func<T, bool> conditioner){ 
		int result = 0;
		for(int i = 0; i<b.Length; i++){ 
			if(conditioner(b[i])){ 
				result++;
			}
		}
		return result;
	}
	static public void RunForAll<T>(this T[] a, System.Action<T> func){ 
		for(int i =0; i<a.Length; i++){ 
			func(a[i]);	
		}	
	}

	static public T[] CalcForAll<T, Y>(Y[] a, System.Func<Y, T> func){ 
		T[] result = new T[a.Length];
		for(int i =0 ; i< a.Length; i++){ 
			result[i] = func(a[i]);
		}
		return result;
	}
	
	
	

	/// <summary>
	/// Odwraca kolejność elementów w tablicy
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="arg"></param>
	/// <returns></returns>
	/// 
	static public T[] ArrayReverse<T>(T[] arg) {
		T[] output = new T[arg.Length];
		for(int i = 0; i<arg.Length; i++) {
			output[i] = arg[arg.Length - 1 - i];
		}
		return output;
	}



	/// <summary>
	/// wykonuje losowanie ważone
	/// </summary>
	/// <param name="wages"></param>
	/// <returns></returns>
	static public int WageRandom(float[] wages) {
		float sum = 0;
		for(int i = 0; i<wages.Length; i++) {
			if(wages[i] < 0)
				wages[i] = 0;
			else
				sum += wages[i];
		}
		
		if(sum == 0f)
			return -1;

		float rand = Random.Range(0, sum);
		//Debug.Log(rand);
		while(rand == sum)
			rand = Random.Range(0, sum);
		int output = -3;
		for(int i = 0; i<wages.Length; i++) {
			rand -=  wages[i];
			if(rand <= 0)
				return i;
		}
		if(output < 0 || output > wages.Length)
			throw new System.Exception("wyjście poza zasięgiem ="+output);

		return output;
	}
	/// <summary>
	/// wykonuje losowanie ważone
	/// </summary>
	/// <param name="wages"></param>
	/// <returns></returns>
	static public int WageRandom(int[] wages) {
		int sum = 0;
		for(int i = 0; i<wages.Length; i++) {
			if(wages[i] < 0)
				wages[i] = 0;
			else
				sum += wages[i];
		}
		if(sum == 0f)
			return -1;

		int rand = Random.Range(0, sum);

		while(rand == sum)
			rand = Random.Range(0, sum);

		for(int i = 0; i<wages.Length; i++) {
			rand -=  wages[i];
			if(rand < 0)
				return i;
		}

		return -2;
	}
	static public bool tryDestroy(Object obj){
		if(obj){
			MonoBehaviour.Destroy(obj);
			return true;
		}else return false;

	}
	static public bool TryDestroyFromTransform(Transform obj){
		if(obj) {
			MonoBehaviour.Destroy(obj.gameObject);
			return true;
		} else return false;
	}
    public enum ScaleMode { relative, squareByWidth, squareByHeight, none };
    static public Rect Relative(Rect original, ScaleMode scaleMode = ScaleMode.relative,float parts = 100f){ 
        switch(scaleMode){
            case ScaleMode.relative:
                return new Rect(original.x*Screen.width/parts, original.y*Screen.height/parts, original.width*Screen.width/parts, original.height*Screen.height/parts);
            break;
            case ScaleMode.squareByWidth:
                return new Rect(original.x*Screen.width/parts, original.y*Screen.width/parts, original.width*Screen.width/parts, original.height*Screen.width/parts);
            break;
            case ScaleMode.squareByHeight:
                 return new Rect(original.x*Screen.height/parts, original.y*Screen.height/parts, original.width*Screen.height/parts, original.height*Screen.height/parts);
            break;
			case ScaleMode.none:
				return original;
			break;
            default:
                throw new System.Exception("ureachable code place reached");
            break;
        }
    }
	static public void DrawBar(Texture texture, float value, Rect fullSize, ScaleMode scale = ScaleMode.relative, float parts = 100f){ 
		if(value > 1f || value < 0f){ 
			Debug.LogError("Pasek o wielkości: "+value);
		}
		
		fullSize.width *= value;	
		GUI.DrawTexture(Relative(fullSize, scale, parts), texture);
	}
	static public void ResetLevel(){ 
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);	
	}
	static public Vector3 Floor(Vector3 v){ 
		return new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));	
	}
	static public float Sign(float f){ 
		if(f==0f)
			return 0f;
		else
			return Mathf.Sign(f);
	}
	static public int SignInt(float f){ 
		if(f==0f)
			return 0;
		else
			return (int)Mathf.Sign(f);
	}

	/// <summary>
	/// działa jak modulo, ale przy a ujemnym daje dodatni wynik
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	static public float PositiveModulo(float a, float b){ 
		float result = a%b;
		return (result < 0)? (b+result) : result;
	}

	/// <summary>
	/// zwraca obiekty w stożku
	/// </summary>
	/// <param name="startpoint">centrum</param>
	/// <param name="direction">kierunek castowania stożka</param>
	/// <param name="angleRange">kątowy zasięg</param>
	/// <param name="maxRange">zasięg</param>
	/// <param name="layerMask">maska kolizji</param>
	/// <returns>tablicę colliderów w stożku</returns>
	static public Collider[] OverlapCone(Vector3 startpoint, Vector3 direction, float angleRange, float maxRange, int layerMask){ 
		Collider[] targets = Physics.OverlapSphere(startpoint, maxRange, layerMask);
		List<Collider> list = new List<Collider>();
		foreach(Collider collider in targets){ 
			if(Vector3.Angle(collider.transform.position - startpoint, direction)<=angleRange){ 
				list.Add(collider);	
			}
		}
		
		return list.ToArray();
	}

	/// <summary>
	/// Klasa generyczna kolejek, może być używana zarówno jako fifo, filo jak i jako ich mieszanka
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Que<T>{ 
		class Element{ 
			public Element previous;
			public Element next;
			public T value;
			public Element(Element previous, T value, Element next){ 
				this.previous = previous;
				this.next = next;
				this.value = value;
			}
		}	
		private Element first = null;
		private Element last = null;

		/// <summary>
		/// czy lista zawiera jakieś elementy
		/// </summary>
		public bool notEmpty { get{ return (first != null); } }

		
		/// <summary>
		/// Daje element na koniec kolejki
		/// </summary>
		/// <param name="value">wartość</param>
		public void PushBack(T value){ 	
			if(last != null){
				last.next = new Element(last, value, null);
				last = last.next;
			}else{ 
				first = last = new Element(null, value, null);
			}
		}
		
		/// <summary>
		/// Daje element na początek kolejki
		/// </summary>
		/// <param name="value">wartość</param>
		public void PushForward(T value){ 
			if(first != null){
				first.previous = new Element(null, value, first);
				first = first.previous;
			}else{ 
				last = first = new Element(null, value, null);
			}
		}

		/// <summary>
		/// Usuwa i zwraca ostatni element kolejki
		/// </summary>
		/// <returns></returns>
		public T PopBack(){ 
			if(last == null)
				throw new System.Exception("Popping empty que");

			T result = last.value;
			if(last == first){
				last = first = null;
			} else{
				last = last.previous;
				last.next = null;
			}
			return result;
		}


		/// <summary>
		/// Usuwa i zwraca pierwszy element kolejki
		/// </summary>
		/// <returns></returns>
		public T PopForward(){ 
			if(first == null)
				throw new System.Exception("Popping empty que");

			T result = first.value;
			if(last == first){ 
				last = first = null;
			}else{
				first = first.next;
				first.previous = null;
			}
			return result;
		}

		/// <summary>
		/// zwraca ostatni element bez jego usuwania
		/// </summary>
		/// <returns></returns>
		public T PeekBack(){ 
			if(last == null)
				throw new System.Exception("Peeking empty que");

			return last.value;
		}

		/// <summary>
		/// zwraca pierwszy element bez jego usuwania
		/// </summary>
		/// <returns></returns>
		public T PeekForward(){ 
			if(first == null)
				throw new System.Exception("Peeking empty que");

			return first.value;
		}

		/// <summary>
		/// zamienia kolejke na liste
		/// </summary>
		/// <returns></returns>
		public List<T> ToList(){ 
			return new List<T>(ToArray());
		}

		/// <summary>
		/// zamienia kolejke na tablice
		/// </summary>
		/// <returns></returns>
		public T[] ToArray(){ 
			T[] result = new T[Count()];
			Element current = first;
			for(int i =0; i< result.Length; i++){ 
				result[i] = first.value;
				current = first.next;
			}
			return result;
		}
			
		/// <summary>
		/// zwraca liczbę elementów w kolejce
		/// </summary>
		/// <returns></returns>
		public int Count(){ 
			return Count(first);
		}
		
		private int Count(Element start){ 
			if(start == null)
				return 0;
			else
				return 1 + Count(start.next);
		}
		
		public T this[int index]{ 
			get{ 
				Element current = first;
				int i = index;
				while(i > 0){ 
					if(	current == null){ 
						throw new System.Exception("Index out of range " +index);
					}
					current = first.next;
					i--;
				}
				return current.value;
			}
		}

		public override string ToString() {
			string result = "que of "+typeof(T).ToString()+" : ";
			Element current = first;
			if(!notEmpty)
				return "empty que of "+typeof(T).ToString();
			while(current != null){ 
				result += current.value.ToString()+" ";
				current = current.next;
			}
			return result;
		}
	}

	public static List<Transform> FindAllChildren(Transform parent){ 
		if(!parent)
			return null;

		Transform[] t_temp = parent.GetComponentsInChildren<Transform>();
		List<Transform> result = new List<Transform>();
		foreach(Transform i in t_temp){ 
			result.Add(i);
			foreach(Transform j in FindAllChildren(i))
				result.Add(j);
		}
		return result;
	}
	
	/// <summary>
    /// Sortuje tablice
    /// </summary>
    /// <typeparam name="T">Typ zmiennych w tablicy</typeparam>
    /// <param name="arr">Tablica do posortowania</param>
    /// <param name="indexer">funkcja ustalająca kolejność elementów w tablicy (kolejność rosnąca)</param>
    static public void Sort<T>(ref T[] arr, System.Func<T, float> indexer)
    {
        float[] indexes = new float[arr.Length];
        for(int  i = 0; i< arr.Length; i++)
        {
            indexes[i] = indexer(arr[i]);
        }
        for (int i = 0; i < arr.Length; i++)
        {
            bool changed = false;

            for (int j = 1; j < arr.Length; j++)
            {

                if (indexes[j - 1] > indexes[j])
                {
                    changed = true;
                    float f_temp = indexes[j - 1];
                    indexes[j - 1] = indexes[j];
                    indexes[j] = f_temp;
                    T t_temp = arr[j - 1];
                    arr[j - 1] = arr[j];
                    arr[j] = t_temp;
                }
            }
            if (!changed)
                return;
        }

    }

} 
