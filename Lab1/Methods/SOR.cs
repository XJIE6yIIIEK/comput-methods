using System;
using Vectors;
using Matrices;
using Lab1;

namespace Methods
{

	internal class SOR
	{
		private Vector X;           //Вычисленный вектор X
		IOModule io;                //Модуль IO

		private int theoretical;    //Теоретическое количество итераций

		/// <summary>
		/// Значение теоретического количества итераций
		/// </summary>
		public int Theoretical {
			get { return theoretical; }
		}

		/// <summary>
		/// Значение переменных X
		/// </summary>
		public Vector Answer
		{
			get { return X; }
		}

		/// <summary>
		/// Метод ПВР
		/// </summary>
		public int SOR_Method(double w, Vector X, Vector startVector, Matrix A, Vector B, double eps, bool output) {
			int iter = 0;
			double residualNorm; // норма невязки
			Vector Z = (Vector)startVector.Clone(); // вектор значений, вычисленных по методу Зейделя
			Vector newX = (Vector)startVector.Clone(); // текущее значение X
			Vector prevX = (Vector)startVector.Clone(); // предыдущее значение X
			do {
				prevX = (Vector)newX.Clone(); // сохранение старого значения
				residualNorm = (B - A * newX).EnergyNorm(A); // вычисление нормы невязки
				iter++;	 // увеличение счетчика итерации

				for (int i = 0; i < Z.Length; i++) { // вычисление новых значений векторов
					double sumNew = 0; // сумма, полученная на основе текущих вычисленных значений (до i)
					double sumOld = 0; // сумма, полученная на основе старых вычисленных значений (после i)
					for (int j = 0; j < i; j++)	// вычисление sumNew
						sumNew += A[i, j] * newX[j];
					for (int j = i + 1; j < newX.Length; j++) // вычисление sumOld
						sumOld += A[i, j] * newX[j];
					Z[i] = 1 / A[i, i] * (B[i] - sumNew - sumOld); // вычисление X[i] по методу Зейделя
					newX[i] = newX[i] + w * (Z[i] - newX[i]); // вычисление X[i] по методу Релаксации
				}

				if (output) // вывод 
					this.WriteStep(iter, w, residualNorm, (X - newX).EnergyNorm(A), newX);
			}												// условие остановки алгоритма
			while ((X - newX).EnergyNorm(A) / (X - startVector).EnergyNorm(A) >= eps);

			this.X = (Vector)newX.Clone(); // сохранение полученного значения в поле X
			return iter; // вернуть количество итераций
		}

		public SOR(Matrix A, Vector B, Vector X, double eps, IOModule io, double w = 0, double eps_w0 = 1E-2) {
			this.io = io; // модуль ввода вывода
			 // теоритическая оценка алгоритма

			theoretical = (int)(Math.Sqrt(A.EuclideCondition) * Math.Log(1 / eps) / 4);
			

			int min = int.MaxValue;	 // min итераций
			if (w == 0)
				for (int i = 1; i <= 19; i += 1) {  // поиск минимального значения w 
				    // решение с заданной погрешностью
					double cur_w = (double)i / 10.0;
					int cur = this.SOR_Method(cur_w, X, B, A, B, eps_w0, false);
					this.WriteStep((double)i / 10, cur); // вывод
					if (cur < min) // выбор минимального
					{
						min = cur;
						w = cur_w;
					}
				}

			io.WriteLine($"w* = {io.PrettyfyDouble(w, 6)}; ItrMin = {min}"); // вывод верного значения

			SOR_Method(w, X, B, A, B, eps, true); // решение с заданной погрешностью и наилучшим w
		}

		private void WriteStep(double w, int iter) { // вывод шага итерации
			string iterStr = string.Format("{0,5}", iter);
			string wIter = io.PrettyfyDouble(w, 12);

			string str = $"| {wIter} | {iterStr}  ";

			io.WriteLine(str);
		}

		private void WriteHeadW(int count) { // вывод шапки
			string centeredIter = io.CenterString("Iter", 7);
			string centeredW = io.CenterString("w", 14);

			string head = $"|{centeredW}|{centeredIter}";

			io.WriteLine(head);
		}

		// вывод шага итерации
		private void WriteStep(int iter, double w, double residualNorm, double errNorm, Vector X) { 
			string iterStr = string.Format("{0,5}", iter);
			string wIter = io.PrettyfyDouble(w, 12);
			string residualNormStr = io.PrettyfyDouble(residualNorm, 12);
			string errNormStr = io.PrettyfyDouble(errNorm, 12);

			string str = $"| {iterStr} | {wIter} | {residualNormStr} | {errNormStr} | {X.ToString()}";

			io.WriteLine(str);
		}

		private void WriteHead(int count) {	 // вывод шапки
			string centeredIter = io.CenterString("Iter", 7);
			string centeredW = io.CenterString("w", 14);
			string centeredResNorm = io.CenterString("Residual norm", 14);
			string centeredErrNorm = io.CenterString("Error norm", 14);

			string x = " ";
			for(int i = 0; i < count; i++) {
				string centeredX = io.CenterString($"X[{i + 1}]", 14);
				x = x + centeredX;
			}

			string head = $"|{centeredIter}|{centeredW}|{centeredResNorm}|{centeredErrNorm}|{x}";

			io.WriteLine(head);
		}
	}
}
