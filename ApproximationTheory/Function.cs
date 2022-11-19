﻿using System;
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
	}
}
