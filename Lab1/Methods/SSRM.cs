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
		private Matrix G;
		private Matrix D;

		public Vector Answer {
			get { return X; }
		}

		public Matrix Matrix_G {
			get { return G; }
		}

		public Matrix Matrix_D {
			get { return D; }
		}

		public SSRM(Matrix A, Vector B) {
			G = new Matrix(A.Rows, A.Cols);
			D = new Matrix(A.Rows, A.Cols);

			for(int i = 0; i < G.Rows; i++) {
				for(int j = i; j < G.Cols; j++) {
					double sum = 0;

					if(i == j) {
						for(int k = 0; k <= i - 1; k++) {
							sum += G[k, i] * G[k, i] * D[k, k];
						}

						double underroot = A[i, i] - sum;
						D[i, i] = Math.Sign(underroot);

						G[i, j] = Math.Sqrt(Math.Abs(underroot));
					} else {
						for(int k = 0; k <= i - 1; k++) {
							sum += G[k, i] * G[k, j] * D[k, k];
						}

						G[i, j] = (A[i, j] - sum) / (G[i, i] * D[i, i]);
					}
				}
			}

			X = G.Inverse * D.Inverse * G.Transposition().Inverse * B;
		}
	}
}
