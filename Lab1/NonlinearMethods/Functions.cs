using System;
using Matrices;
using Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonlinearMethods {
	//Делегат для методов классов, наследуемых от интерфейса IFunctions
	delegate double FunctionDelegate(Vector X);

	/// <summary>
	/// Интерфей для функций
	/// </summary>
    interface IFunctions {
		/// <summary>
		/// Частная проихводная функции по X
		/// </summary>
		/// <param name="X">Вектор значений</param>
		/// <returns>Значение производной</returns>
        double EvaluateDerivativeX(Vector X);

		/// <summary>
		/// Частная проихводная функции по Y
		/// </summary>
		/// <param name="X">Вектор значений</param>
		/// <returns>Значение производной</returns>
		double EvaluateDerivativeY(Vector X);

		/// <summary>
		/// Значение функции
		/// </summary>
		/// <param name="X">Вектор значений</param>
		/// <returns>Значение функции</returns>
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

	class F : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 2 * (Math.Cos(X[1] - 1) + X[0] - 0.8) + 2 * Math.Sin(X[0]) * (X[1] - Math.Cos(X[0]) - 2);
		}

		public double EvaluateDerivativeY(Vector X) {
			return 2 * (-Math.Sin(X[1] - 1)) * (Math.Cos(X[1] - 1) + X[0] - 0.8) + 2 * (X[1] - Math.Cos(X[0]) - 2);
		}

		public double Evaluate(Vector X) {
			return Math.Pow(X[1] - Math.Cos(X[0]) - 2, 2) + Math.Pow(Math.Cos(X[1] - 1) + X[0] - 0.8, 2);
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

	class _Fi2_2 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return Math.Sin(X[0] - 1);
		}

		public double EvaluateDerivativeY(Vector X) {
			return 0;
		}

		public double Evaluate(Vector X) {
            return 0.5 - Math.Cos(X[0] - 1);
		}
	}

	class _Fi1_2 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 0;
		}

		public double EvaluateDerivativeY(Vector X) {
			return -Math.Sin(X[1]);
		}

		public double Evaluate(Vector X) {
			return 3 + Math.Cos(X[1]);
		}
	}

	class _F1_2 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
            return -Math.Sin(X[0] - 1);
		}

		public double EvaluateDerivativeY(Vector X) {
			return 1;
		}

		public double Evaluate(Vector X) {
            return X[1] + Math.Cos(X[0] - 1) - 0.5;
		}
	}

	class _F2_2 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 1;
		}

		public double EvaluateDerivativeY(Vector X) {
			return Math.Sin(X[1]);
		}

		public double Evaluate(Vector X) {
			return X[0] - Math.Cos(X[1]) - 3;
		}
	}

	class _F_2 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
            return 2 * (X[1] + Math.Cos(X[0] - 1) - 0.5) * (-Math.Sin(X[0] - 1)) + 2 * (X[0] - Math.Cos(X[1]) - 3);
		}

		public double EvaluateDerivativeY(Vector X) {
            return 2 * (X[1] + Math.Cos(X[0] - 1) - 0.5) + 2 * (X[0] - Math.Cos(X[1]) - 3) * Math.Sin(X[1]);
		}

		public double Evaluate(Vector X) {
			return Math.Pow(X[1] + Math.Cos(X[0] - 1) - 0.5, 2) + Math.Pow(X[0] - Math.Cos(X[1]) - 3, 2);
		}
	}

	class _Fi1_28 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 0;
		}

		public double EvaluateDerivativeY(Vector X) {
			return Math.Sin(X[1] + 0.5);
		}

		public double Evaluate(Vector X) {
			return 0.8 - Math.Cos(X[1] + 0.5);
		}
	}

	class _Fi2_28 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return -Math.Cos(X[0]) / 2;
		}

		public double EvaluateDerivativeY(Vector X) {
			return 0;
		}

		public double Evaluate(Vector X) {
			return 0.8 - Math.Sin(X[0]) / 2;
		}
	}

	class _F1_28 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 1;
		}

		public double EvaluateDerivativeY(Vector X) {
			return -Math.Sin(X[1] + 0.5);
		}

		public double Evaluate(Vector X) {
			return Math.Cos(X[1] + 0.5) + X[0] - 0.8;
		}
	}

	class _F2_28 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return Math.Cos(X[0]);
		}

		public double EvaluateDerivativeY(Vector X) {
			return 2;
		}

		public double Evaluate(Vector X) {
			return Math.Sin(X[0]) + 2 * X[1] - 1.6;
		}
	}

	class _F_28 : IFunctions {
		public double EvaluateDerivativeX(Vector X) {
			return 2 * (Math.Cos(X[1] + 0.5) + X[0] - 0.8) + 2 * (Math.Sin(X[0]) + 2 * X[1] - 1.6) * (Math.Cos(X[0]));
		}

		public double EvaluateDerivativeY(Vector X) {
			return 2 * (Math.Cos(X[1] + 0.5) + X[0] - 0.8) * (-Math.Sin(X[1] + 0.5)) + 4 * (Math.Sin(X[0]) + 2 * X[1] - 1.6);
		}

		public double Evaluate(Vector X) {
			return Math.Pow(Math.Cos(X[1] + 0.5) + X[0] - 0.8, 2) + Math.Pow(Math.Sin(X[0]) + 2 * X[1] - 1.6, 2);
		}
	}
}