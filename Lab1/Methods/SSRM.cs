using Matrices;
using Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Methods {
	internal class SSRM {
		private Vector X;
		private L G;
		private Matrix D;

		public Vector Answer {
			get { return X; }
		}

		public L Matrix_G {
			get { return G; }
		}

		public Matrix Matrix_D {
			get { return D; }
		}

		public SSRM(Matrix A, Vector B) {
			G = new L(A.Rows, A.Cols);
			D = new Matrix(A.Rows, A.Cols);

			G[0, 0] = Math.Sqrt(A[0, 0]);
			//D[0, 0] = Math.Sign(A[0, 0]);

			for(int i = 1; i < A.Rows; i++) {
				G[i, 0] = A[0, i] / A[0, 0];
			}

			for(int i = 1; i < A.Rows; i++) {
				for(int j = 1; j <= i; j++) {
					if(i != j) {
						double sum = 0;

						for(int k = 0; k < j; k++) {
							sum += G[i, k] * G[j, k] /** D[k, k]*/;
						}

						G[i, j] = (A[i, j] - sum) / G[j, j];
					} else {
						double sum = 0;

						for(int k = 0; k < j; k++) {
							sum += G[i, k] * G[i, k] /** D[k, k]*/;
						}

						double underRoot = A[i, j] - sum;
						//D[i, i] = Math.Sign(underRoot);

						G[i, j] = Math.Sqrt(underRoot);
					}
				}
			}

			Vector Y = new Vector(B.Length);

			Y[0] = B[0] / G[0, 0];

			for(int i = 1; i < Y.Length; i++) {
				double sum = 0;

				for(int k = 0; k < i; k++) {
					sum += G[i, k] * Y[k];
				}

				Y[i] = (B[i] - sum) / G[i, i];
			}

			X = new Vector(B.Length);

			X[X.Length - 1] = Y[Y.Length - 1] / G[G.Rows - 1, G.Cols - 1];

			for(int i = X.Length - 2; i >= 0; i--) {
				double sum = 0;

				for(int k = i + 1; k < X.Length; k++) {
					sum += G[k, i] * X[k];
				}

				X[i] = (Y[i] - sum) / G[i, i];
			}

			//X = G.Inverse * G.Transposition().Inverse * B;
		}
	}
}
