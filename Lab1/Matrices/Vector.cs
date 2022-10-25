using System;
using Matrices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vectors {
	internal class Vector : ICloneable {
		private protected double[] vector;		//Матрица значений

		private protected int length;			//Длина вектора

		/// <summary>
		/// Длина вектора
		/// </summary>
		public int Length {
			get { return length; }
		}

		public double this[int i] {
			get {
				if(i >= length || i < 0) {
					throw new Exception("Out of array");
				}

				return vector[i];
			}

			set {
				if(i >= length || i < 0) {
					throw new Exception("Out of array");
				}

				vector[i] = value;
			}
		}

		public Vector(int length) {
			this.length = length;
			vector = new double[length];

			for(int i = 0; i < length; i++) {
				vector[i] = 0;
			}
		}

		/// <summary>
		/// Перестановка элементов
		/// </summary>
		/// <param name="swapIndexes">Матрица индексов перестановок</param>
		/// <returns>Вектор с переставленными элементами</returns>
		public Vector FastSwap(int[] swapIndexes) {
			Vector res = new Vector(length);

			for(int i = 0; i < length; i++) {
				res[i] = vector[swapIndexes[i]];
			}

			return res;
		}

		/// <summary>
		/// Перестановка элементов
		/// </summary>
		/// <param name="k">k - й элемент</param>
		/// <param name="l">l - й элемент</param>
		/// <returns>Вектор с переставленными элементами</returns>
		public Vector FastSwap(int k, int l) {
			Vector res = (Vector)Clone();

			double tmp = res[k];
			res[k] = res[l];
			res[l] = tmp;

			return res;
		}

		/// <summary>
		/// Заполнение вектора указанным значением
		/// </summary>
		/// <param name="arr">Массив значений</param>
		public void Fill(double[] arr) {
			for(int i = 0; i < length; i++) {
				vector[i] = arr[i];
			}
		}

		/// <summary>
		/// Заполнение вектора указанным значением
		/// </summary>
		/// <param name="num">Значение</param>
		public void Fill(double num) {
			for(int i = 0; i < length; i++) {
				vector[i] = num;
			}
		}

		/// <summary>
		/// Подсчёт энергетической нормы
		/// </summary>
		/// <param name="A">Матрица</param>
		/// <returns>Значение энергетической нормы</returns>
		public double EnergyNorm(Matrix A) {
			return Math.Sqrt(Dot(A * this, this));
		}

		public double Norm() {
			double sum = 0;
			for (int i = 0; i < this.length; i++) {
				sum += this[i] * this[i];
            }
			return Math.Sqrt(sum);
        }

		/// <summary>
		/// Копирование
		/// </summary>
		/// <returns></returns>
		public object Clone() {
			Vector res = new Vector(length);

			for(int i = 0; i < length; i++) {
				res[i] = vector[i];
			}

			return res;
		}

		/// <summary>
		/// Получение вектора из нативного массива
		/// </summary>
		/// <param name="arr">Массив значений</param>
		/// <param name="length">Длина массива</param>
		/// <returns>Вектор</returns>
		public static Vector GetFromArray(int[] arr, int length) {
			Vector res = new Vector(length);

			for(int i = 0; i < length; i++) {
				res[i] = arr[i];
			}

			return res;
		}

		/// <summary>
		/// Скалярное произведение векторов
		/// </summary>
		/// <param name="a">Вектор</param>
		/// <param name="b">Вектор</param>
		/// <returns>Значение скалярного произведения</returns>
		public static double Dot(Vector a, Vector b) {
			if(a.length != b.length) {
				throw new Exception("Vectors length are not same!");
			}

			double res = 0;

			for(int i = 0; i < a.length; i++) {
				res += a[i] * b[i];
			}

			return res;
		}

		/// <summary>
		/// Векторное произведение векторов
		/// </summary>
		/// <param name="a">Вектор</param>
		/// <param name="b">Вектор</param>
		/// <returns>Матрица</returns>
		public static Matrix Cross(Vector a, Vector b) {
			Matrix res = new Matrix(a.length, a.length);

			for(int i = 0; i < a.length; i++) {
				for(int j = 0; j < a.length; j++) {
					res[i, j] = a[i] * b[j];
				}
			}

			return res;
		}

		public static Vector operator *(Matrix a, Vector b) {
			Vector res = new Vector(b.length);

			for(int i = 0; i < a.Rows; i++) {
				double sum = 0;

				for(int j = 0; j < b.length; j++) {
					sum += a[i, j] * b[j];
				}

				res[i] = sum;
			}

			return res;
		}

		public static Vector operator *(Vector a, Matrix b) {
			Vector res = new Vector(a.length);

			for(int i = 0; i < a.length; i++) {
				double sum = 0;

				for(int j = 0; j < b.Cols; j++) {
					sum += a[j] * b[j, i];
				}

				res[i] = sum;
			}

			return res;
		}

		public static Vector operator *(double a, Vector b) {
			Vector res = new Vector(b.Length);

			for(int i = 0; i < res.length; i++) {
				res[i] = a * b[i];
            }

			return res;
		}

		public static Vector operator *(Vector a, double b)
		{
			Vector res = new Vector(a.Length);

			for (int i = 0; i < res.length; i++)
			{
				res[i] = a[i] * b;
			}

			return res;
		}
		public static Vector operator +(Vector a, Vector b) {
			Vector res = new Vector(a.length);

			for(int i = 0; i < a.length; i++) {
				res[i] = a[i] + b[i];
			}

			return res;
		}

		public static Vector operator -(Vector a, Vector b) {
			Vector res = new Vector(a.length);

			for(int i = 0; i < a.length; i++) {
				res[i] = a[i] - b[i];
			}

			return res;
		}

		/// <summary>
		/// Конвертация вектора в строку
		/// </summary>
		/// <returns>Строковое представление вектора</returns>
		public new string ToString() {
			string res = "";

			for(int i = 0; i < length; i++) {
				string doubleStr = string.Empty;

				if(Math.Abs(vector[i]) > 0.0000001) {
					doubleStr = string.Format("{0,14:0.000000;-0.000000;0} ", vector[i]);
				} else {
					doubleStr = string.Format("{0,14:0.0000E0;-0.0000E0;0} ", vector[i]);
				}

				res = res + doubleStr;
			}

			return res;
		}
	}
}
