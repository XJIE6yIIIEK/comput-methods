using Lab1;
using Matrices;
using Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace ApproximationTheory {
	class DRMSA {
		Vector F, X;
		IOModule io;
		public DRMSA(int a, int b, int n, IOModule io) {
			this.io = io;
			Function func = new Function(a, b, n);
			this.X = func.GetVectorX();
			this.F = func.GetVectorF(X);
		}
		public Vector getPolConsts() {
			Matrix A = new Matrix(3, 3);
			Vector B = new Vector(3);
			
			for(int i = 0; i < X.Length; i++) {
				for(int j = 0; j < 3; j++) {
					for(int k = j; k < 3; k++) {
						A[j, k] += Basis.g(j + 1, X[i]) * Basis.g(k + 1, X[i]);
						if (k != j)
							A[k, j] += Basis.g(k + 1, X[i]) * Basis.g(j + 1, X[i]);
					}
				}

				for(int j = 0; j < 3; j++) {
					B[j] += F[i] * Basis.g(j + 1, X[i]);
				}
			}

			LU lu = new LU(A, B, 3, io);
			return lu.X; 
		}
	}
}
