using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace Matrices {
	class SweepMethod {
		Vector answer;
		public Vector Answer {
			get { return answer; }
		}
		public SweepMethod(Matrix A, Vector B) {
			double y = A[0, 0];
			Vector a = new Vector(B.Length);
			Vector b = new Vector(B.Length);
			int N = B.Length;
			a[0] = -A[0, 1] / y;
			b[0] = B[0] / y;
			for(int i = 1; i < N - 1; i++) {
				y = A[i, i] + A[i, i - 1] * a[i - 1];
				a[i] = -A[i, i + 1] / y;
				b[i] = (B[i] - A[i, i - 1] * b[i - 1]) / y;
			}

			answer = new Vector(N);

			answer[N-1] = (B[N - 1] - A[N - 1, N - 2] * b[N - 2]) / 
				(A[N - 1, N - 1] + A[N - 1, N - 2] * a[N - 2]);
			for(int i = N - 2; i >= 0; i--) {
				answer[i] = a[i] * answer[i + 1] + b[i];
			}
		}
	}
}
