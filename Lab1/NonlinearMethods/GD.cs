using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;
using Matrices;
using Lab1;

namespace NonlinearMethods {
	internal class GD {
		private Vector X;			//Ответ
		private IOModule io;		//Модуль ввода-вывода
		private IFunctions f1;		//Функция F1
		private IFunctions f2;		//Функция F2
		private IFunctions f;		//Функция F = F1^2 + F2^2

		public Vector Answer {
			get { return X; }
		}

		public GD(Vector _X, Vector X_NM, IFunctions[] functionsArr, double alpha, double lambda, double eps, IOModule io) {
			this.io = io;

			f1 = functionsArr[0];
			f2 = functionsArr[1];
			f = functionsArr[2];

			FunctionDelegate[] functions = { f1.Evaluate, f2.Evaluate };
			FunctionDelegate[] derivativesF = { f.EvaluateDerivativeX, f.EvaluateDerivativeY };
			FunctionDelegate[,] derivatives = {
				{ f1.EvaluateDerivativeX, f1.EvaluateDerivativeY },
				{ f2.EvaluateDerivativeX, f2.EvaluateDerivativeY }
			};

			Vector X0 = (Vector)_X.Clone();
			Vector X;

			double residualNorm = 1;
			double err = 0;
			double startAlpha = alpha;
			int iter = 0;
			int genIter = 0;

			Vector derivativesFVector = new Vector(2);
			Matrix Jacobian = new Matrix(2, 2);

			WriteHead();

			do {
				iter++;
				startAlpha = alpha;

				int k = 0;

				for(int i = 0; i < derivativesFVector.Length; i++) {
					derivativesFVector[i] = derivativesF[i](X0);
				}

				for(int i = 0; i < Jacobian.Rows; i++) {
					for(int j = 0; j < Jacobian.Cols; j++) {
						Jacobian[i, j] = derivatives[i, j](X0);
					}
				}

				while(f.Evaluate(X0 - startAlpha * derivativesFVector) >= f.Evaluate(X0)) {
					startAlpha *= lambda;
					k++;
					genIter++;
				}

				X = X0 - startAlpha * derivativesFVector;

				err = (X_NM - X).Norm();

				residualNorm = Math.Sqrt(Math.Pow(functions[0](X), 2) + Math.Pow(functions[1](X), 2));

				WriteStep(iter, X, residualNorm, err, startAlpha, k);

				X0 = (Vector)X.Clone();
			} while(residualNorm > eps);

			io.WriteLine();
			io.WriteLine($"All iterations = {genIter}");

			this.X = X;
		}

		private void WriteStep(int iter, Vector X, double residualNorm, double err, double _alpha, int _k) { // печать шага итерации
			string iterStr = string.Format("{0,5}", iter);
			string x = io.PrettyfyDouble(X[0], 12);
			string y = io.PrettyfyDouble(X[1], 12);
			string resNorm = io.PrettyfyDouble(residualNorm, 12);
			string _err = io.PrettyfyDouble(err, 12);
			string alpha = io.PrettyfyDouble(_alpha, 12);
			string k = string.Format("{0,5}", _k);

			string str = $"| {iterStr} | {x} | {y} | {alpha} | {resNorm} | {_err} | {k}";

			io.WriteLine(str);
		}

		private void WriteHead() { // печать шапки
			string centeredIter = io.CenterString("Iter", 7);
			string centeredX = io.CenterString("x", 14);
			string centeredY = io.CenterString("y", 14);
			string centAlpha = io.CenterString("Alpha", 14);
			string centeredResNorm = io.CenterString("Residual Norm", 14);
			string centeredErrEst = io.CenterString("Error norm", 14);
			string ceneteredK = io.CenterString("K", 7);

			string head = $"|{centeredIter}|{centeredX}|{centeredY}|{centAlpha}|{centeredResNorm}|{centeredErrEst}|{ceneteredK}";

			io.WriteLine(head);
		}
	}
}
