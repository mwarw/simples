using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RPN
{
	public class Operation{
        public byte arguments;
        public System.Func<float[], float> operation;
		public Operation(byte arguments, System.Func<float[], float> operation){ 
			this.arguments = arguments;
			this. operation = operation;
		}
		public static Operation ConstValue(float value){ 
			return new Operation(0, (float[] arg)=>{return value; });	
		}
    }

	private class Stack
	{
		public float value;
		public Stack next;
		public Stack(float value, Stack next)
		{
			this.value = value;
			this.next = next;
		}
	}

	public static float Calculate(Operation[] formula){ 
			
		Stack stack = null;
		for(int i =0; i<formula.Length; i++){
			float[] values = new float[(int)formula[i].arguments];
			for(int j = formula[i].arguments - 1; j>=0; j--){
				values[j] = stack.value;
				stack = stack.next;
			}
			stack = new Stack(formula[i].operation(values), stack);

		}
		return stack.value;
	}

	static Dictionary<float, Operation> constantList = new Dictionary<float, Operation>();
	public static Operation[] StringRPNtoFormula(string formula, params Dictionary<string, Operation>[] dicts){ 
		string[] formula_arr = formula.Split(' ');
		Operation[] result = new Operation[formula_arr.Length];

		

		for(int i = 0; i<result.Length; i++){ 
			
			if(float.TryParse(formula_arr[i], out float f)){ 
				if(!constantList.ContainsKey(f)){
					constantList.Add(f, Operation.ConstValue(f));
				}
				result[i] = constantList[f];
				result[i] = Operation.ConstValue(f);
			}else for(int j =0; j<dicts.Length; j++){ 
				if(dicts[j].ContainsKey(formula_arr[i])){ 
					result[i] = dicts[j][formula_arr[i]];
					break;
				}
			}
		}
		return result;		
	}

	public static Dictionary<string, Operation> ClassicMathDictionary = new Dictionary<string, Operation> { 
		{"+", new Operation(2, (float[] arg)=>{return arg[0] + arg[1];}) },
		{"-", new Operation(2, (float[] arg)=>{return arg[0] - arg[1];}) },
		{"*", new Operation(2, (float[] arg)=>{return arg[0] * arg[1];}) },
		{"/", new Operation(2, (float[] arg)=>{return arg[0] / arg[1];}) },
		{"^", new Operation(2, (float[] arg)=>{return Mathf.Pow(arg[0], arg[1]);}) },
		{"sqrt", new Operation(1, (float[] arg)=>{return Mathf.Sqrt(arg[0]);}) },
		{"floor", new Operation(1, (float[] arg)=>{return Mathf.Floor(arg[0]);}) },
		{"max", new Operation(2, (float[] arg)=>{return Mathf.Max(arg[0], arg[1]); }) },
		{"min", new Operation(2, (float[] arg)=>{return Mathf.Min(arg[0], arg[1]); }) }
	};



}
