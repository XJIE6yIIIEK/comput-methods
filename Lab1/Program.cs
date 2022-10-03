using System;
using Matrices;
using Vectors;
using Methods;
using System.Runtime.InteropServices;

namespace Lab1
{

    class LU
    {
        public Matrix A;
        public Matrix U;
        public Matrix L;
        public Vector B;
        public Matrix P;
        public Matrix E;

        private int n; //Размерность
        public int? rang; //Ранг матрицы A

		/// <summary>
		/// Матрица, содержащая индексы перестановок
		/// </summary>
		public int[] m;
        public int permutNumber = 0;

		public IOModule extIOModule;

        /// <summary>
        /// Инициализация матрицы, как единичной
        /// </summary>
        /// <param name="E">Матрица, в главную диагональ которой будут записаны единицы</param>
		static public void InitializeE(Matrix E) {
			for(int i = 0; i < E.Rows; i++) {
				for(int j = 0; j < E.Cols; j++) {
					if(i == j) {
						E[i, j] = 1;
					}
				}
			}
		}

        /// <summary>
        /// LUP-разложение матрицы A
        /// </summary>
        /// <param name="_A">Матрица A</param>
        /// <param name="_B">Правый вектор B</param>
        /// <param name="n">Размерность</param>
        /// <param name="ioModule">Модкль ввода-вывода</param>
		public LU(Matrix _A, Vector _B, int n, IOModule ioModule)
        {
            extIOModule = ioModule;
            ioModule.WriteLine("LUP-decomposition");

			this.n = n;

			//Глубокое копирование матриц, для сохранности исходных данных
			A = _A.DeepClone();
            B = (Vector)_B.Clone();
			U = A.DeepClone(MatrixType.U);

			L = new L(n, n);
			P = new Matrix(n, n);
			E = new Matrix(n, n);

            //Инициализация единичных матриц
			LU.InitializeE(E);
			LU.InitializeE(P);

			m = new int[n];
            for (int i = 0; i < n; i++) {
                m[i] = i;
            }

            for (int k = 0; k < n; k++)
            {
				//Поиск перестановки в k-ом столбце
				Tuple<int, double> swapResult = GetSwapIndex(k, m);
                int m0 = swapResult.Item1;
                double maxDiagVal = swapResult.Item2;

                if (maxDiagVal < 1E-6)
                    rang = k;

				ioModule.WriteLine($"k={k + 1}, m={m0 + 1}");

				//Выполнение перестановки строк в матрицах U, A, B, P,
                //если опорный элемент не на главной диагонали
				if(m0 != k)
                {
                    U = U.FastSwap(m0, k);
                    A = A.FastSwap(m0, k);
                    B = B.FastSwap(m0, k);
                    P = P.FastSwap(m0, k);

                    permutNumber += 1;

					ioModule.WriteLine("P=", 1);
					ioModule.WriteLine(P.ToString(), 2);
                }

				//Преобразование методом Гаусса
				double ukk = U[k, k];

                for (int j = k; j < n; j++)
                    U[k, j] = U[k, j] / ukk;

                B[k] = B[k] / ukk;

                for (int i = k + 1; i < n; i++)
                {
                    double aik = U[i, k];
                    B[i] = B[i] - B[k] * aik;

                    for (int j = k; j < n; j++)
                    {
                        U[i, j] = U[i, j] - U[k, j] * aik;
                    }
                }

                for (int j = 0; j <= k; j++)
                {
                    L[k, j] = A[k, j];
                    double c = 0;
                    for (int i = 0; i < j; i++)
                    {
                        c += L[k, i] * U[i, j];
                    }
                    L[k, j] -= c;
                }

                ioModule.WriteLine("U=", 1);
				ioModule.WriteLine(U.ToString(), 2);
				ioModule.WriteLine("L=", 1);
				ioModule.WriteLine(L.ToString(), 2);
			}

            if (rang == null)
                rang = n;

            Vector pVal = Vector.GetFromArray(m, n);            

            ioModule.SeparateText();
            ioModule.WriteLine("P Vector=");
            ioModule.WriteLine(pVal.ToString(), 1);
            ioModule.WriteLine();
            ioModule.WriteLine($"Rank = {rang}");
			ioModule.SeparateText();
		}

        /// <summary>
        /// Получение индекса перестановки
        /// </summary>
        /// <param name="j">Столбец выбора опорного элемента</param>
        /// <param name="m">Вектор индексов перестановок</param>
        /// <returns>Пара (индекс перестановки, максимальное значение на диагонали)</returns>
        Tuple<int, double> GetSwapIndex(int j, int[] m)
        {
            double max = double.MinValue;
            int ind = -1;

            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(U[i, j]) > max)
                {
                    max = Math.Abs(U[i, j]);
                    ind = i;
                }
            }

            if (ind != j) {
                int tmp = m[ind];
                m[ind] = m[j];
                m[j] = tmp;
            }

            return new Tuple<int, double>(ind, max);
        }

        /// <summary>
        /// Получение вектора X
        /// </summary>
        /// <returns></returns>
        public Vector Answer()
        {
            return U.Inverse * B;
        }

        public int GetSign() {
            if(permutNumber % 2 == 0) {
                return 1;
            }

            return -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IOModule ioModule = new IOModule();

            ioModule.ExcelInit();

            int n;
            ioModule.GetSize(out n);

            Matrix A = new Matrix(n, n);
            Vector B = new Vector(n);
			//Vector X = new Vector(n);

            ioModule.GetInfo(A, B);

            //Вычисление правого вектора
            //B = A * X;

            ioModule.TerminateExcel();

            ioModule.FileOpen();

            /*ioModule.WriteLine("INPUT");
            ioModule.WriteLine("X=");
            ioModule.WriteLine(X.ToString(), 1);
			ioModule.WriteLine("A=");
			ioModule.WriteLine(A.ToString(), 1);
			ioModule.WriteLine("B=");
			ioModule.WriteLine(B.ToString(), 1);
            ioModule.SeparateText();

            //LUP-разложение
			LU lu = new LU(A, B, n, ioModule);*/

            SSRM ssrm_eval = new SSRM(A, B);
            ioModule.WriteLine($"{ssrm_eval.Answer.ToString()}");
            ioModule.SeparateText();
            ioModule.WriteLine($"{(A.Inverse * B).ToString()}");
            
            /*//Нахождение вектора X
			Vector _X = lu.Answer();
            //Проверка
			Matrix X_diff = (lu.L * lu.U) - lu.A;

			ioModule.WriteLine("X_Eval =");
			ioModule.WriteLine(_X.ToString(), 1);
			ioModule.WriteLine("L * U - P * A =");
            ioModule.WriteLine(X_diff.ToString(), 1);
            ioModule.SeparateText();

            //Нахождение обратной матрицы
            Matrix A_INVERSE = lu.U.Inverse * lu.L.Inverse * lu.E.FastSwap(lu.m);
            //Проверка
            Matrix A_diff = lu.A * A_INVERSE - lu.P;

			ioModule.WriteLine("A=");
			ioModule.WriteLine(lu.A.ToString(), 1);
			ioModule.WriteLine("A^(-1)=");
			ioModule.WriteLine(A_INVERSE.ToString(), 1);
			ioModule.WriteLine("A * A^(-1) - E =");
			ioModule.WriteLine(A_diff.ToString(), 1);
			ioModule.SeparateText();

            //Нахождение норм
            double quadNorm = lu.A.QuadricNorm * A_INVERSE.QuadricNorm;
			double octNorm = lu.A.OctoNorm * A_INVERSE.OctoNorm;
			double eucNorm = lu.A.EuclideNorm;

			ioModule.WriteLine("Cond(A) = ");
			ioModule.WriteLine($"Quadratic norm = {quadNorm}", 1);
			ioModule.WriteLine($"Octagon norm = {octNorm}", 1);
			ioModule.WriteLine($"Euclidean norm = {eucNorm}", 1);
			ioModule.WriteLine($"|A| = {lu.GetSign() * lu.L.Determinant}");
			ioModule.SeparateText();

            //Нахождение невязки
            Vector B_diff = lu.A * _X - B.FastSwap(lu.m);

            ioModule.WriteLine("A * X - B =");
			ioModule.WriteLine(B_diff.ToString(), 1);
            ioModule.WriteLine("Error = ");
			ioModule.WriteLine((X - _X).ToString(), 1);*/

			ioModule.FileClose();

            Console.WriteLine("DONE!");
			Console.Read();
        }
    }
}