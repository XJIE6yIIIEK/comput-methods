using System;

namespace ApproximationTheory {
	static class Basis {
		public static double g(int c, double X) {
			switch(c) {
				case 1:
					return 1;
				case 2:
					return X;
				case 3:
					return X * X;
				default:
					return 0.0;
			}
		}
	}
}
