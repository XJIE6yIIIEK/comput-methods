using System;
using Vectors;

namespace Matrices {
	enum MatrixType {
		Null = -1,
		General = 0,
		L = 1,
		U = 2
	}

	/// <summary>
	/// Матричный класс
	/// </summary>
	internal class Matrix {
		private protected double[,] matrix;	  //Матрица значений

		private protected Matrix t;			  //Матрицы поворота
		private protected Matrix tt;			  //Транспонированная матрица поворота
		public Matrix selfVals;		  //Матрица собственных векторов

		private protected Matrix inverseMatrix; //Обратная матрица

		private protected int rows;			  //Строки
		private protected int cols;			  //Столбцы
		private protected MatrixType _type;     //Тип матрицы в LUP-разложении

		private protected double? determinant;  //Определитель
		private protected double? quadricNorm;  //Квадратичная норма
		private protected double? octoNorm;	  //Октоэдрическая норма
		private protected double? euclideNorm;  //Евклидова норма

		public double this[int n, int m] {
			get {
				if(n >= rows || m >= cols || n < 0 || m < 0) {
					throw new Exception("Out of array");
				}

				return matrix[n, m];
			}

			set {
				if(n >= rows || m >= cols || n < 0 || m < 0) {
					throw new Exception("Out of array");
				}

				matrix[n, m] = value;
			}
		}

		/// <summary>
		/// Количество строк матрицы. <i><b>Только для чтения</b></i>.
		/// </summary>
		public int Rows {
			get { return rows; }
		}

		/// <summary>
		/// Количество столбцов матрицы <i><b>Только для чтения</b></i>.
		/// </summary>
		public int Cols {
			get { return cols; }
		}

		/// <summary>
		/// Определитель матрицы. <i><b>Только для чтения</b></i>.
		/// </summary>
		/// <remarks>
		/// <i>После первого вызова значение кэшируется.</i>
		/// </remarks>
		public double Determinant {
			get {
				if(determinant == null) {
					determinant = MatrixDeterminant();
				}

				return (double)determinant;
			}
		}

		/// <summary>
		/// Квадратичная норма матрицы. <i><b>Только для чтения</b></i>.
		/// </summary>
		/// <remarks>
		/// <i>После первого вызова значение кэшируется.</i>
		/// </remarks>
		public double QuadricNorm {
			get {
				if(quadricNorm != null) {
					return (double)quadricNorm;
				}

				quadricNorm = Norm12(1);

				return (double)quadricNorm;
			}
		}

		/// <summary>
		/// Октоэдрическая норма матрицы. <i><b>Только для чтения</b></i>.
		/// </summary>
		/// <remarks>
		/// <i>После первого вызова значение кэшируется.</i>
		/// </remarks>
		public double OctoNorm {
			get {
				if(octoNorm != null) {
					return (double)octoNorm;
				}

				octoNorm = Norm12(2);

				return (double)octoNorm;
			}
		}

		/// <summary>
		/// Евклидова норма матрицы. <i><b>Только для чтения</b></i>.
		/// </summary>
		/// <remarks>
		/// <i>После первого вызова значение кэшируется.</i>
		/// </remarks>
		public double EuclideNorm {
			get {
				if(euclideNorm != null) {
					return (double)euclideNorm;
				}

				euclideNorm = Norm3();

				return (double)euclideNorm;
			}
		}

		/// <summary>
		/// Обратная матрица. <i><b>Только для чтения</b></i>.
		/// </summary>
		/// <remarks>
		/// <i>После первого вызова значение кэшируется.</i>
		/// </remarks>
		public Matrix Inverse {
			get {
				if (inverseMatrix != null) {
					return inverseMatrix;
				}

				inverseMatrix = MatrixInverse();

				return inverseMatrix;
			}
		}

		/// <summary>
		/// Конструктор класса Matrix
		/// </summary>
		/// <param name="n">Количество строк</param>
		/// <param name="m">Количество столбцов</param>
		/// <param name="_type">Тип матрицы</param>
		public Matrix(int n, int m, MatrixType _type = MatrixType.General) {
			rows = n;
			cols = m;
			matrix = new double[n, m];
			this._type = _type;

			for(int i = 0; i < n; i++) {
				for(int j = 0; j < m; j++) {
					matrix[i, j] = 0;
				}
			}
		}

		/// <summary>
		/// Глубокое копирование матрицы
		/// </summary>
		/// <param name="_type">Тип матрицы. 
		/// Если <i>null</i>, то принимает значение копируемой матрицы</param>
		/// <returns>Возвращает скопированную матрицу</returns>
		public Matrix DeepClone(MatrixType _type = MatrixType.Null) { 
			Matrix res = new Matrix(rows, cols, (_type == MatrixType.Null ? this._type : _type));

			for(int i = 0; i < rows; i++) {
				for(int j = 0; j < cols; j++) {
					res[i, j] = matrix[i, j];
				}
			}

			if(t != null) {
				res.t = t.Clone();
				res.tt = tt.Clone();
				res.selfVals = selfVals.Clone();
			}

			return res;
		}

		/// <summary>
		/// Быстрая перестановка строк матрицы.
		/// </summary>
		/// <param name="n">Индекс переставляемой строки</param>
		/// <param name="m">Индекс переставляемой строки</param>
		/// <returns>Матрица с переставленными строками</returns>
		public Matrix FastSwap(int n, int m) {
			Matrix res = DeepClone();

			for (int i = 0; i < cols; i++) {
				double tmp = res[n, i];
				res[n, i] = res[m, i];
				res[m, i] = tmp;
			}

			return res;
		}

		/// <summary>
		/// Быстрая перестановка строк матрицы.
		/// </summary>
		/// <param name="m">Матрица индексов перестановок</param>
		/// <returns>Матрица с переставленными строками</returns>
		public Matrix FastSwap(int[] m) {
			Matrix res = DeepClone();

			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < cols; j++) {
					res[i, j] = matrix[m[i], j];
				}
			}

			return res;
		}

		/// <summary>
		/// Неполное копирование матрицы. Копируются только значения.
		/// </summary>
		/// <returns>Возвращает поверхностную копию матрицу</returns>
		public Matrix Clone() {
			Matrix res = new Matrix(rows, cols, (_type == MatrixType.Null ? this._type : _type));

			for(int i = 0; i < rows; i++) {
				for(int j = 0; j < cols; j++) {
					res[i, j] = matrix[i, j];
				}
			}

			return res;
		}

		/// <summary>
		/// Копирование матрицы.
		/// </summary>
		/// <param name="n">Количество строк</param>
		/// <param name="m">Количество столбцов</param>
		/// <param name="cloningMatrix">Клонируемая матрица</param>
		/// <param name="_type">Тип склонированной матрицы</param>
		/// <returns>Клонированная матрица</returns>
		public static Matrix Clone(int n, int m, double[,] cloningMatrix, MatrixType _type = MatrixType.General) {
			Matrix res = new Matrix(n, m, _type);

			for(int i = 0; i < n; i++) {
				for(int j = 0; j < m; j++) {
					res[i, j] = cloningMatrix[i, j];
				}
			}

			return res;
		}

		/// <summary>
		/// Копирование вектора.
		/// </summary>
		/// <param name="n">Количество строк</param>
		/// <param name="cloningVector">Клонируемый вектор</param>
		/// <returns>Клонированный вектор</returns>
		public static Matrix Clone(int n, int[] cloningVector) {
			Matrix res = new Matrix(n, 1);

			for (int i = 0; i < n; i++) {
				res[i, 0] = cloningVector[i];
			}

			return res;
		}

		/// <summary>
		/// Нахождение матрицы поворота
		/// </summary>
		/// <param name="i">Индекс строки максимального элемента</param>
		/// <param name="j">Индекс столбца максимального элемента</param>
		/// <param name="alfa">Угол поворота</param>
		private protected void SetT(int i, int j, double alfa) {
			for (int k = 0; k < rows; k++)
			{
				for (int m = 0; m < cols; m++)
					if (k == m)
						t[k, m] = 1;
					else
						t[k, m] = 0;
			}

			t[i, i] = Math.Cos(alfa);
			t[j, j] = Math.Cos(alfa);
			t[i, j] = -Math.Sin(alfa);
			t[j, i] = Math.Sin(alfa);
		}

		/// <summary>
		/// Повороты матрицы
		/// </summary>
		public void Permutation() {
			double max = double.MinValue;
			int k = 0, m = 0;

			if (selfVals == null)
			{
				selfVals = Clone();
				t = new Matrix(rows, cols, MatrixType.General);
			}

			//Поиск индексов и значения максимального элемента
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
					if (i != j && Math.Abs(selfVals[i, j]) > max)
					{
						max = Math.Abs(selfVals[i, j]);
						k = i;
						m = j;
					}
			}

			if (max < 0.00000001)
				return ;

			//Поиск угла поворота
			double tan = 2 * selfVals[k, m] / (selfVals[k, k] - selfVals[m, m]);
			double alfa = Math.Tan(tan) / 2;

			//Нахождение матрицы поворота
			SetT(k, m, alfa);

			tt = t.Transposition();
			selfVals = tt * selfVals;
			selfVals = selfVals * t;

			Permutation();
		}

		/// <summary>
		/// Евклидова норма
		/// </summary>
		/// <returns>Значение нормы</returns>
		private protected double Norm3() {
			double max = double.MinValue;
			double min = double.MaxValue;

			//Вычисление эрмитово сопряженной матрицы
			Matrix A_Ev = (this * Transposition());

			//Повороты
			A_Ev.Permutation();

			Matrix selfVars = A_Ev.selfVals;

			for (int i = 0; i < rows; i++) {
				double selfVar = Math.Abs(selfVars[i, i]);

				if (selfVar > max){
					max = selfVar;
				}
				
				if (selfVar < min) {
					min = selfVar;
				}
			}

				return Math.Sqrt(max / min);
		}

		/// <summary>
		/// Вычисление квадратичной/октоэдрической нормы
		/// </summary>
		/// <param name="type">Тип нормы. 
		/// 1 - Квадратичная, 
		/// 2 - Октоэдрическая</param>
		/// <returns>Значение нормы</returns>
		private protected double Norm12(int type) {
			double max = double.MinValue;

			for(int i = 0; i < rows; i++) {
				double sum = 0;

				for(int j = 0; j < cols; j++) {
					if(type == 1) {
						sum += Math.Abs(matrix[i, j]);
					} else {
						sum += Math.Abs(matrix[j, i]);
					}
				}

				if(sum > max) {
					max = sum;
				}
			}

			return max;
        }

		//Переопределение суммы матриц
		public static Matrix operator +(Matrix a, Matrix b) {
			if(a.Cols != b.Cols || a.Rows != b.Rows) {
				throw new Exception("Incorrect matrix sizes");
			}

			Matrix res = new Matrix(a.Rows, a.Cols);

			for(int i = 0; i < a.Rows; i++) {
				for(int j = 0; j < a.Cols; j++) {
					res[i, j] = a[i, j] + b[i, j];
				}
			}

			return res;
		}

		//Переопределение разницы матрицы
		public static Matrix operator -(Matrix a, Matrix b) {
			if(a.Cols != b.Cols || a.Rows != b.Rows) {
				throw new Exception("Incorrect matrix sizes");
			}

			Matrix res = new Matrix(a.Rows, a.Cols);

			for(int i = 0; i < a.Rows; i++) {
				for(int j = 0; j < a.Cols; j++) {
					res[i, j] = a[i, j] - b[i, j];
				}
			}

			return res;
		}

		//Переопределение матричного умножения
		public static Matrix operator *(Matrix a, Matrix b) {
			if(a.Cols != b.Rows) {
				throw new Exception("Incorrect matrix sizes");
			}

			Matrix res = new Matrix(a.Rows, b.Cols);

			for(int i = 0; i < a.Rows; i++) {
				for(int j = 0; j < b.Cols; j++) {
					double elem_res = 0;

					for(int n = 0; n < a.Cols; n++) {
						elem_res += a[i, n] * b[n, j];
					}

					res[i, j] = elem_res;
				}
			}

			return res;
		}

		//Переопределение умножения матрицы на скаляр
		public static Matrix operator *(int c, Matrix a) {
			Matrix res = new Matrix(a.Rows, a.Cols);

			for(int i = 0; i < a.Rows; i++) {
				for(int j = 0; j < a.Cols; j++) {
					res[i, j] = a[i, j] * c;
				}
			}

			return res;
		}

		/// <summary>
		/// Вычисление определителя методом разложения строки.
		/// </summary>
		/// <returns>Определитель матрицы</returns>
		private protected virtual double MatrixDeterminant() {
			double determinant = 0;

			if(Rows == 1 && Cols == 1) {
				determinant = matrix[0, 0];
				return determinant;
			}

			for(int n = 0; n < Cols; n++) {
				Matrix minor = new Matrix(Rows - 1, Cols - 1);

				for(int i = 0; i < Rows - 1; i++) {
					for(int j = 0; j < Cols; j++) {
						if(j == n) {
							continue;
						}

						if(j < n) {
							minor[i, j] = matrix[i, j];
						} else {
							minor[i, j - 1] = matrix[i, j];
						}
					}
				}

				double minor_determinant = matrix[Rows - 1, n] * minor.Determinant;

				determinant += Math.Pow(-1, Cols + n + 1) * minor_determinant;
			}

			return determinant;
		}

		/// <summary>
		/// Транспонирование матрицы
		/// </summary>
		/// <returns>Транспонированная матрица</returns>
		public Matrix Transposition() {
			Matrix res = new Matrix(rows, cols, _type);

			for(int i = 0; i < rows; i++) {
				for(int j = 0; j < cols; j++) {
					res[j, i] = matrix[i, j];
				}
			}

			return res;
		}

		/// <summary>
		/// Нахождение обратной матрицы
		/// </summary>
		/// <returns>Обратная матрица</returns>
		private protected virtual Matrix MatrixInverse() {
			Matrix res = new Matrix(rows, cols, MatrixType.L);
			Matrix matrixT = this.Transposition();

			for(int i = 0; i < rows; i++) {
				for(int j = 0; j < cols; j++) {
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

			return res;
		}

		/// <summary>
		/// Конвертация матрицы в строку
		/// </summary>
		/// <returns>Строковое представление матрицы</returns>
		public new virtual string ToString() {
			string res = "";

			for(int i = 0; i < rows; i++) {
				string tmp = "";

				for(int j = 0; j < cols; j++) {
					string doubleStr = string.Empty;

					if(Math.Abs(matrix[i, j]) > 0.0000001) {
						doubleStr = string.Format("{0,14:0.000000;-0.000000;0} ", matrix[i, j]);
					} else { 
						doubleStr = string.Format("{0,14:0.0000E0;-0.0000E0;0} ", matrix[i, j]);
					}

					tmp = tmp + doubleStr;
				}

				res = res + tmp + "\n";
			}

			res = res.Substring(0, res.Length - 1);
			
			return res;
		}
	}
}
