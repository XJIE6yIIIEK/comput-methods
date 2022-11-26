using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace ApproximationTheory {
	class Function {
		int a, b, n;

		public Function(int a, int b, int n) {
			this.a = a;
			this.b = b;
			this.n = n;
		}

		public double FunctionValue(double x) {
			return Math.Pow(3, x) + 5 * x - 2;
		}

		public double DerivFuntionValue(double x) {
			return Math.Pow(3, x) * Math.Log(3) + 5;
		}

		public double FourthDerivFunctionValue(double x) {
			return Math.Pow(3, x) * Math.Pow(Math.Log(3), 4);
		}
		public double FifthDerivFunctionValue(double x) {
			return Math.Pow(3, x) * Math.Pow(Math.Log(3), 5); 
		}
		public Vector GetVectorX() {
			Vector v = new Vector(n + 1);
			double h = 1.0 * (b - a) / n;
			for(int i = 0; i <= n; i++) {
				v[i] = a + i * h;
			}
			return v;
		}
		public Vector GetVectorF(Vector X) {
			Vector v = new Vector(X.Length);
			for(int i = 0; i < X.Length; i++) {
				v[i] = FunctionValue(X[i]);
			}
			return v;
		}

		public Vector GetVectorDerivF(Vector X) {
			Vector v = new Vector(X.Length);
			for(int i = 0; i < X.Length; i++) {
				v[i] = DerivFuntionValue(X[i]);
			}
			return v;
		}
	}

	interface IFunction {
		double Solve(double x);
		double d1x(double x);
	}

	class F_30 : IFunction {
		public F_30() { }

		public double Solve(double x) {
			return Math.Pow(3, x) + 5 * x - 2;
		}

		public double d1x(double x) {
			return Math.Pow(3, x) * Math.Log(3) + 5;
		}
	}

	class F_2 : IFunction {
		public F_2() { }

		public double Solve(double x) {
			return Math.Exp(x) + 5 * x - 3;
		}

		public double d1x(double x) {
			return Math.Exp(x) + 5;
		}
	}

	class F_28 : IFunction {
		public F_28() { }

		public double Solve(double x) {
			return Math.Pow(3, x) - 2 * x + 5;
		}

		public double d1x(double x) {
			return Math.Pow(3, x) * Math.Log(3) - 2;
		}
	}
}
