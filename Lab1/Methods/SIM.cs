using Vectors;
using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;

namespace Methods {
	internal class SIM {
		private Vector X;			//Вычисленный вектор X
		private IOModule io;		//Модуль IO

		private int theoretical;	//Теоретическое количество итераций

		/// <summary>
		/// Значение переменных X
		/// </summary>
		public Vector Answer {
			get { return X; }
		}

		/// <summary>
		/// Значение теоретического количества итераций
		/// </summary>
		public int Theoretical {
			get { return theoretical; }
		}

		/// <summary>
		/// Конструктор метода
		/// </summary>
		/// <param name="A">Матрица A</param>
		/// <param name="B">Матрица B</param>
		/// <param name="eps">Точность</param>
		/// <param name="io">Модуль IO</param>
		public SIM(Matrix A, Vector B, double eps, IOModule io) {
			this.io = io;

			WriteHead(B.Length);

			Vector X_Prev = new Vector(B.Length);
			Vector X_Curr = new Vector(B.Length);

			double stopCriterion;
			double tau = 0.9 * 2 / A.EuclideNorm;

			theoretical = (int)(A.EuclideCondition * Math.Log(1 / eps) / 2);

			int iter = 0;

			X_Curr.Fill(new double[] { 1, 2, 3, 4 });

			do {
				Tuple<Vector, double, double> res = SIMStep(A, B, X_Curr, X_Prev, tau);
				Vector X = res.Item1;
				double q = res.Item2;
				double residualNorm = res.Item3;

				Vector diff = X - X_Curr;
				double errNorm = diff.EnergyNorm(A);
				double errAss = q * errNorm / (1 - q);

				X_Prev = X_Curr;
				X_Curr = X;

				stopCriterion = residualNorm;

				WriteStep(iter, tau, q, residualNorm, errNorm, errAss, X);

				iter++;
			} while(stopCriterion > eps);

			this.X = X_Curr;
		}

		public SIM(Matrix A, Vector B, Vector X_Eval, double eps, IOModule io) {
			this.io = io;

			WriteHead(B.Length);

			Vector X_Prev = new Vector(B.Length);
			Vector X_Curr = (Vector)B.Clone();

			double stopCriterion;
			double residualNorm = (A * X_Curr - B).EnergyNorm(A);
			double tau = 0.9 * 2 / A.EuclideNorm;
			theoretical = (int)(A.EuclideCondition * Math.Log(1 / eps) / 2);
			eps *= (X_Curr - X_Eval).EnergyNorm(A);

			int iter = 0;

			do {
				Tuple<Vector, double, double> res = SIMStep(A, B, X_Curr, X_Prev, tau);
				Vector X = res.Item1;
				double q = res.Item2;

				Vector diff = X - X_Curr;
				double errNorm = diff.EnergyNorm(A);
				double errAss = q * errNorm / (1 - q);

				X_Prev = X_Curr;
				X_Curr = X;

				stopCriterion = (X - X_Eval).EnergyNorm(A);

				WriteStep(iter, tau, q, residualNorm, stopCriterion, errAss, X);

				residualNorm = res.Item3;

				iter++;
			} while(stopCriterion > eps);

			this.X = X_Curr;
		}

		/// <summary>
		/// Итерационный шаг
		/// </summary>
		/// <param name="A">Матрица A</param>
		/// <param name="B">Матрица B</param>
		/// <param name="X_Curr">X(k)</param>
		/// <param name="X_Prev">X(k-1)</param>
		/// <param name="tau">Тау</param>
		/// <returns>(Вектор X(k+1), Q, Норма невязки)</returns>
		private Tuple<Vector, double, double> SIMStep(Matrix A, Vector B, Vector X_Curr, Vector X_Prev, double tau) {
			Vector X = X_Curr + tau * (B - A * X_Curr);

			Vector residual = A * X - B;
			Vector Q_Numerator = X - X_Curr;
			Vector Q_Denumerator = X_Curr - X_Prev;

			double Q_NumEnergyNorm = Q_Numerator.EnergyNorm(A);
			double Q_DenumEnergyNorm = Q_Denumerator.EnergyNorm(A);
			double q = Q_NumEnergyNorm / Q_DenumEnergyNorm;

			double residualNorm = residual.EnergyNorm(A);

			return new Tuple<Vector, double, double>(X, q, residualNorm);
		}

		private void WriteStep(int iter, double tau, double q, double residualNorm, double errNorm, double errAss, Vector X) {
			string iterStr = string.Format("{0,5}", iter + 1);
			string tauStr = io.PrettyfyDouble(tau, 12);
			string qStr = io.PrettyfyDouble(q, 12);
			string residualNormStr = io.PrettyfyDouble(residualNorm, 12);
			string errNormStr = io.PrettyfyDouble(errNorm, 12);
			string errAssStr = io.PrettyfyDouble(errAss, 12);

			string str = $"| {iterStr} | {tauStr} | {qStr} | {residualNormStr} | {errNormStr} | {errAssStr} | {X.ToString()}";

			io.WriteLine(str);
		}

		private void WriteHead(int count) {
			string centeredIter = io.CenterString("Iter", 7);
			string centeredTau = io.CenterString("Tau", 14);
			string centeredQ = io.CenterString("Q", 14);
			string centeredResNorm = io.CenterString("Residual norm", 14);
			string centeredErrNorm = io.CenterString("Error norm", 14);
			string centeredErrEst = io.CenterString("Error estimate", 14);

			string x = " ";
			for(int i = 0; i < count; i++) {
				string centeredX = io.CenterString($"X[{i+1}]", 14);
				x = x + centeredX;
			}

			string head = $"|{centeredIter}|{centeredTau}|{centeredQ}|{centeredResNorm}|{centeredErrNorm}|{centeredErrEst}|{x}";

			io.WriteLine(head);
		}
	}
}
