using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace ApproximationTheory {
	class DividedDifferences {
		Vector X;
		public DividedDifferences(Vector X) {
			this.X = X;
		}
		public List<Vector> GetDiffsList(Vector F) {
			List<Vector> listDiffs = new List<Vector>();
			listDiffs.Add(F);
			int length = F.Length - 1;
			for(int i = 0; i < length; i++) {
				F = GetDiffsVector(F);
				listDiffs.Add(F);
			}
			return listDiffs;
		}

		private Vector GetDiffsVector(Vector F) {
			int step = X.Length - F.Length + 1;
			Vector curDiffs = new Vector(F.Length - 1);
			for(int i = 0; i < curDiffs.Length; i++) {
				curDiffs[i] = (F[i + 1] - F[i]) / (X[i + step] - X[i]);
			}
			return curDiffs;
		}

	}
}
