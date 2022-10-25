using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;
using Matrices;

using FunctionVector = System.Collections.Generic.List<NonlinearMethods.FunctionDelegate>;
using FunctionMatrix = System.Collections.Generic.List<System.Collections.Generic.List<NonlinearMethods.FunctionDelegate>>;
using Lab1;

namespace NonlinearMethods {
	internal class GD {
		private Vector X;
		private IOModule io;

		public Vector Answer {
			get { return X; }
		}

		public GD(Vector _X, FunctionVector functions, IFunctions F, FunctionMatrix derivatives, double lambda, double alpha, double eps, IOModule io) {
			this.io = io;

			Vector X0 = (Vector)_X.Clone();
			Vector X = (Vector)_X.Clone();

			double residualNorm = 0;
			double q = 0;
			double startAlpha = alpha;
			int iter = 0;

			FunctionVector derivativesF = new FunctionVector();
			derivativesF.Add(F.EvaluateDerivativeX);
			derivativesF.Add(F.EvaluateDerivativeY);

			Vector derivativesFVector = new Vector(2);
			Matrix Jacobian = new Matrix(2, 2);

			WriteHead();

			do {
				iter++;
				startAlpha = alpha;

				for(int i = 0; i < derivativesFVector.Length; i++) {
					derivativesFVector[i] = derivativesF[i](X0);
				}

				for(int i = 0; i < Jacobian.Rows; i++) {
					for(int j = 0; j < Jacobian.Cols; j++) {
						Jacobian[i, j] = derivatives[i][j](X);
					}
				}

				while(F.Evaluate(X0 - startAlpha * derivativesFVector) >= F.Evaluate(X0)) {
					startAlpha *= lambda;
				}

				X = X0 - startAlpha * derivativesFVector;

				q = Jacobian.EuclideNorm;

				residualNorm = Math.Sqrt(Math.Pow(X[0] - X0[0], 2) + Math.Pow(X[1] - X0[1], 2));

				WriteStep(iter, X, residualNorm, functions, q, alpha);

				X0 = (Vector)X.Clone();
			} while(residualNorm > eps);

			this.X = X;
		}

		private void WriteStep(int iter, Vector X, double residualNorm, FunctionVector functions, double q, double _alpha) { // печать шага итерации
			string iterStr = string.Format("{0,5}", iter);
			string x = io.PrettyfyDouble(X[0], 12);
			string y = io.PrettyfyDouble(X[1], 12);
			string resNorm = io.PrettyfyDouble(residualNorm, 12);
			string f1 = io.PrettyfyDouble(functions[0](X), 12);
			string f2 = io.PrettyfyDouble(functions[1](X), 12);
			string jacobianNorm = io.PrettyfyDouble(q, 12);
			string alpha = io.PrettyfyDouble(_alpha, 12);

			string str = $"| {iterStr} | {x} | {y} | {resNorm} | {f1} | {f2} | {jacobianNorm} | {alpha}";

			io.WriteLine(str);
		}

		private void WriteHead() { // печать шапки
			string centeredIter = io.CenterString("Iter", 7);
			string centeredX = io.CenterString("x", 14);
			string centeredY = io.CenterString("y", 14);
			string centeredResNorm = io.CenterString("Residual norm", 14);
			string F1 = io.CenterString("F1", 14);
			string F2 = io.CenterString("F2", 14);
			string jNorm = io.CenterString("Jacobian norm", 14);
			string centeredAlpha = io.CenterString("Alpha", 14);

			string head = $"|{centeredIter}|{centeredX}|{centeredY}|{centeredResNorm}|{F1}|{F2}|{jNorm}|{centeredAlpha}";

			Console.WriteLine(head);
		}
	}
}
