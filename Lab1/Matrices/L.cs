using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrices {
	internal class L : Matrix {
		public L(int rows, int cols) : base(rows, cols, MatrixType.L) {

		}

		/// <summary>
		/// Нахождение обратной нижнетреугольной матрицы
		/// </summary>
		/// <returns>Обратная матрица</returns>
		private protected override Matrix MatrixInverse() {
			Matrix res = new L(rows, cols);
			Matrix matrixT = Transposition();

			for(int i = 0; i < rows; i++) {
				for(int j = 0; j < cols; j++) {
					if(i == j) {
						res[i, j] = 1 / matrixT[i, j];
					} else if(j > i) {
						res[i, j] = 0;
					} else {
						Matrix addict = new Matrix(rows - 1, cols - 1);

						for(int n = 0; n < rows; n++) {
							for(int m = 0; m < cols; m++) {
								if(n == i || m == j) {
									continue;
								}

								addict[(n < i ? n : n - 1), (m < j ? m : m - 1)] = matrixT[n, m];
							}
						}

						res[i, j] = Math.Pow(-1, i + 1 + j + 1) * addict.Determinant / Determinant;
					}
				}
			}

			return res;
		}

		/// <summary>
		/// Вычисление определителя нижнетреугольной матрицы
		/// </summary>
		/// <returns>Определитель матрицы</returns>
		private protected override double MatrixDeterminant() {
			double determinant = 1;
			for(int i = 0; i < Rows; i++) {
				determinant *= matrix[i, i];
			}

			return determinant;
		}
	}
}
