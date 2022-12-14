using System;
using Matrices;
using Vectors;
using NonlinearMethods;
using System.Collections.Generic;
using ApproximationTheory;
using Integration;
using BoundaryValueProblem;

namespace Lab1 {
	enum Tasks { 
		StraightMethods = 1,
		IterativeMethods = 2,
        NonlinearMethods = 3,
        ApproximationMethods = 4,
        NumericalIntegration = 5,
        BoundaryValueProblem = 6
    }

    class Program {

        static IOModule io;

        static void StraightMethods()
        {
            io.ExcelInit();

            int n;
            io.GetSize(out n);

            Matrix A = new Matrix(n, n);
            Vector X = new Vector(n);

            io.GetMatrixInfo(A, "A");
            io.GetVectorInfo(X, "X");

            Vector B = A * X;

            io.TerminateExcel();
            io.FileOpen();

            io.WriteLine("INPUT");
            io.WriteLine("X=");
            io.WriteLine(X.ToString(), 1);
            io.WriteLine("A=");
            io.WriteLine(A.ToString(), 1);
            io.WriteLine("B=");
            io.WriteLine(B.ToString(), 1);
            io.SeparateText();

            Methods.LU lu = new Methods.LU(A, B, n, io);

            Vector _X = lu.X;
            Matrix X_diff = (lu.L * lu.U) - lu.A;

            io.WriteLine("X_Eval =");
            io.WriteLine(_X.ToString(), 1);
            io.WriteLine("L * U - P * A =");
            io.WriteLine(X_diff.ToString(), 1);
            io.SeparateText();

            //Нахождение обратной матрицы
            Matrix A_INVERSE = lu.U.Inverse * lu.L.Inverse * lu.E.FastSwap(lu.m);
            //Проверка
            Matrix A_diff = lu.A * A_INVERSE - lu.P;

            io.WriteLine("A=");
            io.WriteLine(lu.A.ToString(), 1);
            io.WriteLine("A^(-1)=");
            io.WriteLine(A_INVERSE.ToString(), 1);
            io.WriteLine("A * A^(-1) - E =");
            io.WriteLine(A_diff.ToString(), 1);
            io.SeparateText();

            //Нахождение норм
            double quadNorm = lu.A.QuadricNorm * A_INVERSE.QuadricNorm;
            double octNorm = lu.A.OctoNorm * A_INVERSE.OctoNorm;
            double eucNorm = lu.A.EuclideNorm;

            io.WriteLine("Cond(A) = ");
            io.WriteLine($"Quadratic norm = {quadNorm}", 1);
            io.WriteLine($"Octagon norm = {octNorm}", 1);
            io.WriteLine($"Euclidean norm = {eucNorm}", 1);
            io.WriteLine($"|A| = {lu.GetSign() * lu.L.Determinant}");
            io.SeparateText();

            //Нахождение невязки
            Vector B_diff = lu.A * _X - B.FastSwap(lu.m);

            io.WriteLine("A * X - B =");
            io.WriteLine(B_diff.ToString(), 1);
            io.WriteLine("Error = ");
            io.WriteLine((X - _X).ToString(), 1);

            io.FileClose();
        }

        static void IterativeMethods()
        {
            io.ExcelInit();

            double eps = 0.0001;
            int n;

            io.GetSize(out n);

            Matrix A = new Matrix(n, n);
            Vector B = new Vector(n);

            io.GetMatrixInfo(A, "A");
            io.GetVectorInfo(B, "B");

            io.TerminateExcel();
            io.FileOpen();

            io.WriteLine("INPUT");
            io.WriteLine("A=");
            io.WriteLine(A.ToString(), 1);
            io.WriteLine("B=");
            io.WriteLine(B.ToString(), 1);
            io.SeparateText();

            Methods.SSRM ssrm = new Methods.SSRM(A, B);

            io.WriteLine("Straight squareroot method result:");
            io.WriteLine(ssrm.Answer.ToString());
            io.WriteLine($"Matrix norm: {A.EuclideNorm}");
            io.SeparateText();

            io.WriteLine("Simple iteration method:");

            Methods.SIM sim = new Methods.SIM(A, B, eps, io);

            io.SeparateText();

            io.WriteLine("Fastest Gradient Descent method:");

            Methods.FGDM fgdm = new Methods.FGDM(A, B, eps, io);

            io.SeparateText();

            io.WriteLine("SOR method:");

            Methods.SOR sor = new Methods.SOR(A, B, ssrm.Answer, eps, io);

            io.SeparateText();

            io.WriteLine("Conjugate Gradient method:");

            Methods.CGM cgm = new Methods.CGM(A, B, ssrm.Answer, eps, io);

            io.SeparateText();

            io.WriteLine($"cond(A) = {A.EuclideCondition}");
            io.WriteLine("Theoretical number of iterations:");
            io.WriteLine($"SOR method = {sor.Theoretical}");
            io.WriteLine($"Conjugate Gradient method = {cgm.Theoretical}");

            io.FileClose();
        }
        static void NonlinearMethods() {
            //Преднастройки
            IFunctions[,] funcs = new IFunctions[3, 5] {
                { new F1(), new F2(), new Fi1(), new Fi2(), new F() },
				{ new _F1_28(), new _F2_28(), new _Fi1_28(), new _Fi2_28(), new _F_28() },
				{ new _F1_2(), new _F2_2(), new _Fi1_2(), new _Fi2_2(), new _F_2() }
			};

            double[,] roughAns = {
                { 0.5, -0.2 },
				{ 0.4, 0.6 },
				{ 3.4, 1.2   }
            };

            int length = funcs.GetUpperBound(0) + 1;

			io.FileOpen();

			for(int i = 0; i < length; i++) {
                io.WriteLine($"ПРИМЕР {i}");

                double eps = 1E-4;
                double alpha = 1;
                double lambda = 0.5;

                IFunctions f1 = funcs[i, 0];
                IFunctions f2 = funcs[i, 1];
                IFunctions fi1 = funcs[i, 2];
                IFunctions fi2 = funcs[i, 3];
                IFunctions f = funcs[i, 4];

                Vector X = new Vector(2);
                X[0] = roughAns[i, 0];
                X[1] = roughAns[i, 1];

				io.WriteLine($"M0 = ({roughAns[i, 0]},{roughAns[i, 1]})");
				io.WriteLine($"Alpha = {alpha}, Lambda = {lambda}");
                io.SeparateText();

				io.WriteLine("Newton's method");
                io.WriteLine();

                IFunctions[] NMFuncs = { f1, f2 };

                NonlinearMethods.NM nm = new NonlinearMethods.NM(X, io, NMFuncs);

                io.SeparateText();

                io.WriteLine("Simple Iteration method");
                io.WriteLine();

                IFunctions[] SIMFuncs = { f1, f2, fi1, fi2 };

                NonlinearMethods.SIM sim = new NonlinearMethods.SIM(nm.Answer, X, io, eps, SIMFuncs);

                io.SeparateText();

                io.WriteLine("Gradient Descend Method");
                io.WriteLine();

                IFunctions[] GDFuncs = { f1, f2, f };

                GD gd = new GD(X, nm.Answer, GDFuncs, alpha, lambda, eps, io);

                io.SeparateText();
            }
		}

        static void ApproximationMethods() {
            const int a = 1;
            const int b = 2;
            const int n = 5;

            IFunction func_30 = new F_28();

            io.FileOpen();
            io.WriteLine("Newton method");
            ApproximationTheory.NewtonMethod nm = new ApproximationTheory.NewtonMethod(func_30, a, b, n, io);
            io.SeparateText();

            io.WriteLine("CubicSplines");
            ApproximationTheory.CubicSplinesMethod csm = new ApproximationTheory.CubicSplinesMethod(func_30, a, b, n, io);
            io.SeparateText();

            io.WriteLine("Discrete RMS Approximation");
            ApproximationTheory.DRMSA drmsa = new ApproximationTheory.DRMSA(func_30, a, b, n, io);
            io.SeparateText();

            io.WriteLine("Continuous RMS Approximation");
            ApproximationTheory.CRMSA crmsa = new ApproximationTheory.CRMSA(func_30, a, b, n, io);
            io.SeparateText();

            io.WriteLine("Uniform approximation");
            ApproximationTheory.UniformApproximationP2 uniform = new ApproximationTheory.UniformApproximationP2(func_30, a, b, n, io);
            io.SeparateText();

            io.WriteLine($"Reverse Root");
            ApproximationTheory.ReverseRootMethod reverse = new ApproximationTheory.ReverseRootMethod(func_30, a, b, (func_30.Solve(a) + func_30.Solve(b)) / 2, n, io);
        }

        static void NumericalIntegration() {
            double eps = 1E-08;
            double exactVal = 7.46143535976102;
            double a = 1;
            double b = 2;

            ApproximationTheory.IFunction func_30 = new F_28();

            io.FileOpen();

            io.WriteLine("J = " + exactVal);
            io.SeparateText();

            io.WriteLine("Trapezoid Formula");
            Trapezoid tr = new Trapezoid(func_30, a, b, exactVal, eps, io);
            io.SeparateText();

            io.WriteLine("Approximated Trapezoid Formula");
            TrapezoidSpline trs = new TrapezoidSpline(func_30, a, b, exactVal, eps, io);
            io.SeparateText();

            io.WriteLine("Simpson's Formula");
            SimpsonIntegration simp = new SimpsonIntegration(func_30, exactVal, a, b, eps, io);
            io.SeparateText();

            io.WriteLine("Gauss Formula");
            Gauss gs = new Gauss(func_30, exactVal, a, b, eps, io);
        }

        static void BoundaryValue() {
            int var = 2;
            ABC abc = new ABC2();
            double eps = 1E-4;

            io.FileOpen();

            io.WriteLine("Figing Method");
            FiringMethod fm = new FiringMethod(eps, var, abc, io);
            io.SeparateText();

            io.WriteLine("Finite Differences Explicit");
            FiniteDifferencesExplicit fde = new FiniteDifferencesExplicit(var, io);
            io.SeparateText();

            io.WriteLine("Finite Differences Implicit");
            FiniteDifferencesImplicit fdi = new FiniteDifferencesImplicit(var, io);
        }

        static void TaskSwitch(int task) {
            switch ((Tasks)task) {
                case Tasks.StraightMethods: {
                    StraightMethods();
                } break;

                case Tasks.IterativeMethods: {
                    IterativeMethods();
                } break;

                case Tasks.NonlinearMethods: {
                    NonlinearMethods();
                } break;

                case Tasks.ApproximationMethods: {
                    ApproximationMethods();
				} break;

                case Tasks.NumericalIntegration: {
                    NumericalIntegration();
                } break;

                case Tasks.BoundaryValueProblem: {
                    BoundaryValue();
                }
                break;

                default: {
                    Console.WriteLine("Wrong program");
                } break;
            }
        }

        static void Main(string[] args) {
            io = new IOModule();

            Console.WriteLine("Choose program:");
            Console.WriteLine("1 - Straight Methods (Lab 1)");
            Console.WriteLine("2 - Iterative Methods (Lab 2)");
            Console.WriteLine("3 - Nonlinear Methods (Lab3)");
            Console.WriteLine("4 - Approximation Methods (Lab4)");
			Console.WriteLine("5 - Numerical Integration (Lab5)");
            Console.WriteLine("6 - Boundary Value Problem(Lab6)");
            Console.Write("Enter number: ");

            Console.WriteLine();

            int task;

            if (int.TryParse(Console.ReadLine(), out task)) {
                TaskSwitch(task);
            }

            Console.WriteLine("DONE!");
            Console.Read();
        }
    }
}