using System;
using Matrices;
using Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonlinearMethods {
    delegate double FunctionDelegate(Vector X);

    interface IFunctions {
        double EvaluateDerivativeX(Vector X);

        double EvaluateDerivativeY(Vector X);

        double Evaluate(Vector X);
    }

    class F1 : IFunctions {
        public double EvaluateDerivativeX(Vector X) {
            return 1.0;
        }

        public double EvaluateDerivativeY(Vector X) {
            return -Math.Sin(X[1] - 1);
        }

        public double Evaluate(Vector X) {
            return Math.Cos(X[1] - 1) + X[0] - 0.8;
        }
    }

    class F2 : IFunctions {
        public double EvaluateDerivativeX(Vector X) {
            return Math.Sin(X[0]);
        }

        public double EvaluateDerivativeY(Vector X) {
            return 1.0;
        }

        public double Evaluate(Vector X) {
            return X[1] - Math.Cos(X[0]) - 2;
        }
    }

    class Fi1 : IFunctions {
        public double EvaluateDerivativeX(Vector X) {
            return 0.0;
        }

        public double EvaluateDerivativeY(Vector X) {
            return Math.Sin(X[1] - 1);
        }

        public double Evaluate(Vector X) {
            return 0.8 - Math.Cos(X[1] - 1);
        }
    }

    class Fi2 : IFunctions {
        public double EvaluateDerivativeX(Vector X) {
            return -Math.Sin(X[0]);
        }

        public double EvaluateDerivativeY(Vector X) {
            return 0.0;
        }

        public double Evaluate(Vector X) {
            return 2 + Math.Cos(X[0]);
        }
    }

	class _Fi1 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return Math.Cos(X[0] + 1);
		}

		public double EvaluateDerivativeY(Vector X) {
			return 0;
		}

		public double Evaluate(Vector X) {
            return Math.Sin(X[0] + 1) - 2.2;
		}
	}

	class _Fi2 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 0;
		}

		public double EvaluateDerivativeY(Vector X) {
			return Math.Sin(X[1]) / 2;
		}

		public double Evaluate(Vector X) {
			return 1 - Math.Cos(X[1]) / 2;
		}
	}

	class _F1 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
            return Math.Cos(X[0] + 1);
		}

		public double EvaluateDerivativeY(Vector X) {
			return -1;
		}

		public double Evaluate(Vector X) {
            return Math.Sin(X[0] + 1) - X[1] - 2.2;
		}
	}

	class _F2 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 2;
		}

		public double EvaluateDerivativeY(Vector X) {
			return -Math.Sin(X[1]);
		}

		public double Evaluate(Vector X) {
			return 2 * X[0] + Math.Cos(X[1]) - 2;
		}
	}

	class _F : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
            return 4 * (2 * X[0] + Math.Cos(X[1]) - 2) + 2 * Math.Cos(X[0] + 1) * (Math.Sin(X[0] + 1) - X[1] - 2.2);
		}

		public double EvaluateDerivativeY(Vector X) {
            return -2 * (Math.Sin(X[0] + 1) - X[1] - 2.2) - 2 * Math.Sin(X[1]) * (2 * X[0] + Math.Cos(X[1]) - 2.0);
		}

		public double Evaluate(Vector X) {
			return Math.Pow(2 * X[0] - Math.Cos(X[1]) - 2, 2) + Math.Pow(Math.Sin(X[0] + 1) - X[1] - 2.2, 2);
		}
	}
}